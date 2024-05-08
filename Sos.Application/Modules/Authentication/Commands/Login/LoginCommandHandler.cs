using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Cryptography;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Email;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Authentication;
using Sos.Contracts.Email;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.Core.Errors;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Domain.UserAggregate.Services;
using Sos.Domain.UserAggregate.ValueObjects;

namespace Sos.Application.Modules.Authentication.Commands.Login
{
    /// <summary>
    /// Represents the <see cref="LoginCommand"/> handler.
    /// </summary>
    public sealed class LoginCommandHandler : ICommandHandler<LoginCommand, Result<TokenResponse>>
    {
        private const string Subject = "Xác minh tài khoản trong ứng dụng SOS";
        private const int ExprireMinutes = 3;

        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHashCheckerService _passwordHashCheckerService;
        private readonly IJwtProvider _jwtProvider;
        private readonly IVerifyCodeGenerator _verifyCodeGenerator;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="passwordHashCheckerService">The password hash checker service.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        /// <param name="verifyCodeGenerator">The verify code generator.</param>
        /// <param name="emailService">The email service.</param>
        public LoginCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHashCheckerService passwordHashCheckerService,
            IJwtProvider jwtProvider,
            IVerifyCodeGenerator verifyCodeGenerator,
            IEmailService emailService
        )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHashCheckerService = passwordHashCheckerService;
            _jwtProvider = jwtProvider;
            _verifyCodeGenerator = verifyCodeGenerator;
            _emailService = emailService;
        }

        // <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            Result<Email> emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<TokenResponse>(GeneralError.InvalidEmailOrPassword);
            }

            Maybe<User> maybeUser = await _userRepository.GetByEmailAsync(emailResult.Value);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<TokenResponse>(GeneralError.InvalidEmailOrPassword);
            }

            User user = maybeUser.Value;

            bool isPasswordCorrect = user.VerifyPasswordHash(request.Password, _passwordHashCheckerService);

            if (!isPasswordCorrect)
            {
                return Result.Failure<TokenResponse>(GeneralError.InvalidEmailOrPassword);
            }

            if (user.VerifiedAt == null)
            {
                if (user.VerifyCode == null
                    || (user.VerifyCode != null && user.VerifyCodeExpired < DateTime.Now))
                {
                    user.VerifyCode = _verifyCodeGenerator.Generate();

                    user.VerifyCodeExpired = DateTime.Now.AddMinutes(ExprireMinutes);

                    _userRepository.Update(user);

                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    await _emailService.SendEmailAsync(new MailRequest(
                        user.Email!.Value,
                        Subject,
                        user.VerifyCode!
                    ));
                }

                return Result.Failure<TokenResponse>(GeneralError.UserNotVerified);
            }

            string accessToken = _jwtProvider.GenerateAccessToken(user);

            string refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new TokenResponse(accessToken, refreshToken));
        }
    }
}
