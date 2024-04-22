using Microsoft.EntityFrameworkCore;
using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.FriendshipAggregate.Enums;
using Sos.Domain.UserAggregate;

namespace Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestBySender
{
    /// <summary>
    /// Represents a query handler for getting a list of friend requests by sender.
    /// </summary>
    public sealed class GetFriendshipRequestBySenderQueryHandler
        : IQueryHandler<GetFriendshipRequestBySenderQuery, Maybe<ListFriendshipRequestBySenderResponse>>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFriendshipRequestBySenderQueryHandler"/> class.
        /// </summary>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        /// <param name="dbContext">The database context.</param>
        public GetFriendshipRequestBySenderQueryHandler(
            IUserIdentifierProvider userIdentifierProvider,
            IDbContext dbContext
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _dbContext = dbContext;
        }

        // < inheritdoc />
        public async Task<Maybe<ListFriendshipRequestBySenderResponse>> Handle(GetFriendshipRequestBySenderQuery request, CancellationToken cancellationToken)
        {
            if (request.SenderId == Guid.Empty
                || request.SenderId != _userIdentifierProvider.UserId)
            {
                return Maybe<ListFriendshipRequestBySenderResponse>.None;
            }

            // Get friend requests via linq query syntax
            var friendshipRequests = await (
                from friendshipRequest in _dbContext.Set<FriendshipRequest>().AsNoTracking()
                join user in _dbContext.Set<User>().AsNoTracking()
                    on friendshipRequest.ReceiverId equals user.Id
                where friendshipRequest.SenderId == request.SenderId
                    && friendshipRequest.StatusRequest!.Value == StatusEnum.Pending
                select new FriendshipRequestBySenderResponse(
                    friendshipRequest.Id,
                    friendshipRequest.ReceiverId,
                    user.FullName!,
                    user.Avatar!.AvatarUrl,
                    friendshipRequest.StatusRequest!.Value,
                    friendshipRequest.CreatedOnUtc,
                    friendshipRequest.ModifiedOnUtc
                )
            ).ToArrayAsync(cancellationToken);

            //// Get friend requests via IEnumerable<T> query syntax
            //var users = await _dbContext.Set<User>().AsNoTracking().ToListAsync(cancellationToken);

            //var friendReqs = await _dbContext.Set<FriendshipRequest>()
            //    .AsNoTracking()
            //    .Where(fr => fr.SenderId == request.SenderId
            //        && fr.StatusRequest!.Value == StatusEnum.Pending)
            //    .Join(
            //        users,
            //        friendshipRequest => friendshipRequest.ReceiverId,
            //        user => user.Id,
            //        (friendshipRequest, user) => new FriendshipRequestBySenderResponse(
            //            friendshipRequest.Id,
            //            friendshipRequest.ReceiverId,
            //            user.FullName!,
            //            user.Avatar!.AvatarUrl,
            //            friendshipRequest.StatusRequest!.Value,
            //            friendshipRequest.CreatedOnUtc,
            //            friendshipRequest.ModifiedOnUtc
            //        )
            //).ToArrayAsync(cancellationToken);

            var response = new ListFriendshipRequestBySenderResponse(friendshipRequests);

            return response;
        }
    }
}
