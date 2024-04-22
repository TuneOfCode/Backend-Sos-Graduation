using Sos.Domain.Core.Commons.Bases;

namespace Sos.Domain.UserAggregate.Errors
{
    /// <summary>
    /// Contains the email errors.
    /// </summary>
    public static class EmailError
    {
        public static Error NullOrEmpty =>
            new
            (
                "Email.NullOrEmpty",
                "Địa chỉ email bắt buộc phải có.",
                "The email is required."
            );

        public static Error LongerThanAllowed =>
            new
            (
                "Email.LongerThanAllowed",
                "Địa chỉ email quá dài.",
                "The email is longer than allowed."
            );

        public static Error InvalidFormat =>
            new
            (
                "Email.InvalidFormat",
                "Địa chỉ email không hợp lệ.",
                "The email format is invalid."
            );
    }
}
