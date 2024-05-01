namespace Sos.Contracts.Authentication
{
    /// <summary>
    /// Reprensents the verify request.
    /// </summary>
    public record VerifyRequest(
        string Email,
        string Code
    );
}
