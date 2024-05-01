namespace Sos.Contracts.Authentication
{
    /// <summary>
    /// Response for re-send verify code response.
    /// </summary>
    public record ReSendVerifyCodeResponse(
        string VerifyCode,
        DateTime VerifyCodeExpried
    );
}
