using Microsoft.EntityFrameworkCore;
using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.UserAggregate;

namespace Sos.Application.Modules.Authentication.Queries
{
    public sealed class GetMeQueryHandler
        : IQueryHandler<GetMeQuery, Maybe<UserResponse>>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetMeQueryHandler"/> class.
        /// </summary>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        /// <param name="dbContext">The database context.</param>
        public GetMeQueryHandler(
            IUserIdentifierProvider userIdentifierProvider,
            IDbContext dbContext
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _dbContext = dbContext;
        }

        // < inheritdoc />
        public async Task<Maybe<UserResponse>> Handle(GetMeQuery request, CancellationToken cancellationToken)
        {
            var response = await _dbContext.Set<User>()
                .Where(u => u.Id == _userIdentifierProvider.UserId)
                .Select(user => new UserResponse(
                    user.Id,
                    user.FullName!,
                    user.FirstName!,
                    user.LastName!,
                    user.Email!.Value,
                    user.ContactPhone!.Value,
                    user.Avatar!.AvatarUrl,
                    user.Location!.Longitude,
                    user.Location!.Latitude,
                    user.VerifiedAt,
                    user.CreatedAt
                ))
                .SingleOrDefaultAsync(cancellationToken);

            if (response is null)
            {
                return Maybe<UserResponse>.None;
            }

            return response;
        }
    }
}
