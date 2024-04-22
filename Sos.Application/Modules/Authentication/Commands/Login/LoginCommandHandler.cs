using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Authentication;
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
    /// <remarks>
    /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
    /// </remarks>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="passwordHashCheckerService">The password hash checker service.</param>
    /// <param name="jwtProvider">The jwt provider.</param>
    public sealed class LoginCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHashCheckerService passwordHashCheckerService,
        IJwtProvider jwtProvider
    ) : ICommandHandler<LoginCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHashCheckerService _passwordHashCheckerService = passwordHashCheckerService;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

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

            string accessToken = _jwtProvider.GenerateAccessToken(user);

            string refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new TokenResponse(accessToken, refreshToken));
        }
    }
}
