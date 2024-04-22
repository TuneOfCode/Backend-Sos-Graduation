using Sos.Domain.Core.Abstractions;
using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.FriendshipAggregate.Enums;
using Sos.Domain.FriendshipAggregate.Errors;
using Sos.Domain.FriendshipAggregate.Events;
using Sos.Domain.FriendshipAggregate.ValueObjects;
using Sos.Domain.UserAggregate;

namespace Sos.Domain.FriendshipAggregate
{
    /// <summary>
    /// Represents the friendship request.
    /// </summary>
    public sealed class FriendshipRequest : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipRequest"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private FriendshipRequest()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FriendshipRequest"/> class.
        /// </summary>
        /// <param name="sender">The sender value.</param>
        /// <param name="receiver">The receiver value.</param>
        /// <param name="status">The status value of request.</param>
        public FriendshipRequest(User sender, User receiver, Status status)
            : base(Guid.NewGuid())
        {
            Ensure.NotNull(sender, "The sender is required.", nameof(sender));
            Ensure.NotEmpty(sender.Id, "The sender identifier is required.", $"{nameof(sender)}{nameof(sender.Id)}");
            Ensure.NotNull(receiver, "The receiver is required.", nameof(receiver));
            Ensure.NotEmpty(receiver.Id, "The receiver identifier is required.", $"{nameof(receiver)}{nameof(receiver.Id)}");

            SenderId = sender.Id;
            ReceiverId = receiver.Id;
            StatusRequest = status;

            AddDomainEvent(new FriendshipRequestCreatedDomainEvent(this));
        }

        /// <summary>
        /// Gets the sender identifier.
        /// </summary>
        public Guid SenderId { get; private set; }


        /// <summary>
        /// Gets the receiver identifier.
        /// </summary>
        public Guid ReceiverId { get; private set; }

        /// <summary>
        /// Gets the status of the request.
        /// </summary>
        public Status? StatusRequest { get; set; }

        /// <inheritdoc />
        public DateTime CreatedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? ModifiedOnUtc { get; }

        /// <inheritdoc />
        public DateTime? DeletedOnUtc { get; }

        /// <inheritdoc />
        public bool Deleted { get; }

        /// <summary>
        /// Accepts the request.
        /// </summary>
        /// <returns>The result of the accepting operation.</returns>
        public Result Accept()
        {
            if (StatusRequest!.Value == StatusEnum.Accepted)
            {
                Result.Failure(FriendshipDomainError.AlreadyAccepted);
            }

            if (StatusRequest!.Value == StatusEnum.Rejected)
            {
                Result.Failure(FriendshipDomainError.AlreadyRejected);
            }

            StatusRequest = Status.Create(StatusEnum.Accepted).Value;

            AddDomainEvent(new FriendshipRequestAcceptedDomainEvent(this));

            return Result.Success();
        }

        /// <summary>
        /// Rejects the request.
        /// </summary>
        /// <returns>The result of the rejecting operation.</returns>
        public Result Reject()
        {
            if (StatusRequest!.Value == StatusEnum.Accepted)
            {
                Result.Failure(FriendshipDomainError.AlreadyAccepted);
            }

            if (StatusRequest!.Value == StatusEnum.Rejected)
            {
                Result.Failure(FriendshipDomainError.AlreadyRejected);
            }

            StatusRequest = Status.Create(StatusEnum.Rejected).Value;

            AddDomainEvent(new FriendshipRequestRejectedDomainEvent(this));

            return Result.Success();
        }
    }
}
