namespace Sos.Domain.UserAggregate.Enums
{
    /// <summary>
    /// Represents the user requirement enum.
    /// </summary>
    public static class UserRequirementEnum
    {
        public const int MinFirstNameLength = 2;

        public const int MaxFirstNameLength = 50;

        public const int MinLastNameLength = 2;

        public const int MaxLastNameLength = 150;

        public const int MaxEmailLength = 256;

        public const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        public const int MinPhoneLength = 7;

        public const int MaxPhoneLength = 15;

        public const string VietnamPhoneRegexPattern = @"^0[0-9]{9,10}$";

        public const int MinPasswordLength = 6;

        public static readonly Func<char, bool> IsLower = c => c >= 'a' && c <= 'z';

        public static readonly Func<char, bool> IsUpper = c => c >= 'A' && c <= 'Z';

        public static readonly Func<char, bool> IsDigit = c => c >= '0' && c <= '9';

        public static readonly Func<char, bool> IsNonAlphaNumeric = c
            => !(IsLower(c) || IsUpper(c) || IsDigit(c));

        public const long MaxSizeFile = 5 * 1024 * 1024; // 5MB

        public static string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".tiff" };

        public static string StoragePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        public static string MediaStoragePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "medias");
    }
}
