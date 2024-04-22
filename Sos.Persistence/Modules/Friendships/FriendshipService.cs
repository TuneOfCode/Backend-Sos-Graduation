using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.Core.Exceptions;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.FriendshipAggregate.Enums;
using Sos.Domain.FriendshipAggregate.Errors;
using Sos.Domain.FriendshipAggregate.Repositories;
using Sos.Domain.FriendshipAggregate.Services;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Repositories;

namespace Sos.Persistence.Modules.Friendships
{
    /// <summary>
    /// Represents the friendship service.
    /// </summary>
    public sealed class FriendshipService : IFriendshipService
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="friendshipRepository">The friendship repository.</param>
        /// <param name="friendshipRequestRepository">The friendship request repository.</param>
        public FriendshipService(
            IUserRepository userRepository,
            IFriendshipRepository friendshipRepository,
            IFriendshipRequestRepository friendshipRequestRepository
        )
        {
            _userRepository = userRepository;
            _friendshipRepository = friendshipRepository;
            _friendshipRequestRepository = friendshipRequestRepository;
        }

        // < inheritdoc />
        public async Task CreateFriendshipAsync(FriendshipRequest friendshipRequest)
        {
            if (friendshipRequest.StatusRequest!.Value == StatusEnum.Accepted)
            {
                throw new DomainException(FriendshipDomainError.AlreadyAccepted);
            }

            if (friendshipRequest.StatusRequest!.Value == StatusEnum.Rejected)
            {
                throw new DomainException(FriendshipDomainError.AlreadyRejected);
            }

            Maybe<User> maybeSender = await _userRepository.GetByIdAsync(friendshipRequest.SenderId);

            if (maybeSender.HasNoValue)
            {
                throw new DomainException(FriendshipDomainError.SenderNotFound);
            }

            Maybe<User> maybeReceiver = await _userRepository.GetByIdAsync(friendshipRequest.ReceiverId);

            if (maybeReceiver.HasNoValue)
            {
                throw new DomainException(FriendshipDomainError.ReceiverNotFound);
            }

            User sender = maybeSender.Value;

            User receiver = maybeReceiver.Value;

            friendshipRequest.Accept();

            _friendshipRequestRepository.Update(friendshipRequest);

            _friendshipRepository.Insert(new Friendship(sender, receiver));

            _friendshipRepository.Insert(new Friendship(receiver, sender));
        }

        // < inheritdoc />
        public async Task<Result> RemoveFriendshipAsync(Guid userId, Guid friendId)
        {
            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(userId);

            if (maybeUser.HasNoValue)
            {
                throw new DomainException(FriendshipDomainError.UserNotFound);
            }

            Maybe<User> maybeFriend = await _userRepository.GetByIdAsync(friendId);

            if (maybeFriend.HasNoValue)
            {
                throw new DomainException(FriendshipDomainError.FriendNotFound);
            }

            User user = maybeUser.Value;

            User friend = maybeFriend.Value;

            Maybe<FriendshipRequest> maybeFriendshipRequest = await _friendshipRequestRepository
                .GetFriendshipRequestAsync(user, friend);

            if (maybeFriendshipRequest.HasNoValue)
            {
                throw new DomainException(FriendshipDomainError.FriendshipRequestNotFound);
            }

            FriendshipRequest friendshipRequest = maybeFriendshipRequest.Value;

            _friendshipRequestRepository.Remove(friendshipRequest);

            _friendshipRepository.Remove(new Friendship(user, friend));

            _friendshipRepository.Remove(new Friendship(friend, user));

            return Result.Success();
        }
    }
}
