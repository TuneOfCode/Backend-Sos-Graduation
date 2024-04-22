using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Friendships.Errors;

namespace Sos.Application.Modules.Friendships.Commands.CancelFriendshipRequest
{
    public sealed class CancelFriendshipRequestCommandValidator : AbstractValidator<CancelFriendshipRequestCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelFriendshipRequestCommandValidator"/> class.
        /// </summary>
        public CancelFriendshipRequestCommandValidator()
        {
            RuleFor(x => x.FriendshipRequestId)
                .NotEmpty()
                .WithError(CancelFriendshipRequestError.FriendshipRequestIsRequired);
        }
    }
}
