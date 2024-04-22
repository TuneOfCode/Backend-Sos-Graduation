using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sos.Api.Configurations;
using Sos.Application.Modules.Friendships.Commands.AcceptFriendshipRequest;
using Sos.Application.Modules.Friendships.Commands.CancelFriendshipRequest;
using Sos.Application.Modules.Friendships.Commands.CreateFriendshipRequest;
using Sos.Application.Modules.Friendships.Commands.RejectFriendshipRequest;
using Sos.Application.Modules.Friendships.Commands.RemoveFriendship;
using Sos.Application.Modules.Friendships.Queries.GetFriendshipByUserId;
using Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestById;
using Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestByReceiver;
using Sos.Application.Modules.Friendships.Queries.GetFriendshipRequestBySender;
using Sos.Contracts.Common;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.Core.Errors;

namespace Sos.Api.Modules.Friendships
{
    public sealed class FriendshipsController(IMediator mediator) : ApiController(mediator)
    {
        [HttpGet(FriendshipsRoute.GetFriendshipRequestById)]
        [ProducesResponseType(typeof(FriendshipRequestResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendshipRequestById(Guid friendshipRequestId) =>
            await Maybe<GetFriendshipRequestByIdQuery>
                .From(new GetFriendshipRequestByIdQuery(friendshipRequestId))
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpGet(FriendshipsRoute.GetFriendshipRequestBySenderId)]
        [ProducesResponseType(typeof(ListFriendshipRequestBySenderResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendshipRequestBySenderId(
            Guid senderId) =>
            await Maybe<GetFriendshipRequestBySenderQuery>
                .From(new GetFriendshipRequestBySenderQuery(senderId))
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpGet(FriendshipsRoute.GetFriendshipRequestByReceiverId)]
        [ProducesResponseType(typeof(ListFriendshipRequestByReceiverResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendshipRequestByReceiverId(
            Guid receiverId) =>
            await Maybe<GetFriendshipRequestByReceiverQuery>
                .From(new GetFriendshipRequestByReceiverQuery(receiverId))
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpPost(FriendshipsRoute.CreateFriendshipRequest)]
        [ProducesResponseType(typeof(FriendshipRequestResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateFriendshipRequest(
            FriendshipRequestRequest friendshipRequestRequest) =>
            await Result.Create(friendshipRequestRequest, GeneralError.UnProcessableRequest)
                .Map(request => new CreateFriendshipRequestCommand(
                    request.SenderId,
                    request.ReceiverId
                 ))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, BadRequest);

        [HttpPatch(FriendshipsRoute.AcceptFriendshipRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptFriendshipRequest(
            Guid friendshipRequestId) =>
            await Result.Success(new AcceptFriendshipRequestCommand(friendshipRequestId))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, BadRequest);

        [HttpPatch(FriendshipsRoute.RejectFriendshipRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RejectFriendshipRequest(
            Guid friendshipRequestId) =>
            await Result.Success(new RejectFriendshipRequestCommand(friendshipRequestId))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, BadRequest);

        [HttpPatch(FriendshipsRoute.CancelFriendshipRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CancelFriendshipRequest(
            Guid friendshipRequestId) =>
            await Result.Success(new CancelFriendshipRequestCommand(friendshipRequestId))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, BadRequest);

        [HttpGet(FriendshipsRoute.GetFriendshipByUserId)]
        [ProducesResponseType(typeof(PaginateList<FriendshipResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFriendshipByUserId(
            Guid userId, [FromQuery] PaginateRequest paginateRequest) =>
            await Maybe<GetFriendshipByUserIdQuery>
                .From(new GetFriendshipByUserIdQuery(userId, paginateRequest.Page, paginateRequest.PageSize))
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpPatch(FriendshipsRoute.RemoveFriendshipByUserId)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RemoveFriendshipByUserId(
            Guid userId, Guid friendId) =>
            await Result.Success(new RemoveFriendshipCommand(userId, friendId))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, BadRequest);
    }
}
