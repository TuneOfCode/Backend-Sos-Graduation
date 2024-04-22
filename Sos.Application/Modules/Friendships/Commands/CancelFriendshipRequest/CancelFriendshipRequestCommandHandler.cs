using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.FriendshipAggregate.Errors;
using Sos.Domain.FriendshipAggregate.Repositories;

namespace Sos.Application.Modules.Friendships.Commands.CancelFriendshipRequest
{
    public sealed class CancelFriendshipRequestCommandHandler
        : ICommandHandler<CancelFriendshipRequestCommand, Result>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelFriendshipRequestCommandHandler"/> class.
        /// </summary>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        /// <param name="friendshipRequestRepository">The friendship request repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CancelFriendshipRequestCommandHandler(
            IUserIdentifierProvider userIdentifierProvider,
        IFriendshipRequestRepository friendshipRequestRepository,
            IUnitOfWork unitOfWork
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _friendshipRequestRepository = friendshipRequestRepository;
            _unitOfWork = unitOfWork;
        }

        // < inheritdoc />
        public async Task<Result> Handle(CancelFriendshipRequestCommand request, CancellationToken cancellationToken)
        {
            Maybe<FriendshipRequest> maybeFriendshipRequest = await _friendshipRequestRepository.GetByIdAsync(request.FriendshipRequestId);

            if (maybeFriendshipRequest.HasNoValue)
            {
                return Result.Failure(FriendshipDomainError.FriendshipRequestNotFound);
            }

            FriendshipRequest friendshipRequest = maybeFriendshipRequest.Value;

            if (friendshipRequest.SenderId != _userIdentifierProvider.UserId)
            {
                return Result.Failure(FriendshipDomainError.SenderIsNotCurrentUser);
            }

            _friendshipRequestRepository.Remove(friendshipRequest);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
