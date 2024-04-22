using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Cryptography;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Errors;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Domain.UserAggregate.Services;
using Sos.Domain.UserAggregate.ValueObjects;

namespace Sos.Application.Modules.Users.Commands.ChangePassword
{
    /// <summary>
    /// Represents the <see cref="ChangePasswordCommand"/> handler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ChangePasswordCommandHandler"/> class.
    /// </remarks>
    /// <param name="userIdentifierProvider">The user identifier provider.</param>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="passwordHasher">The password hasher.</param>
    /// <param name="passwordHashCheckerService">The password hash checker service.</param>
    public sealed class ChangePasswordCommandHandler(
        IUserIdentifierProvider userIdentifierProvider,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher,
        IPasswordHashCheckerService passwordHashCheckerService) : ICommandHandler<ChangePasswordCommand, Result>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider = userIdentifierProvider;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly IPasswordHashCheckerService _passwordHashCheckerService = passwordHashCheckerService;

        /// <inheritdoc />
        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userIdentifierProvider.UserId)
            {
                return Result.Failure(UserDomainError.InvalidPermissions);
            }

            Result<Password> passwordResult = Password.Create(request.Password);

            if (passwordResult.IsFailure)
            {
                return Result.Failure(passwordResult.Error);
            }

            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(UserDomainError.NotFound);
            }

            User user = maybeUser.Value;

            bool isCurrentPasswordCorrect = user.VerifyPasswordHash(request.CurrentPassword, _passwordHashCheckerService);

            if (!isCurrentPasswordCorrect)
            {
                return Result.Failure(UserDomainError.InvalidCurrentPassword);
            }

            string passwordHash = _passwordHasher.HashPassword(passwordResult.Value);

            Result result = user.ChangePassword(passwordHash);

            if (result.IsFailure)
            {
                return Result.Failure(result.Error);
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
