using Sos.Domain.Core.Commons.Result;

namespace Sos.Domain.FriendshipAggregate.Services
{
    /// <summary>
    /// Represents the friendship service interface.
    /// </summary>
    public interface IFriendshipService
    {
        /// <summary>
        /// Creates a new friendship.
        /// </summary>
        /// <param name="friendshipRequest">The friendship request.</param>
        /// <returns></returns>
        Task CreateFriendshipAsync(FriendshipRequest friendshipRequest);

        /// <summary>
        /// Removes a friendship.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="friendId">The friend identifier.</param>
        /// <returns>The result of the operation.</returns>
        Task<Result> RemoveFriendshipAsync(Guid userId, Guid friendId);
    }
}
