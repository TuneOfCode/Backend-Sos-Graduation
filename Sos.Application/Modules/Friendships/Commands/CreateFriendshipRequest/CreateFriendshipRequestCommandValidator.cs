using FluentValidation;
using Sos.Application.Core.Extensions;
using Sos.Application.Modules.Friendships.Errors;

namespace Sos.Application.Modules.Friendships.Commands.CreateFriendshipRequest
{
    public sealed class CreateFriendshipRequestCommandValidator : AbstractValidator<CreateFriendshipRequestCommand>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFriendshipRequestCommandValidator"/> class.
        /// </summary>
        public CreateFriendshipRequestCommandValidator()
        {
            RuleFor(x => x.SenderId)
                .NotEmpty()
                .WithError(CreateFriendshipRequestError.SenderIdIsRequired);

            RuleFor(x => x.ReceiverId)
                .NotEmpty()
                .WithError(CreateFriendshipRequestError.ReceiverIdIsRequired);
        }
    }
}
