namespace Sos.Contracts.Users
{
    /// <summary>
    /// Represents the change password request.
    /// </summary>
    public record ChangePasswordRequest(
        string CurrentPassword,
        string Password,
        string ConfirmPassword
    );
}
