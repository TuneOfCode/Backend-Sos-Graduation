using Sos.Domain.UserAggregate;

namespace Sos.Application.Core.Abstractions.Authentication
{
    /// <summary>
    /// Represents the JWT provider interface.
    /// </summary>
    public interface IJwtProvider
    {
        /// <summary>
        /// Generates the JWT access token.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>The JWT token.</returns>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Generates the refresh token.
        /// </summary>
        /// <returns>The refresh token.</returns>
        string GenerateRefreshToken();
    }
}
