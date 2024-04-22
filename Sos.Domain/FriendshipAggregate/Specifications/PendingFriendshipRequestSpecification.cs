using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.FriendshipAggregate.Enums;
using Sos.Domain.UserAggregate;
using System.Linq.Expressions;

namespace Sos.Domain.FriendshipAggregate.Specifications
{
    /// <summary>
    /// Represents the specification for determining the pending friendship request.
    /// </summary>
    public sealed class PendingFriendshipRequestSpecification : Specification<FriendshipRequest>
    {
        private readonly Guid _senderId;
        private readonly Guid _receiverId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PendingFriendshipRequestSpecification"/> class.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="receiver">The receiver.</param>
        public PendingFriendshipRequestSpecification(User sender, User receiver)
        {
            _senderId = sender.Id;
            _receiverId = receiver.Id;
        }

        // <inheritdoc />
        internal override Expression<Func<FriendshipRequest, bool>> ToExpression()
            => friendshipRequest
                => (friendshipRequest.SenderId == _senderId
                        && friendshipRequest.ReceiverId == _receiverId)
                    || (friendshipRequest.ReceiverId == _senderId
                        && friendshipRequest.SenderId == _receiverId)
                    && friendshipRequest.StatusRequest!.Value == StatusEnum.Pending;
    }
}
