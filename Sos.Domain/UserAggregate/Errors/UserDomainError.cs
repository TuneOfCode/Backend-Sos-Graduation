using Sos.Domain.Core.Commons.Bases;

namespace Sos.Domain.UserAggregate.Errors
{
    /// <summary>
    /// Contains the user domain errors.
    /// </summary>
    public static class UserDomainError
    {
        public static Error NotFound =>
            new
            (
                "UserDomain.NotFound",
                "Không tìm thấy người dùng.",
                "The user was not found."
            );

        public static Error CannotChangePassword =>
            new
            (
                "UserDomain.CannotChangePassword",
                "Không cho thay đổi mật khẩu.",
                "The user password cannot be changed."
            );

        public static Error InvalidPermissions =>
            new
            (
                "UserDomain.InvalidPermissions",
                "Quyền của người dùng không hợp lệ.",
                "The user permissions are invalid."
            );

        public static Error EmailAlreadyExists =>
            new
            (
                "UserDomain.EmailAlreadyExists",
                "Địa chỉ email đã tồn tại.",
                "The user email already exists."
            );

        public static Error InvalidCurrentPassword =>
            new
            (
                "UserDomain.InvalidCurrentPassword",
                "Mật khẩu hiện tại không hợp lệ.",
                "The current password is invalid."
            );

        public static Error VerifyCodeIsNotMatch =>
            new
            (
                "UserDomain.VerifyCodeIsNotMatch",
                "Mã xác thực không chính xác hoặc đã hết hạn.",
                "The verify code is not match."
            );

        public static Error InvalidRefreshToken =>
            new
            (
                "UserDomain.InvalidRefreshToken",
                "Mã xác thực token không chính xác.",
                "The refresh token is invalid."
            );
    }
}
