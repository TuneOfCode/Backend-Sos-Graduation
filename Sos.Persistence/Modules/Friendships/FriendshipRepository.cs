using Sos.Application.Core.Abstractions.Data;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.FriendshipAggregate.Repositories;
using Sos.Domain.FriendshipAggregate.Specifications;
using Sos.Domain.UserAggregate;
using Sos.Persistence.Data;

namespace Sos.Persistence.Modules.Friendships
{
    /// <summary>
    /// Represents the friendship repository.
    /// </summary>
    public sealed class FriendshipRepository : GenericRepository<Friendship>, IFriendshipRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public FriendshipRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }

        // < inheritdoc />
        public async Task<bool> CheckIfFriendshipExistsAsync(User user, User friend)
            => await AnyAsync(new FriendshipSpecification(user, friend));
    }
}
