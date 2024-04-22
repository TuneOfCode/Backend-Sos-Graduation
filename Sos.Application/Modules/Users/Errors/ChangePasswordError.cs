using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Users.Errors
{
    /// <summary>
    /// Contains the change password errors.
    /// </summary>
    public static class ChangePasswordError
    {
        public static Error UserIdIsRequired =>
            new
            (
                "ChangePassword.UserIdIsRequired",
                "The user id is required."
            );

        public static Error PasswordIsRequired =>
            new
            (
                "ChangePassword.PasswordIsRequired",
                "The password id is required."
            );

        public static Error CurrentPasswordIsRequired =>
            new
            (
                "ChangePassword.CurrentPasswordIsRequired",
                "The current password is required."
            );

        public static Error PasswordCannotBeTheSameAsCurrentPassword =>
            new
            (
                "ChangePassword.PasswordCannotBeTheSameAsCurrentPassword",
                "The password cannot be the same as the current password."
            );

        public static Error ConfirmPasswordIsRequired =>
            new
            (
                "ChangePassword.ConfirmPasswordIsRequired",
                "The confirm password is required."
            );

        public static Error ConfirmPasswordDoesNotMatch =>
            new
            (
                "ChangePassword.ConfirmPasswordDoesNotMatch",
                "The confirm password does not match."
            );
    }
}
