﻿using Microsoft.EntityFrameworkCore;
using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Common;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.UserAggregate;

namespace Sos.Application.Modules.Friendships.Queries.GetFriendshipByUserId
{
    public sealed class GetFriendshipByUserIdQueryHandler
        : IQueryHandler<GetFriendshipByUserIdQuery,
            Maybe<PaginateList<FriendshipResponse>>>
    {
        private const int _defaultPage = 1;
        private const int _defaultPageSize = 10;

        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IDbContext _dbContext;

        public GetFriendshipByUserIdQueryHandler(
            IUserIdentifierProvider userIdentifierProvider,
            IDbContext dbContext
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _dbContext = dbContext;
        }

        // < inheritdoc />
        public async Task<Maybe<PaginateList<FriendshipResponse>>> Handle(GetFriendshipByUserIdQuery request, CancellationToken cancellationToken)
        {
            int page = request.Page;
            int pageSize = request.PageSize;

            //if (request.UserId != _userIdentifierProvider.UserId
            //    || page < 1)
            //{
            //    return Maybe<PaginateList<FriendshipResponse>>.None;
            //}

            if (page == default || page < 1)
            {
                page = _defaultPage;
            }

            if (pageSize == default)
            {
                pageSize = _defaultPageSize;
            }

            var friendshipResponseQuery =
                from friendship in _dbContext.Set<Friendship>().AsNoTracking()
                join user in _dbContext.Set<User>().AsNoTracking()
                    on friendship.UserId equals user.Id
                join friend in _dbContext.Set<User>().AsNoTracking()
                    on friendship.FriendId equals friend.Id
                where friendship.UserId == request.UserId
                orderby friend.FirstName
                select new FriendshipResponse(
                    user.Id,
                    user.FullName!,
                    user.Email!,
                    user.Avatar!.AvatarUrl,
                    friend.Id,
                    friend.FullName!,
                    friend.Email!,
                    friend.Avatar!.AvatarUrl,
                    friendship.CreatedOnUtc,
                    friendship.ModifiedOnUtc
                );

            int totalCount = await friendshipResponseQuery.CountAsync(cancellationToken);

            var data = await friendshipResponseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToArrayAsync(cancellationToken);

            var response = new PaginateList<FriendshipResponse>(
                data,
                page,
                pageSize,
                totalCount
            );

            return response;
        }
    }
}
