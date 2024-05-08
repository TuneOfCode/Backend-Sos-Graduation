using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Errors;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Domain.UserAggregate.ValueObjects;

namespace Sos.Application.Modules.Authentication.Commands.Verify
{
    /// <summary>
    /// Represents the verify command handler.
    /// </summary>
    public sealed class VerifyCommandHandler : ICommandHandler<VerifyCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerifyCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public VerifyCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork
        )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        // < inheritdoc />
        public async Task<Result> Handle(VerifyCommand request, CancellationToken cancellationToken)
        {
            Result<Email> emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return Result.Failure(EmailError.InvalidFormat);
            }

            Maybe<User> maybeUser = await _userRepository.GetByEmailAsync(emailResult.Value);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(UserDomainError.NotFound);
            }

            User user = maybeUser.Value;

            bool isVerifyCodeChecker = user.VerifyCodeChecker(request.Code);

            if (!isVerifyCodeChecker)
            {
                return Result.Failure(UserDomainError.VerifyCodeIsNotMatch);
            }

            user.VerifiedAt = DateTime.Now;

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
