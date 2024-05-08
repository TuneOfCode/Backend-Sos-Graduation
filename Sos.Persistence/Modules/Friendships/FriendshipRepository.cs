using Microsoft.EntityFrameworkCore;
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
        private readonly IDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public FriendshipRepository(IDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }

        // < inheritdoc />
        public async Task<bool> CheckIfFriendshipExistsAsync(User user, User friend)
            => await AnyAsync(new FriendshipSpecification(user, friend));

        // < inheritdoc />
        public async Task<IReadOnlyList<Friendship>> GetFriendshipsAsync(User user)
        {
            var friendships = await (
                from friendship in _dbContext.Set<Friendship>().AsNoTracking()
                join userDB in _dbContext.Set<User>().AsNoTracking()
                    on friendship.UserId equals userDB.Id
                join friend in _dbContext.Set<User>().AsNoTracking()
                    on friendship.FriendId equals friend.Id
                where friendship.UserId == user.Id
                orderby friend.FirstName
                select new Friendship(userDB, friend)
            ).ToListAsync();

            return friendships;
        }
    }
}
