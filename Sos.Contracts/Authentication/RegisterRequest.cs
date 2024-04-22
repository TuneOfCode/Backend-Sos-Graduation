namespace Sos.Contracts.Authentication
{
    /// <summary>
    /// Request for user registration
    /// </summary>
    public record RegisterRequest(
        string FirstName,
        string LastName,
        string Email,
        string ContactPhone,
        string Password,
        string ConfirmPassword
    );
}
