namespace Sos.Domain.Core.Abstractions
{
    /// <summary>
    /// Represents the marker interface for soft-deletable entities.
    /// </summary>
    public interface ISoftDeletableEntity
    {
        /// <summary>
        /// Gets the date and time in the entity was deleted on.
        /// </summary>
        DateTime? DeletedAt { get; }

        /// <summary>
        /// Gets a value indicating whether the entity has been deleted.
        /// </summary>
        bool Deleted { get; }
    }
}
