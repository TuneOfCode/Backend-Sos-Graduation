using Sos.Domain.Core.Commons.Bases;

namespace Sos.Domain.FriendshipAggregate.Errors
{
    /// <summary>
    /// Contains the status errors.
    /// </summary>
    public static class StatusError
    {
        public static Error NullOrEmpty =>
            new
            (
                "Status.NullOrEmpty",
                "The status value cannot be null or empty."
            );

        public static Error NotAllowedValue =>
            new
            (
                "Status.NotAllowedValue",
                "The status value is not allowed."
            );
    }
}
