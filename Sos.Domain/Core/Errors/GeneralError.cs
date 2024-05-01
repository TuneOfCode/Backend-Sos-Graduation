using Sos.Domain.Core.Commons.Bases;

namespace Sos.Domain.Core.Errors
{
    public static class GeneralError
    {
        public static Error ServerError
            => new
            (
                "General.ServerError",
                "The server encountered an unrecoverable error."
            );

        public static Error UnProcessableRequest
            => new
            (
                "General.UnProcessableRequest",
                "The server could not process the request."
            );

        public static Error InvalidEmailOrPassword
            => new
            (
                "Authentication.InvalidEmailOrPassword",
                "Tài khoản hoặc mật khẩu không chính xác.",
                "The wrong email or password."
            );

        public static Error UserNotVerified
            => new
            (
                "Authentication._UserNotVerified",
                "Vui lòng xác minh tài khoản từ email.",
                "Please verify your account."
            );
    }
}
