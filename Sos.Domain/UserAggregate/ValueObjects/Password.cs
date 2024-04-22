using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate.Enums;
using Sos.Domain.UserAggregate.Errors;

namespace Sos.Domain.UserAggregate.ValueObjects
{
    /// <summary>
    /// Represents the password value object.
    /// </summary>
    public sealed class Password : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Password"/> class.
        /// </summary>
        /// <param name="value">The password value.</param>
        private Password(string value) => Value = value;

        /// <summary>
        /// Gets the password value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(Password password) => password?.Value ?? string.Empty;

        /// <summary>
        /// Creates a new <see cref="Password"/> instance based on the specified value.
        /// </summary>
        /// <param name="password">The password value.</param>
        /// <returns>The result of the password creation process containing the password or an error.</returns>
        public static Result<Password> Create(string password) =>
            Result.Create(password, PasswordError.NullOrEmpty)
                .Ensure(p => !string.IsNullOrWhiteSpace(p), PasswordError.NullOrEmpty)
                .Ensure(p => p.Length >= UserRequirementEnum.MinPasswordLength, PasswordError.TooShort)
                //.Ensure(p => p.Any(UserRequirementEnum.IsLower), PasswordError.MissingLowercaseLetter)
                //.Ensure(p => p.Any(UserRequirementEnum.IsUpper), PasswordError.MissingUppercaseLetter)
                //.Ensure(p => p.Any(UserRequirementEnum.IsDigit), PasswordError.MissingDigit)
                //.Ensure(p => p.Any(UserRequirementEnum.IsNonAlphaNumeric), PasswordError.MissingNonAlphaNumeric)
                .Map(p => new Password(p));

        /// <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
