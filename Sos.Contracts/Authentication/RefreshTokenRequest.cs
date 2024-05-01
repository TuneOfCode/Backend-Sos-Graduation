namespace Sos.Contracts.Authentication
{
    public record RefreshTokenRequest(
        Guid UserId,
        string RefreshToken
    );
}
