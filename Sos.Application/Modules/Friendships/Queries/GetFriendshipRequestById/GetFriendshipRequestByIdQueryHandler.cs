using Microsoft.EntityFrameworkCore;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.UserAggregate;

namespace Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestById
{
    /// <summary>
    /// Represents a query for getting a friendship request by identifier.
    /// </summary>
    public sealed class GetFriendshipRequestByIdQueryHandler : IQueryHandler<GetFriendshipRequestByIdQuery, Maybe<FriendshipRequestResponse>>
    {
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initialize a new instance of the <see cref="GetFriendshipRequestByIdQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetFriendshipRequestByIdQueryHandler(
            IDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        // < inheritdoc />
        public async Task<Maybe<FriendshipRequestResponse>> Handle(GetFriendshipRequestByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.FriendshipRequestId == Guid.Empty)
            {
                return Maybe<FriendshipRequestResponse>.None;
            }

            var response = await (
                from friendshipRequest in _dbContext.Set<FriendshipRequest>().AsNoTracking()
                join sender in _dbContext.Set<User>().AsNoTracking()
                    on friendshipRequest.SenderId equals sender.Id
                join receiver in _dbContext.Set<User>().AsNoTracking()
                    on friendshipRequest.ReceiverId equals receiver.Id
                where friendshipRequest.Id == request.FriendshipRequestId
                select new FriendshipRequestResponse(
                    friendshipRequest.Id,
                    friendshipRequest.SenderId,
                    sender.FullName!,
                    sender.Avatar!.AvatarUrl,
                    friendshipRequest.ReceiverId,
                    receiver.FullName!,
                    receiver.Avatar!.AvatarUrl,
                    friendshipRequest.StatusRequest!.Value,
                    friendshipRequest.CreatedAt,
                    friendshipRequest.ModifiedAt
                 )
            ).SingleOrDefaultAsync(cancellationToken);

            if (response is null)
            {
                return Maybe<FriendshipRequestResponse>.None;
            }

            return response;
        }
    }
}
