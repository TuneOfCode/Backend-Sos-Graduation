using Sos.Domain.Core.Commons.Bases;

namespace Sos.Domain.UserAggregate.Errors
{
    /// <summary>
    /// Contains the avatar errors.
    /// </summary>
    public class AvatarError
    {

        public static Error BiggerThanMaxSize =>
            new
            (
                "Avatar.BiggerThanMaxSize",
                "Kích thước quá lớn.",
                "The avatar is bigger than the maximum size."
            );

        public static Error NotAllowedExtension =>
            new
            (
                "Avatar.NotAllowedExtension",
                "Dạng tệp không hợp lệ.",
                "The avatar is not allowed extension."
            );
    }
}
