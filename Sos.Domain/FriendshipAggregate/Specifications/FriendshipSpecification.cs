using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.UserAggregate;
using System.Linq.Expressions;

namespace Sos.Domain.FriendshipAggregate.Specifications
{
    /// <summary>
    /// Represents the specification for determining the friendship of two users.
    /// </summary>
    public sealed class FriendshipSpecification : Specification<Friendship>
    {
        private readonly Guid _userId;
        private readonly Guid _friendId;

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipSpecification"/> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="friend">The friend.</param>
        public FriendshipSpecification(User user, User friend)
        {
            _userId = user.Id;
            _friendId = friend.Id;
        }

        internal override Expression<Func<Friendship, bool>> ToExpression()
            => friendship => friendship.UserId == _userId && friendship.FriendId == _friendId;
    }
}
