using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Friendships.Errors;

namespace Sos.Application.Modules.Friendships.Commands.RemoveFriendship
{
    public sealed class RemoveFriendshipCommandValidator
        : AbstractValidator<RemoveFriendshipCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveFriendshipCommandValidator"/> class.
        /// </summary>
        public RemoveFriendshipCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty()
                .WithError(RemoveFriendshipError.UserIsRequired);

            RuleFor(x => x.FriendId)
                .NotEmpty()
                .WithError(RemoveFriendshipError.FriendIsRequired);
        }
    }
}
