using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Authentication.Errors
{
    /// <summary>
    /// Contains the verify errors.
    /// </summary>
    public static class VerifyError
    {
        public static Error EmailIsRequired =>
            new
            (
                "VerifyError.EmailIsRequired",
                "Địa chỉ email bắt buộc phải có.",
                "The email is required."
            );

        public static Error CodeIsRequired =>
            new
            (
                "VerifyError.EmailIsRequired",
                "Mã xác thực bắt buộc phải có.",
                "The Code is required."
            );
    }
}
