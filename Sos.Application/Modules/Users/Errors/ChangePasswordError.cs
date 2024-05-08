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
                "Mã người dùng bắt buộc phải có.",
                "The user id is required."
            );

        public static Error PasswordIsRequired =>
            new
            (
                "ChangePassword.NewPasswordIsRequired",
                "Mật khẩu mới bắt buộc phải có.",
                "The new password is required."
            );

        public static Error CurrentPasswordIsRequired =>
            new
            (
                "ChangePassword.CurrentPasswordIsRequired",
                "Mật khẩu hiện tại bắt buộc phải có.",
                "The current password is required."
            );

        public static Error PasswordCannotBeTheSameAsCurrentPassword =>
            new
            (
                "ChangePassword.NewPasswordCannotBeTheSameAsCurrentPassword",
                "Mật khẩu mới không được trùng với mật khẩu hiện tại.",
                "The password cannot be the same as the current password."
            );

        public static Error ConfirmPasswordIsRequired =>
            new
            (
                "ChangePassword.ConfirmPasswordIsRequired",
                "Nhập lại mật khẩu mới bắt buộc phải có.",
                "The confirm password is required."
            );

        public static Error ConfirmPasswordDoesNotMatch =>
            new
            (
                "ChangePassword.ConfirmPasswordDoesNotMatch",
                "Nhập lại mật khẩu mới không chính xác.",
                "The confirm password does not match."
            );
    }
}
