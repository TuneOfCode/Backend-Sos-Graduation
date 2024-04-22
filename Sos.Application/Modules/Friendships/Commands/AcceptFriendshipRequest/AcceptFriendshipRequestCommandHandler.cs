using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.FriendshipAggregate.Enums;
using Sos.Domain.FriendshipAggregate.Errors;
using Sos.Domain.FriendshipAggregate.Repositories;
using Sos.Domain.FriendshipAggregate.Services;

namespace Sos.Application.Modules.Friendships.Commands.AcceptFriendshipRequest
{
    public sealed class AcceptFriendshipRequestCommandHandler : ICommandHandler<AcceptFriendshipRequestCommand, Result>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;
        private readonly IFriendshipService _friendshipService;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptFriendshipRequestCommandHandler"/> class.
        /// </summary>
        /// <param name="userIdentifierProvider">The user identitifer.</param>
        /// <param name="friendshipRequestRepository">The friendship request repository.</param>
        /// <param name="friendshipService">The friendship service.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public AcceptFriendshipRequestCommandHandler(
            IUserIdentifierProvider userIdentifierProvider,
            IFriendshipRequestRepository friendshipRequestRepository,
            IFriendshipService friendshipService,
            IUnitOfWork unitOfWork
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _friendshipRequestRepository = friendshipRequestRepository;
            _friendshipService = friendshipService;
            _unitOfWork = unitOfWork;
        }

        // < inheritdoc />
        public async Task<Result> Handle(AcceptFriendshipRequestCommand request, CancellationToken cancellationToken)
        {
            Maybe<FriendshipRequest> maybeFriendshipRequest = await _friendshipRequestRepository.GetByIdAsync(request.FriendshipRequestId);

            if (maybeFriendshipRequest.HasNoValue)
            {
                return Result.Failure(FriendshipDomainError.FriendshipRequestNotFound);
            }

            FriendshipRequest friendshipRequest = maybeFriendshipRequest.Value;

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

            await _friendshipService.CreateFriendshipAsync(friendshipRequest);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
