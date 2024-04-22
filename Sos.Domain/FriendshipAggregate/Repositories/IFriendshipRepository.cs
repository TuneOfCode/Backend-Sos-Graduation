using Sos.Domain.UserAggregate;

namespace Sos.Domain.FriendshipAggregate.Repositories
{
    /// <summary>
    /// Represents the friendship repository interface.
    /// </summary>
    public interface IFriendshipRepository
    {
        /// <summary>
        /// Checks if a friendship exists between two users.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="friend">The friend.</param>
        /// <returns>True if the specified users are friends, otherwise false.</returns>
        Task<bool> CheckIfFriendshipExistsAsync(User user, User friend);

        /// <summary>
        /// Inserts the specified friendship to the database.
        /// </summary>
        /// <param name="friendship">The friendship to be inserted to the database.</param>
        void Insert(Friendship friendship);

        /// <summary>
        /// Removes the specified friendship from the database.
        /// </summary>
        /// <param name="friendship">The friendship to be removed    to the database.</param>
        void Remove(Friendship friendship);
    }
}
