namespace Sos.Contracts.Users
{
    /// <summary>
    /// Respresents the user response.
    /// </summary>
    public record UserResponse(
        Guid UserId,
        string FullName,
        string FirstName,
        string LastName,
        string Email,
        string ContactPhoneNumber,
        string? Avatar,
        DateTime? VerifiedOnUtc,
        DateTime? CreatedOnUtc
    );
}
