using Microsoft.EntityFrameworkCore;
using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.FriendshipAggregate.Enums;
using Sos.Domain.UserAggregate;

namespace Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestByReceiver
{
    /// <summary>
    /// Represents the query handler for getting the list of friendship request by receiver.
    /// </summary>
    public sealed class GetFriendshipRequestByReceiverQueryHandler
        : IQueryHandler<GetFriendshipRequestByReceiverQuery, Maybe<ListFriendshipRequestByReceiverResponse>>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFriendshipRequestByReceiverQueryHandler"/> class.
        /// </summary>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        /// <param name="dbContext">The database context.</param>
        public GetFriendshipRequestByReceiverQueryHandler(
            IUserIdentifierProvider userIdentifierProvider,
            IDbContext dbContext
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _dbContext = dbContext;
        }

        // < inheritdoc />
        public async Task<Maybe<ListFriendshipRequestByReceiverResponse>> Handle(GetFriendshipRequestByReceiverQuery request, CancellationToken cancellationToken)
        {
            if (request.ReceiverId == Guid.Empty
                || request.ReceiverId != _userIdentifierProvider.UserId)
            {
                return Maybe<ListFriendshipRequestByReceiverResponse>.None;
            }

            var friendshipRequests = await (
                from friendshipRequest in _dbContext.Set<FriendshipRequest>().AsNoTracking()
                join user in _dbContext.Set<User>().AsNoTracking()
                    on friendshipRequest.SenderId equals user.Id
                where friendshipRequest.ReceiverId == request.ReceiverId
                    && friendshipRequest.StatusRequest!.Value == StatusEnum.Pending
                select new FriendshipRequestByReceiverResponse(
                    friendshipRequest.Id,
                    friendshipRequest.SenderId,
                    user.FullName!,
                    user.Avatar!.AvatarUrl,
                    friendshipRequest.StatusRequest!.Value,
                    friendshipRequest.CreatedAt,
                    friendshipRequest.ModifiedAt
                )
            ).ToArrayAsync(cancellationToken);

            var response = new ListFriendshipRequestByReceiverResponse(friendshipRequests);

            return response;
        }
    }
}
