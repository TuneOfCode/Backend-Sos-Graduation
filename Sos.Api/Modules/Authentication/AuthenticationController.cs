using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sos.Api.Configurations;
using Sos.Application.Modules.Authentication.Commands.Login;
using Sos.Application.Modules.Authentication.Queries;
using Sos.Application.Modules.Users.Commands.CreateUser;
using Sos.Contracts.Authentication;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.Core.Errors;

namespace Sos.Api.Modules.Authentication
{
    [Authorize]
    public sealed class AuthenticationController(IMediator mediator) : ApiController(mediator)
    {
        [AllowAnonymous]
        [HttpPost(AuthenticationRoute.Register)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] RegisterRequest registerRequest) =>
            await Result.Create(registerRequest, GeneralError.UnProcessableRequest)
                .Map(request => new CreateUserCommand(
                    request.FirstName,
                    request.LastName,
                    request.Email,
                    request.ContactPhone,
                    request.Password,
                    request.ConfirmPassword
                 ))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, BadRequest);

        [AllowAnonymous]
        [HttpPost(AuthenticationRoute.Login)]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest) =>
            await Result.Create(loginRequest, GeneralError.UnProcessableRequest)
                .Map(request => new LoginCommand(request.Email, request.Password))
                .Bind(command => Mediator.Send(command))
                .Match(Ok, BadRequest);

        [HttpGet(AuthenticationRoute.GetMe)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMe() =>
            await Maybe<GetMeQuery>
                .From(new GetMeQuery())
                .Bind(query => Mediator.Send(query))
                .Match(Ok, NotFound);
    }
}
