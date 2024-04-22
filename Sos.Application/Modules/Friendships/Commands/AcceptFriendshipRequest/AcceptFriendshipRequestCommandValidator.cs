using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Friendships.Errors;

namespace Sos.Application.Modules.Friendships.Commands.AcceptFriendshipRequest
{
    public sealed class AcceptFriendshipRequestCommandValidator : AbstractValidator<AcceptFriendshipRequestCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AcceptFriendshipRequestCommandValidator"/> class.
        /// </summary>
        public AcceptFriendshipRequestCommandValidator()
        {
            RuleFor(x => x.FriendshipRequestId)
                .NotEmpty()
                .WithError(AcceptFriendshipRequestError.FriendshipRequestIsRequired);
        }
    }
}
