using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.UserAggregate;

namespace Sos.Domain.FriendshipAggregate.Repositories
{
    /// <summary>
    /// Represents the friendship request repository interface.
    /// </summary>
    public interface IFriendshipRequestRepository
    {
        /// <summary>
        /// Gets the friend request with the specified identifier.
        /// </summary>
        /// <param name="friendshipRequestId">The friendship request identifier.</param>
        /// <returns>The maybe instance that may contain the friendship request with the specified identifier.</returns>
        Task<Maybe<FriendshipRequest>> GetByIdAsync(Guid friendshipRequestId);

        /// <summary>
        /// Checks if the specified users have a pending friendship request.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="receiver">The receiver.</param>
        /// <returns>True if the specified users have a pending friendship request, otherwise false.</returns>
        Task<bool> CheckFriendshipRequestIsPendingAsync(User sender, User receiver);

        /// <summary>
        /// Gets the friendship request between the specified users.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="receiver">The receiver.</param>
        /// <returns></returns>
        Task<FriendshipRequest> GetFriendshipRequestAsync(User sender, User receiver);

        /// <summary>
        /// Inserts the specified friendship request to the database.
        /// </summary>
        /// <param name="friendshipRequest">The friendship request to be inserted to the database.</param>
        void Insert(FriendshipRequest friendshipRequest);

        /// <summary>
        /// Updates the specified friendship request in the database.
        /// </summary>
        /// <param name="friendshipRequest">The friendship request to be updated in the database.</param>
        void Update(FriendshipRequest friendshipRequest);

        /// <summary>
        /// Removes the specified friendship request from the database.
        /// </summary>
        /// <param name="friendshipRequest">Thhhe friendship request to be removed from the database.</param>
        void Remove(FriendshipRequest friendshipRequest);
    }
}
