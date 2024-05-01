using Sos.Application.Core.Abstractions.Cryptography;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Email;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Authentication;
using Sos.Contracts.Email;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Errors;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Domain.UserAggregate.ValueObjects;

namespace Sos.Application.Modules.Authentication.Commands.ReSendVerifyCode
{
    /// <summary>
    /// Represents the re-send verify code command handler.
    /// </summary>
    public sealed class ReSendVerifyCodeCommandHandler : ICommandHandler<ReSendVerifyCodeCommand, Result<ReSendVerifyCodeResponse>>
    {
        private const string Subject = "Xác minh tài khoản trong ứng dụng SOS";
        private const int ExprireMinutes = 3;

        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVerifyCodeGenerator _verifyCodeGenerator;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReSendVerifyCodeCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="verifyCodeGenerator">The verify code generator.</param>
        /// <param name="emailService">The email service.</param>
        public ReSendVerifyCodeCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IVerifyCodeGenerator verifyCodeGenerator,
            IEmailService emailService
        )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _verifyCodeGenerator = verifyCodeGenerator;
            _emailService = emailService;
        }

        public async Task<Result<ReSendVerifyCodeResponse>> Handle(ReSendVerifyCodeCommand request, CancellationToken cancellationToken)
        {
            Result<Email> emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure<ReSendVerifyCodeResponse>(EmailError.InvalidFormat);
            }

            Maybe<User> maybeUser = await _userRepository.GetByEmailAsync(emailResult.Value);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<ReSendVerifyCodeResponse>(UserDomainError.NotFound);
            }

            User user = maybeUser.Value;

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

            return Result.Success(new ReSendVerifyCodeResponse(
                user.VerifyCode!,
                user.VerifyCodeExpired!.Value
            ));
        }
    }
}
