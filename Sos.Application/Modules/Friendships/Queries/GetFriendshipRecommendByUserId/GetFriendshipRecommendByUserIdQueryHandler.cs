using Microsoft.EntityFrameworkCore;
using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Common;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.UserAggregate;

namespace Sos.Application.Modules.Friendships.Queries.GetFriendshipRecommendByUserId
{
    /// <summary>
    /// Represents the get friendship recommend by user id query handler.
    /// </summary>
    public sealed class GetFriendshipRecommendByUserIdQueryHandler
        : IQueryHandler<GetFriendshipRecommendByUserIdQuery,
            Maybe<PaginateList<UserResponse>>>
    {
        private const int _defaultPage = 1;
        private const int _defaultPageSize = 10;

        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetFriendshipRecommendByUserIdQueryHandler"/> class.
        /// </summary>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        /// <param name="dbContext">The database context.</param>
        public GetFriendshipRecommendByUserIdQueryHandler(
            IUserIdentifierProvider userIdentifierProvider,
            IDbContext dbContext
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _dbContext = dbContext;
        }

        // < inheritdoc />
        public async Task<Maybe<PaginateList<UserResponse>>> Handle(GetFriendshipRecommendByUserIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId != _userIdentifierProvider.UserId)
            {
                return Maybe<PaginateList<UserResponse>>.None;
            }

            int page = request.Page;
            int pageSize = request.PageSize;

            if (page == default || page < 1)
            {
                page = _defaultPage;
            }

            if (pageSize == default)
            {
                pageSize = _defaultPageSize;
            }

            var friendshipRequestExistedResponseQuery =
                from friendshipRequest in _dbContext.Set<FriendshipRequest>().AsNoTracking()
                where friendshipRequest.SenderId == request.UserId || friendshipRequest.ReceiverId == request.UserId
                select request.UserId == friendshipRequest.SenderId ? friendshipRequest.ReceiverId : friendshipRequest.SenderId;

            var friendshipRecommendResponseQuery =
                from stranger in _dbContext.Set<User>().AsNoTracking()
                join acquaintanceId in friendshipRequestExistedResponseQuery
                    on stranger.Id equals acquaintanceId into acquaintance
                from subAcquaintanceId in acquaintance.DefaultIfEmpty()
                where stranger.Id != request.UserId
                    && stranger.Id != subAcquaintanceId
                orderby stranger.FirstName
                select new UserResponse(
                    stranger.Id,
                    stranger.FullName!,
                    stranger.FirstName!,
                    stranger.LastName!,
                    stranger.Email!.Value,
                    stranger.ContactPhone!.Value,
                    stranger.Avatar!.AvatarUrl,
                    stranger.Location!.Longitude,
                    stranger.Location!.Latitude,
                    stranger.VerifiedAt,
                    stranger.CreatedAt
                );

            int totalCount = await friendshipRecommendResponseQuery.CountAsync(cancellationToken);

            var data = await friendshipRecommendResponseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync(cancellationToken);

            var response = new PaginateList<UserResponse>(
                data,
                page,
                pageSize,
                totalCount
            );

            return response;
        }
    }
}
