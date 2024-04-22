using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.FriendshipAggregate.Errors;
using Sos.Domain.FriendshipAggregate.Repositories;
using Sos.Domain.FriendshipAggregate.Services;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Repositories;

namespace Sos.Application.Modules.Friendships.Commands.RemoveFriendship
{
    public sealed class RemoveFriendshipCommandHandler
        : ICommandHandler<RemoveFriendshipCommand, Result>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IFriendshipService _friendshipService;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveFriendshipCommandHandler"/> class.
        /// </summary>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        /// <param name="friendshipRepository">The friendship repository.</param>
        /// <param name="friendshipService">The friendship service.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public RemoveFriendshipCommandHandler(
            IUserIdentifierProvider userIdentifierProvider,
            IFriendshipRepository friendshipRepository,
            IFriendshipService friendshipService,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _friendshipRepository = friendshipRepository;
            _friendshipService = friendshipService;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        // < inheritdoc />
        public async Task<Result> Handle(RemoveFriendshipCommand request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userIdentifierProvider.UserId)
            {
                return Result.Failure(FriendshipDomainError.NotAllowedToRemove);
            }

            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(FriendshipDomainError.UserNotFound);
            }

            Maybe<User> maybeFriend = await _userRepository.GetByIdAsync(request.FriendId);

            if (maybeFriend.HasNoValue)
            {
                return Result.Failure(FriendshipDomainError.FriendNotFound);
            }

            User user = maybeFriend.Value;

            User friend = maybeUser.Value;

            bool isFriend = await _friendshipRepository.CheckIfFriendshipExistsAsync(user, friend);

            if (!isFriend)
            {
                return Result.Failure(FriendshipDomainError.FriendNotFound);
            }

            await _friendshipService.RemoveFriendshipAsync(user.Id, friend.Id);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
