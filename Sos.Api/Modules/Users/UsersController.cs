using MediatR;
using Microsoft.AspNetCore.Mvc;
using Sos.Api.Configurations;
using Sos.Application.Modules.Users.Commands.ChangePassword;
using Sos.Application.Modules.Users.Commands.UpdateLocation;
using Sos.Application.Modules.Users.Commands.UpdateUser;
using Sos.Application.Modules.Users.Queries.GetUserById;
using Sos.Application.Modules.Users.Queries.GetUsers;
using Sos.Contracts.Common;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.Core.Errors;

namespace Sos.Api.Modules.Users
{
    public sealed class UsersController(IMediator mediator) : ApiController(mediator)
    {
        // [Authorize(Roles = Roles.Admin)]
        [HttpGet(UsersRoute.GetUsers)]
        [ProducesResponseType(typeof(PaginateList<UserResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetUsers([FromQuery] PaginateRequest paginateRequest) =>
            await Maybe<GetUsersQuery>
                .From(new GetUsersQuery(paginateRequest.Page, paginateRequest.PageSize))
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpGet(UsersRoute.GetById)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid userId) =>
            await Maybe<GetUserByIdQuery>
                .From(new GetUserByIdQuery(userId))
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);

        [HttpPatch(UsersRoute.Update)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUser(Guid userId, UpdateUserRequest updateUserRequest) =>
            await Result.Create(updateUserRequest, GeneralError.UnProcessableRequest)
                .Map(request => new UpdateUserCommand(
                    userId,
                    request.FirstName,
                    request.LastName,
                    request.ContactPhone,
                    request.Avatar
                  ))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, BadRequest);

        [HttpPatch(UsersRoute.ChangePassword)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ChangePasssword(Guid userId, [FromBody] ChangePasswordRequest changePasswordRequest) =>
            await Result.Create(changePasswordRequest, GeneralError.UnProcessableRequest)
                .Map(request => new ChangePasswordCommand(
                    userId,
                    request.CurrentPassword,
                    request.Password,
                    request.ConfirmPassword
                ))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, BadRequest);

        [HttpPatch(UsersRoute.UpdateLocation)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateLocation(Guid userId, [FromBody] UpdateLocationRequest updateLocationRequest) =>
            await Result.Create(updateLocationRequest, GeneralError.UnProcessableRequest)
                .Map(request => new UpdateLocationCommand(
                    userId,
                    request.Longitude,
                    request.Latitude
                ))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, BadRequest);
    }
}
