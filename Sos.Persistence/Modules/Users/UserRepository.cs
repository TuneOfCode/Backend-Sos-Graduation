using Sos.Application.Core.Abstractions.Data;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Domain.UserAggregate.Specifications;
using Sos.Domain.UserAggregate.ValueObjects;
using Sos.Persistence.Data;

namespace Sos.Persistence.Modules.Users
{
    /// <summary>
    /// Represents the user repository.
    /// </summary>
    public sealed class UserRepository : GenericRepository<User>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UserRepository(IDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <inheritdoc />
        public async Task<Maybe<User>> GetByEmailAsync(Email email)
            => (await FirstOrDefaultAsync(new UserWithEmailSpecification(email)))!;

        /// <inheritdoc />
        public async Task<bool> IsEmailUniqueAsync(Email email)
            => !await AnyAsync(new UserWithEmailSpecification(email));
    }
}
