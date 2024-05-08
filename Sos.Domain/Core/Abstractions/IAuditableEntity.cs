namespace Sos.Domain.Core.Abstractions
{
    /// <summary>
    /// Represents the marker interface for auditable entities.
    /// </summary>
    public interface IAuditableEntity
    {
        /// <summary>
        /// Gets the created on date and time.
        /// </summary>
        DateTime CreatedAt { get; }

        /// <summary>
        /// Gets the modified on date and time.
        /// </summary>
        DateTime? ModifiedAt { get; }
    }
}