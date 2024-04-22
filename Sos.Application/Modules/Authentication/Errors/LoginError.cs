using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Authentication.Errors
{
    /// <summary>
    /// Contains the login errors.
    /// </summary>
    public static class LoginError
    {
        public static Error EmailIsRequired =>
            new
            (
                "LoginError.EmailIsRequired",
                "Địa chỉ email bắt buộc phải có.",
                "The email is required."
            );

        public static Error PasswordIsRequired =>
            new
            (
                "LoginError.PasswordIsRequired",
                "Mật khẩu bắt buộc phải có.",
                "The password is required."
            );
    }
}
