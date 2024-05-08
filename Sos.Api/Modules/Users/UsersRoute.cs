namespace Sos.Api.Modules.Users
{
    /// <summary>
    /// Contains all routes for <see cref="UsersController"/>
    /// </summary>
    public static class UsersRoute
    {
        public const string GetUsers = "users";

        public const string GetById = "users/{userId:guid}";

        public const string Update = "users/{userId:guid}";

        public const string ChangePassword = "users/{userId:guid}/change-password";

        public const string UpdateLocation = "users/{userId:guid}/update-location";
    }
}
