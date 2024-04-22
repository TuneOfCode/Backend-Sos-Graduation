using Microsoft.AspNetCore.Http;

namespace Sos.Contracts.Users
{
    /// <summary>
    /// Represents the update user request.
    /// </summary>
    public record UpdateUserRequest(
        string FirstName,
        string LastName,
        string ContactPhone,
        IFormFile Avatar
    );
}
