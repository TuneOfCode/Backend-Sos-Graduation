using Sos.Domain.Core.Commons.Bases;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate.Enums;
using Sos.Domain.UserAggregate.Errors;

namespace Sos.Domain.UserAggregate.ValueObjects
{
    /// <summary>
    /// Represents the avatar value object.
    /// </summary>
    public sealed class Avatar : ValueObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Avatar"/> class.
        /// </summary>
        /// <param name="avatarUrl">The avatar url.</param>
        private Avatar(string avatarUrl)
        {
            AvatarUrl = avatarUrl;
        }

        /// <summary>
        /// Gets the avatar url.
        /// </summary>
        public string AvatarUrl { get; }

        public static implicit operator string(Avatar avatar) => avatar.AvatarUrl;

        /// <summary>
        /// Create a new <see cref="Avatar"/> instance based on the specified value.
        /// </summary>
        /// <param name="avatarUrl">The avatar url.</param>
        /// <returns>The result of the avatar creattion process containing the avatar or an error.</returns>
        public static Result<Avatar> Create(string avatarUrl, long avatarSize)
            => Result.Create(avatarUrl, AvatarError.NotAllowedExtension)
                    .Ensure(a => UserRequirementEnum.AllowedExtensions.Contains(Path.GetExtension(a)), AvatarError.NotAllowedExtension)
                    .Ensure(a => avatarSize <= UserRequirementEnum.MaxSizeFile, AvatarError.BiggerThanMaxSize)
                    .Map(a => new Avatar(a));

        // <inheritdoc />
        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return AvatarUrl;
        }
    }
}
