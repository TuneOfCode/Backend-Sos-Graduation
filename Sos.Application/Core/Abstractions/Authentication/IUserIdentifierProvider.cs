namespace Sos.Application.Core.Abstractions.Authentication
{
    /// <summary>
    /// Represents the user identifier provider interface.
    /// </summary>
    public interface IUserIdentifierProvider
    {
        /// <summary>
        /// Gets the authenticated user identifier.
        /// </summary>
        Guid UserId { get; }

        /// <summary>
        /// Gets the authenticated user role application.
        /// </summary>
        string? Role { get; }
    }
}
