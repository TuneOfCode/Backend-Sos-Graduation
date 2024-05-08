using Microsoft.EntityFrameworkCore;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.UserAggregate;

namespace Sos.Application.Modules.Users.Queries.GetUserById
{
    /// <summary>
    /// Represents the get user by id query handler.
    /// </summary>
    /// <remarks>
    /// Initializes a new instance of the <see cref="GetUserByIdQueryHandler"/> class.
    /// </remarks>
    /// <param name="dbContext">The database context.</param>
    public sealed class GetUserByIdQueryHandler(
        IDbContext dbContext
    ) : IQueryHandler<GetUserByIdQuery, Maybe<UserResponse>>
    {
        private readonly IDbContext _dbContext = dbContext;

        // <inheritdoc />   
        public async Task<Maybe<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if (request.UserId == Guid.Empty)
            {
                return Maybe<UserResponse>.None;
            }

            var response = await _dbContext.Set<User>()
                .Where(u => u.Id == request.UserId)
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
