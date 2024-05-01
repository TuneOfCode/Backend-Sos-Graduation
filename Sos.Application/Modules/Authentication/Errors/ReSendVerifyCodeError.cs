using Sos.Domain.Core.Commons.Bases;

namespace Sos.Application.Modules.Authentication.Errors
{
    /// <summary>
    /// Contains the re-send verify code errors.
    /// </summary>
    public static class ReSendVerifyCodeError
    {
        public static Error EmailIsRequired =>
           new
           (
               "ReSendVerifyCodeError.EmailIsRequired",
               "Địa chỉ email bắt buộc phải có.",
               "The email is required."
           );
    }
}
