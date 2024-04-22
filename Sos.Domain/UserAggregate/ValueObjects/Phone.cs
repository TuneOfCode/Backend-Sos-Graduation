using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate.Enums;
using Sos.Domain.UserAggregate.Errors;
using System.Text.RegularExpressions;

namespace Sos.Domain.UserAggregate.ValueObjects
{
    /// <summary>
    /// Represents the phone value object.
    /// </summary>
    public class Phone : ValueObject
    {
        private static readonly Lazy<Regex> VietnamPhoneFormatRegex =
            new(() => new Regex(UserRequirementEnum.VietnamPhoneRegexPattern, RegexOptions.Compiled));

        /// <summary>
        /// Initializes a new instance of the <see cref="Phone"/> class.
        /// </summary>
        /// <param name="value"></param>
        private Phone(string value) => Value = value;

        /// <summary>
        /// Gets the phone value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(Phone phone) => phone.Value;

        /// <summary>
        /// Creates a new <see cref="Phone"/> instance based on the specified value.
        /// </summary>
        /// <param name="phone">The phone value.</param>
        /// <returns>The result of the phone creattion process containing the phone or an error.</returns>
        public static Result<Phone> Create(string phone) =>
            Result.Create(phone, PhoneError.NullOrEmpty)
                .Ensure(p => !string.IsNullOrWhiteSpace(p), PhoneError.NullOrEmpty)
                .Ensure(p => p.Length >= UserRequirementEnum.MinPhoneLength, PhoneError.ShorterThanAllowed)
                .Ensure(p => p.Length <= UserRequirementEnum.MaxPhoneLength, PhoneError.LongerThanAllowed)
                .Ensure(p => VietnamPhoneFormatRegex.Value.IsMatch(p), PhoneError.InvalidVietnamPhoneFormat)
                .Map(p => new Phone(p));

        // <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
