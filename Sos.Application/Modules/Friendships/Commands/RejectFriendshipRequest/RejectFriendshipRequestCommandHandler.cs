using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.FriendshipAggregate.Enums;
using Sos.Domain.FriendshipAggregate.Errors;
using Sos.Domain.FriendshipAggregate.Repositories;

namespace Sos.Application.Modules.Friendships.Commands.RejectFriendshipRequest
{
    public sealed class RejectFriendshipRequestCommandHandler : ICommandHandler<RejectFriendshipRequestCommand, Result>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="RejectFriendshipRequestCommandHandler"/> class.
        /// </summary>
        /// <param name="userIdentifierProvider">The user identitifer.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="friendshipRequestRepository">The friendship request repository.</param>
        /// <param name="friendshipService">The friendship service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public RejectFriendshipRequestCommandHandler(
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
        public async Task<Result> Handle(RejectFriendshipRequestCommand request, CancellationToken cancellationToken)
        {
            Maybe<FriendshipRequest> maybeFriendshipReqest = await _friendshipRequestRepository.GetByIdAsync(request.FriendshipRequestId);

            if (maybeFriendshipReqest.HasNoValue)
            {
                return Result.Failure(FriendshipDomainError.FriendshipRequestNotFound);
            }

            FriendshipRequest friendshipRequest = maybeFriendshipReqest.Value;

            if (friendshipRequest.ReceiverId != _userIdentifierProvider.UserId)
            {
                return Result.Failure(FriendshipDomainError.NotAllowedToAccept);
            }

            if (friendshipRequest.StatusRequest!.Value == StatusEnum.Accepted)
            {
                return Result.Failure(FriendshipDomainError.AlreadyAccepted);
            }

            if (friendshipRequest.StatusRequest!.Value == StatusEnum.Rejected)
            {
                return Result.Failure(FriendshipDomainError.AlreadyRejected);
            }

            friendshipRequest.Reject();

            _friendshipRequestRepository.Update(friendshipRequest);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
