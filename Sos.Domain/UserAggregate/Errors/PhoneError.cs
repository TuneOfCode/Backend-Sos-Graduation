using Sos.Domain.Core.Commons.Bases;

namespace Sos.Domain.UserAggregate.Errors
{
    /// <summary>
    /// Containing the phone errors.
    /// </summary>
    public static class PhoneError
    {
        public static Error NullOrEmpty =>
            new
            (
                "Phone.NullOrEmpty",
                "Số điện thoại bắt buộc phải có.",
                "The phone is required."
            );

        public static Error ShorterThanAllowed =>
            new
            (
                "Phone.ShorterThanAllowed",
                "Số điện thoại quá ngắn.",
                "The phone is shorter than allowed."
            );

        public static Error LongerThanAllowed =>
            new
            (
                "Phone.LongerThanAllowed",
                "Số điện thoại quá dài.",
                "The phone is longer than allowed."
            );

        public static Error InvalidVietnamPhoneFormat =>
            new
            (
                "Phone.InvalidVietnamPhoneFormat",
                "Số điện thoại ở khu vực Việt Nam không hợp lệ.",
                "The Vietnam phone format is invalid."
            );
    }
}
