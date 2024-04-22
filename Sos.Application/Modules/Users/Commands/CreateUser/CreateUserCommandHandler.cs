using Sos.Application.Core.Abstractions.Cryptography;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Errors;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Domain.UserAggregate.ValueObjects;

namespace Sos.Application.Modules.Users.Commands.CreateUser
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> handler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
    /// </remarks>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    /// <param name="passwordHasher">The password hasher.</param>
    public sealed class CreateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IPasswordHasher passwordHasher
    ) : ICommandHandler<CreateUserCommand, Result<UserResponse>>
    {
        private const string _defaultAvatarUrl = "/images/defaultAvatar.png";
        private const long _defaultAvatarSize = 0;

        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        // <inheritdoc />
        public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Result<Email> emailResult = Email.Create(request.Email);
            Result<Phone> phoneResult = Phone.Create(request.ContactPhone);
            Result<Password> passwordResult = Password.Create(request.Password);

            Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(emailResult, phoneResult, passwordResult);

            if (firstFailureOrSuccess.IsFailure)
            {
                return Result.Failure<UserResponse>(firstFailureOrSuccess.Error);
            }

            bool isEmailUnique = await _userRepository.IsEmailUniqueAsync(emailResult.Value);
            if (!isEmailUnique)
            {
                return Result.Failure<UserResponse>(UserDomainError.EmailAlreadyExists);
            }

            string passwordHash = _passwordHasher.HashPassword(passwordResult.Value);

            var newUser = User.Create(
                request.FirstName,
                request.LastName,
                emailResult.Value,
                phoneResult.Value,
                passwordHash
            );

            Result<Avatar> avatarResult = Avatar.Create(_defaultAvatarUrl, _defaultAvatarSize);

            newUser.Avatar = avatarResult.Value;

            _userRepository.Insert(newUser);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new UserResponse(
                newUser.Id,
                newUser.FullName!,
                newUser.FirstName!,
                newUser.LastName!,
                newUser.Email!.Value,
                newUser.ContactPhone!.Value,
                newUser.Avatar!.AvatarUrl,
                newUser.VerifiedOnUtc,
                newUser.CreatedOnUtc
            ));
        }
    }
}
