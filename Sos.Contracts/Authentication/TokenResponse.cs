namespace Sos.Contracts.Authentication
{
    /// <summary>
    /// Response for token response
    /// </summary>
    /// <param name="accessToken">The access token</param>
    /// <param name="refreshToken">The refresh token</param>
    public record TokenResponse(string accessToken, string refreshToken);
}
