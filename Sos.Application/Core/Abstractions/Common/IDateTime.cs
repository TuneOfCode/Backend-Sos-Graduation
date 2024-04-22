namespace Sos.Application.Core.Abstractions.Common
{
    /// <summary>
    /// Represents the interface for getting the current date and time.
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Gets the current date and time in UTC format.
        /// </summary>
        DateTime UtcNow { get; }

        /// <summary>
        /// Gets the current date and time in local format.
        /// </summary>
        DateTime LocalNow { get; }
    }
}
