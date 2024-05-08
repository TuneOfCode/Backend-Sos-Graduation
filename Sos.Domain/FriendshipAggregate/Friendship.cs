using Sos.Domain.Core.Abstractions;
using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.UserAggregate;

namespace Sos.Domain.FriendshipAggregate
{
    /// <summary>
    /// Represents the friendship entity.
    /// </summary>
    public sealed class Friendship : Entity, IAuditableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Friendship"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private Friendship()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Friendship"/> class.
        /// </summary>
        /// <param name="user">The user value.</param>
        /// <param name="friend">The friend value.</param>
        public Friendship(User user, User friend)
        {
            Ensure.NotNull(user, "The user is required.", nameof(user));
            Ensure.NotEmpty(user.Id, "The user identifier is required.", $"{nameof(user)}{nameof(user.Id)}");
            Ensure.NotNull(friend, "The friend is required.", nameof(friend));
            Ensure.NotEmpty(friend.Id, "The friend identifier is required.", $"{nameof(friend)}{nameof(friend.Id)}");

            UserId = user.Id;
            FriendId = friend.Id;
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        public Guid UserId { get; private set; }

        /// <summary>
        /// Gets the friend identifier.
        /// </summary>
        public Guid FriendId { get; }

        /// <inheritdoc />
        public DateTime CreatedAt { get; }

        /// <inheritdoc />
        public DateTime? ModifiedAt { get; }
    }
}
