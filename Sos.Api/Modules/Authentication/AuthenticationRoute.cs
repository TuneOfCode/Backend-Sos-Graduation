namespace Sos.Api.Modules.Authentication
{
    /// <summary>
    /// Contains all routes for <see cref="AuthenticationController"/>
    /// </summary>
    public static class AuthenticationRoute
    {
        public const string Login = "authentication/login";

        public const string Register = "authentication/register";

        public const string GetMe = "authentication/me";
    }
}
