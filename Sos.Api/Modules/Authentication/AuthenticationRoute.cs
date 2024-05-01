namespace Sos.Api.Modules.Authentication
{
    /// <summary>
    /// Contains all routes for <see cref="AuthenticationController"/>
    /// </summary>
    public static class AuthenticationRoute
    {
        public const string Register = "authentication/register";

        public const string Login = "authentication/login";

        public const string Verify = "authentication/verify";

        public const string ReSendVerifyCode = "authentication/resend-verify-code";

        public const string RefreshToken = "authentication/refresh-token";

        public const string GetMe = "authentication/me";
    }
}
