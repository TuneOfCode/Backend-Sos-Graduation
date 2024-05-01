using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Authentication.Errors
{
    /// <summary>
    /// Contains the refresh token errors.
    /// </summary>
    public static class RefreshTokenError
    {
        public static Error UserIsRequired =>
            new
            (
                "RefreshTokenError.UserIsRequired",
                "Mã người dùng bắt buộc phải có.",
                "The user identifier is required."
            );

        public static Error RefreshTokenIsRequired =>
            new
            (
                "RefreshTokenError.RefreshTokenIsRequired",
                "Mã token xác thực bắt buộc phải có.",
                "The refresh token is required."
            );
    }
}
