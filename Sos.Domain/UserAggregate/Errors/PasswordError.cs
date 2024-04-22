using Sos.Domain.Core.Commons.Bases;

namespace Sos.Domain.UserAggregate.Errors
{
    /// <summary>
    /// Contains the password errors.
    /// </summary>
    public static class PasswordError
    {
        public static Error NullOrEmpty =>
            new
            (
                "Password.NullOrEmpty",
                "Mật khẩu bắt buộc phải có.",
                "The password is required."
            );

        public static Error TooShort =>
            new
            (
                "Password.TooShort",
                "Mật khẩu quá ngắn.",
                "The password is too short."
            );

        public static Error MissingUppercaseLetter =>
            new
            (
                "Password.MissingUppercaseLetter",
                "Mật khẩu phải có ít nhất một ký tự hoa.",
                "The password requires at least one uppercase letter."
            );

        public static Error MissingLowercaseLetter =>
            new
            (
                "Password.MissingLowercaseLetter",
                "Mật khẩu phải có ít nhất một ký tự thường.",
                "The password requires at least one lowercase letter."
            );

        public static Error MissingDigit =>
            new
            (
                "Password.MissingDigit",
                "Mật khẩu phải có ít nhất một ký tự số.",
                "The password requires at least one digit."
            );

        public static Error MissingNonAlphaNumeric =>
            new
            (
                "Password.MissingNonAlphaNumeric",
                "Mật khẩu phải có ít nhất một ký tự đặc biệt.",
                "The password requires at least one non-alphanumeric."
            );
    }
}
