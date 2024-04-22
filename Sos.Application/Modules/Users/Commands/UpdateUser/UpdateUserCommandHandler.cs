using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Enums;
using Sos.Domain.UserAggregate.Errors;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Domain.UserAggregate.ValueObjects;
using System.Net;

namespace Sos.Application.Modules.Users.Commands.UpdateUser
{
    /// <summary>
    /// Represents the update user command handler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="UpdateUserCommandHandler"/> class.
    /// </remarks>
    /// <param name="userRepository">The user repository.</param>
    /// <param name="unitOfWork">The unit of work.</param>
    public sealed class UpdateUserCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork
    ) : ICommandHandler<UpdateUserCommand, Result<UserResponse>>
    {
        private const string _defaultAvatarUrl = "/images/defaultAvatar.png";
        private const long _defaultAvatarSize = 0;

        private readonly IUserRepository _userRepository = userRepository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        // <inheritdoc/>
        public async Task<Result<UserResponse>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            string firstName = request.FirstName;
            string lastName = request.LastName;
            string contactPhone = request.ContactPhone;

            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<UserResponse>(UserDomainError.NotFound);
            }

            User user = maybeUser.Value;

            if (string.IsNullOrWhiteSpace(firstName))
            {
                firstName = user.FirstName!;
            }

            if (string.IsNullOrWhiteSpace(lastName))
            {
                lastName = user.LastName!;
            }

            Result<Phone> phoneResult = Phone.Create(contactPhone ?? user.ContactPhone!.Value);

            var avatarUrl = user.Avatar is null ? _defaultAvatarUrl : user.Avatar!.AvatarUrl;
            Result<Avatar> avatarResult = Avatar.Create(avatarUrl, _defaultAvatarSize);
            string uniqueFileName = null!;

            if (request.Avatar != null)
            {
                uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(request.Avatar.FileName)}";

                if (!Directory.Exists(UserRequirementEnum.StoragePath))
                {
                    Directory.CreateDirectory(UserRequirementEnum.StoragePath);
                }

                uniqueFileName = WebUtility.UrlEncode(uniqueFileName);

                avatarResult = Avatar.Create($"/images/{uniqueFileName}", request.Avatar.Length);
            }

            Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(phoneResult, avatarResult);

            if (firstFailureOrSuccess.IsFailure)
            {
                return Result.Failure<UserResponse>(firstFailureOrSuccess.Error);
            }

            user.Update(
                firstName,
                lastName,
                phoneResult.Value,
                avatarResult.Value
            );

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (!string.IsNullOrEmpty(uniqueFileName))
            {
                string filePath = Path.Combine(UserRequirementEnum.StoragePath, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await request.Avatar!.CopyToAsync(stream, cancellationToken);
            }

            return Result.Success(new UserResponse(
                user.Id,
                user.FullName!,
                user.FirstName!,
                user.LastName!,
                user.Email!.Value,
                user.ContactPhone!.Value,
                user.Avatar!.AvatarUrl,
                user.VerifiedOnUtc,
                user.CreatedOnUtc
            ));
        }
    }
}
