using Microsoft.EntityFrameworkCore;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Common;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.UserAggregate;

namespace Sos.Application.Modules.Users.Queries.GetUsers
{
    public sealed class GetUsersQueryHandler
        : IQueryHandler<GetUsersQuery, Maybe<PaginateList<UserResponse>>>
    {
        private const int _defaultPage = 1;
        private const int _defaultPageSize = 10;

        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetUsersQueryHandler"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public GetUsersQueryHandler(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // < inheritdoc />
        public async Task<Maybe<PaginateList<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
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

            var userResponseQuery = _dbContext.Set<User>()
                .Select(u => new UserResponse(
                    u.Id,
                    u.FullName!,
                    u.FirstName!,
                    u.LastName!,
                    u.Email!,
                    u.ContactPhone!,
                    u.Avatar!.AvatarUrl,
                    u.VerifiedOnUtc,
                    u.CreatedOnUtc
                ));

            int totalCount = await userResponseQuery.CountAsync(cancellationToken);

            var data = await userResponseQuery
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

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
