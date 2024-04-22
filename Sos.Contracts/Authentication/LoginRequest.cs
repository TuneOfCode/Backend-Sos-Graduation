namespace Sos.Contracts.Authentication
{
    /// <summary>
    /// Request for user login.
    /// </summary>
    /// <param name="Email">The email request.</param>
    /// <param name="Password">The password request.</param>
    public record LoginRequest(string Email, string Password);
}
