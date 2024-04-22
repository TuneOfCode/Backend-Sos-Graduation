using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate.Enums;
using Sos.Domain.UserAggregate.Errors;
using System.Text.RegularExpressions;

namespace Sos.Domain.UserAggregate.ValueObjects
{
    /// <summary>
    /// Represents the email value object.
    /// </summary>
    public sealed class Email : ValueObject
    {
        private static readonly Lazy<Regex> EmailFormatRegex =
            new Lazy<Regex>(() => new Regex(
                UserRequirementEnum.EmailRegexPattern,
                RegexOptions.Compiled | RegexOptions.IgnoreCase));

        /// <summary>
        /// Initializes a new instance of the <see cref="Email"/> class.
        /// </summary>
        /// <param name="value">The email value.</param>
        private Email(string value) => Value = value;

        /// <summary>
        /// Gets the email value.
        /// </summary>
        public string Value { get; }

        public static implicit operator string(Email email) => email.Value;

        /// <summary>
        /// Create a new <see cref="Email"/> instance based on the specified value.
        /// </summary>
        /// <param name="email">The email value.</param>
        /// <returns>The result of the email creattion process containing the email or an error.</returns>
        public static Result<Email> Create(string email) =>
            Result.Create(email, EmailError.NullOrEmpty)
                .Ensure(e => !string.IsNullOrWhiteSpace(e), EmailError.NullOrEmpty)
                .Ensure(e => e.Length <= UserRequirementEnum.MaxEmailLength, EmailError.LongerThanAllowed)
                .Ensure(e => EmailFormatRegex.Value.IsMatch(e), EmailError.InvalidFormat)
                .Map(e => new Email(e));

        // <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
