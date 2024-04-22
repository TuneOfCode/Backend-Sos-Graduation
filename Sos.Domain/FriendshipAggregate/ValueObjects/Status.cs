using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.FriendshipAggregate.Enums;
using Sos.Domain.FriendshipAggregate.Errors;

namespace Sos.Domain.FriendshipAggregate.ValueObjects
{
    /// <summary>
    /// Represents the status of a friend request
    /// </summary>
    public sealed class Status : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Status"/> class.
        /// </summary>
        /// <param name="value">The status value</param>
        private Status(string value) => Value = value;

        public string Value { get; }

        public static implicit operator string(Status status) => status.Value;

        /// <summary>
        /// Creates a new instance of the <see cref="Status"/> class.
        /// </summary>
        /// <param name="status">The status value.</param>
        /// <returns>The result of the status creattion process containing the status or an error.</returns>
        public static Result<Status> Create(string status) =>
            Result.Create(status, StatusError.NullOrEmpty)
                .Ensure(s => !string.IsNullOrWhiteSpace(s), StatusError.NullOrEmpty)
                .Ensure(s => s == StatusEnum.Pending
                       || s == StatusEnum.Accepted
                       || s == StatusEnum.Rejected, StatusError.NotAllowedValue)
                .Map(s => new Status(s));


        // <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
