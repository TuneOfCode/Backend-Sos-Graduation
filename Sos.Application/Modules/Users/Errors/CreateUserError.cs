
using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Users.Errors
{
    /// <summary>
    /// Contains the create user errors.
    /// </summary>
    public static class CreateUserError
    {
        public static Error FirstNameIsRequired =>
            new
            (
                "CreateUserError.FirstNameIsRequired",
                "Tên bắt buộc phải có.",
                "The first name is required."
            );

        public static Error FirstNameIsTooShort =>
            new
            (
                "CreateUserError.FirstNameIsTooShort",
                "Tên quá ngắn.",
                "The first name is too short."
            );

        public static Error FirstNameIsTooLong =>
            new
            (
                "CreateUserError.FirstNameIsTooLong",
                "Tên quá dài.",
                "The first name is too long."
            );

        public static Error LastNameIsRequired =>
            new
            (
                "CreateUserError.LastNameIsRequired",
                "Họ và tên đệm bắt buộc phải có.",
                "The last name is required."
            );

        public static Error LastNameIsTooShort =>
            new
            (
                "CreateUserError.LastNameIsTooShort",
                "Họ và tên đệm quá ngắn.",
                "The last name is too short."
            );

        public static Error LastNameIsTooLong =>
            new
            (
                "CreateUserError.LastNameIsTooLong",
                "Họ và tên đệm quá dài.",
                "The last name is too long."
            );

        public static Error EmailIsRequired =>
            new
            (
                "CreateUserError.EmailIsRequired",
                "Email bắt buộc phải có.",
                "The email is required."
            );

        public static Error ContactPhoneIsRequired =>
            new
            (
                "CreateUserError.ContactPhoneIsRequired",
                "Số điện thoại bắt buộc phải có.",
                "The contact phone is required."
            );
        public static Error PasswordIsRequired =>
            new
            (
                "CreateUserError.PasswordIsRequired",
                "Mật khẩu bắt buộc phải có.",
                "The password is required."
            );

        public static Error ConfirmPasswordIsRequired =>
            new
            (
                "CreateUserError.ConfirmPwdIsRequired",
                "Nhập lại mật khẩu bắt buộc phải có.",
                "The confirm password is required."
            );

        public static Error ConfirmPasswordDoNotMatch =>
            new
            (
                "CreateUserError.ConfirmPwdDoNotMatch",
                "Nhập lại mật khẩu không chính xác.",
                "The confirm password does not match."
            );
    }
}
