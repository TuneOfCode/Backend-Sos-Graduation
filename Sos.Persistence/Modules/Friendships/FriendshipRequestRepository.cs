using Sos.Application.Core.Abstractions.Data;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.FriendshipAggregate.Repositories;
using Sos.Domain.FriendshipAggregate.Specifications;
using Sos.Domain.UserAggregate;
using Sos.Persistence.Data;

namespace Sos.Persistence.Modules.Friendships
{
    /// <summary>
    /// Represents the friendship request repository.
    /// </summary>
    public class FriendshipRequestRepository : GenericRepository<FriendshipRequest>, IFriendshipRequestRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipRequestRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public FriendshipRequestRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }

        // < inheritdoc />
        public async Task<bool> CheckFriendshipRequestIsPendingAsync(User sender, User receiver)
            => await AnyAsync(new PendingFriendshipRequestSpecification(sender, receiver));

        public async Task<FriendshipRequest> GetFriendshipRequestAsync(User sender, User receiver)
            => (await FirstOrDefaultAsync(new FriendshipRequestSpecification(sender, receiver)))!;
    }
}
