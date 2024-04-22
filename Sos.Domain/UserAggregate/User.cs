using Sos.Domain.Core.Abstractions;
using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate.Enums;
using Sos.Domain.UserAggregate.Errors;
using Sos.Domain.UserAggregate.Events;
using Sos.Domain.UserAggregate.Services;
using Sos.Domain.UserAggregate.ValueObjects;

namespace Sos.Domain.UserAggregate
{
    /// <summary>
    /// Represents the user entity.
    /// </summary>
    public class User : AggregateRoot, IAuditableEntity, ISoftDeletableEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <remarks>
        /// Required by EF Core.
        /// </remarks>
        private User()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="firstName">The user first name value.</param>
        /// <param name="lastName">The user last name value.</param>
        /// <param name="email">The user email instance.</param>
        /// <param name="contactPhone">The user contact phone instance.</param>
        /// <param name="passwordHash">The user password hash.</param>
        private User(string firstName, string lastName, Email email, Phone contactPhone, string passwordHash)
            : base(Guid.NewGuid())
        {
            Ensure.NotEmpty(firstName, "The first name is required.", nameof(firstName));
            Ensure.NotEmpty(lastName, "The last name is required.", nameof(lastName));
            Ensure.NotEmpty(email, "The email is required.", nameof(email));
            Ensure.NotEmpty(contactPhone, "The contact phone is required.", nameof(contactPhone));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            ContactPhone = contactPhone;
            _passwordHash = passwordHash;
        }

        /// <summary>
        /// Gets the user first name.
        /// </summary>
        public string? FirstName { get; private set; }

        /// <summary>
        /// Gets the user last name.
        /// </summary>
        public string? LastName { get; private set; }

        /// <summary>
        /// Gets the user full name.
        /// </summary>
        public string? FullName => $"{LastName} {FirstName}";

        /// <summary>
        /// Gets the user email.
        /// </summary>
        public Email? Email { get; private set; }

        /// <summary>
        /// Gets the user contact phone.
        /// </summary>
        public Phone? ContactPhone { get; private set; }

        public Avatar? Avatar { get; set; } = null!;

        /// <summary>
        /// Gets the user verified on UTC.
        /// </summary>
        public DateTime? VerifiedOnUtc { get; set; } = null!;

        public string? _passwordHash;

        public string Role { get; set; } = Roles.User;

        public string? RefreshToken { get; set; } = string.Empty;

        public DateTime CreatedOnUtc { get; }

        public DateTime? ModifiedOnUtc { get; }

        public DateTime? DeletedOnUtc { get; }

        public bool Deleted { get; }

        /// <summary>
        /// Creates a new user with the specified first name, last name, email, contact phone and password hash.
        /// </summary>
        /// <param name="firstName">The user first name.</param>
        /// <param name="lastName">The user last name.</param>
        /// <param name="email">The user email.</param>
        /// <param name="contactPhone">The user contact phone.</param>
        /// <param name="passwordHash">The user password hash.</param>
        /// <returns>The new user created by user instance.</returns>
        public static User Create(string firstName, string lastName, Email email, Phone contactPhone,
            string passwordHash)
        {
            var newUser = new User(firstName, lastName, email, contactPhone, passwordHash);

            newUser.AddDomainEvent(new UserCreatedDomainEvent(newUser));

            return newUser;
        }

        /// <summary>
        /// Updates the user with the specified first name, last name, email, contact phone and avatar.
        /// </summary>
        /// <param name="firstName">The user first name.</param>
        /// <param name="lastName">The user last name.</param>
        /// <param name="contactPhone">The user contact phone.</param>
        /// <param name="avatar">The user avatar.</param>
        /// <returns>The user updated by user instance. </returns>
        public void Update(string firstName, string lastName, Phone contactPhone, Avatar avatar)
        {
            FirstName = firstName;
            LastName = lastName;
            ContactPhone = contactPhone;
            Avatar = avatar;

            AddDomainEvent(new UserUpdatedDomainEvent(this));
        }

        /// <summary>
        /// Verifies the password hash.
        /// </summary>
        /// <param name="password">The user password provided.</param>
        /// <param name="passwordHashChecker">The password hash checker.</param>
        /// <returns>True if password provided is valid and matches the password hash, otherwise false.</returns>
        public bool VerifyPasswordHash(string password, IPasswordHashCheckerService passwordHashChecker)
            => !string.IsNullOrWhiteSpace(password)
                && passwordHashChecker.HashesMatch(_passwordHash!, password);

        /// <summary>
        /// Changes the user password.
        /// </summary>
        /// <param name="newPasswordHash">The new password hash.</param>
        /// <returns>The success result or an error.</returns>
        public Result ChangePassword(string newPasswordHash)
        {
            if (string.IsNullOrWhiteSpace(newPasswordHash) || newPasswordHash == _passwordHash)
            {
                return Result.Failure(UserDomainError.CannotChangePassword);
            }

            _passwordHash = newPasswordHash;

            AddDomainEvent(new UserPasswordChangedDomainEvent(this));

            return Result.Success();
        }
    }
}
