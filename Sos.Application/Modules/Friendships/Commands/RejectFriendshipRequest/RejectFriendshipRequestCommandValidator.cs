using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Friendships.Errors;

namespace Sos.Application.Modules.Friendships.Commands.RejectFriendshipRequest
{
    public sealed class RejectFriendshipRequestCommandValidator : AbstractValidator<RejectFriendshipRequestCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RejectFriendshipRequestCommandValidator"/> class.
        /// </summary>
        public RejectFriendshipRequestCommandValidator()
        {
            RuleFor(x => x.FriendshipRequestId)
                .NotEmpty()
                .WithError(AcceptFriendshipRequestError.FriendshipRequestIsRequired);
        }
    }
}
