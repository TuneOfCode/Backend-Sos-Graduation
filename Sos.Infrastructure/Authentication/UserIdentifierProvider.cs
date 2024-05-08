using Microsoft.AspNetCore.Http;
using Sos.Application.Core.Abstractions.Authentication;
using System.Security.Claims;

namespace Sos.Infrastructure.Authentication
{
    /// <summary>
    /// Represents the user identifier provider.
    /// </summary>
    internal sealed class UserIdentifierProvider : IUserIdentifierProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserIdentifierProvider"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        public UserIdentifierProvider(IHttpContextAccessor httpContextAccessor)
        {
            string userIdClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimsType.UserId)
                ?? throw new ArgumentException("The user identifier claim is required.", nameof(httpContextAccessor));

            string roleAppClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimsType.RoleApp) ?? throw new ArgumentException("The user role app claim is required.", nameof(httpContextAccessor));

            string roleClaim = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Role)!;

            Console.WriteLine($"===> role claim in token: {roleClaim}");

            UserId = new Guid(userIdClaim);

            Role = roleAppClaim;
        }

        /// <inheritdoc />
        public Guid UserId { get; }

        public string? Role { get; }
    }
}
