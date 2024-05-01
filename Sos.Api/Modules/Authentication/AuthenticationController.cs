using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Sos.Api.Configurations;
using Sos.Application.Core.Abstractions.Socket;
using Sos.Application.Modules.Authentication.Commands.Login;
using Sos.Application.Modules.Authentication.Commands.RefreshToken;
using Sos.Application.Modules.Authentication.Commands.ReSendVerifyCode;
using Sos.Application.Modules.Authentication.Commands.Verify;
using Sos.Application.Modules.Authentication.Queries;
using Sos.Application.Modules.Users.Commands.CreateUser;
using Sos.Contracts.Authentication;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.Core.Errors;
using Sos.Infrastructure.Socket;

namespace Sos.Api.Modules.Authentication
{
    [Authorize]
    public sealed class AuthenticationController : ApiController
    {
        private readonly IMediator _mediator;
        private readonly IHubContext<NotificationsHub, INotificationsClient> _notificationsHubContext;

        public AuthenticationController(
            IMediator mediator,
            IHubContext<NotificationsHub, INotificationsClient> notificationsHubContext

        ) : base(mediator)
        {
            _mediator = mediator;
            _notificationsHubContext = notificationsHubContext;
        }

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
                .Bind(command => _mediator.Send(command))
                .Match(Ok, BadRequest);

        [AllowAnonymous]
        [HttpPost(AuthenticationRoute.Login)]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest) =>
            await Result.Create(loginRequest, GeneralError.UnProcessableRequest)
                .Map(request => new LoginCommand(request.Email, request.Password))
                .Bind(command => _mediator.Send(command))
                .Match(Ok, BadRequest);

        [AllowAnonymous]
        [HttpPatch(AuthenticationRoute.Verify)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Verify([FromBody] VerifyRequest verifyRequest) =>
            await Result.Create(verifyRequest, GeneralError.UnProcessableRequest)
                .Map(request => new VerifyCommand(request.Email, request.Code))
                .Bind(command => _mediator.Send(command))
                .Match(Ok, BadRequest);

        [AllowAnonymous]
        [HttpPatch(AuthenticationRoute.ReSendVerifyCode)]
        [ProducesResponseType(typeof(ReSendVerifyCodeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReSendVerifyCode([FromBody] ReSendVerifyCodeRequest reSendVerifyCodeRequest) =>
            await Result.Create(reSendVerifyCodeRequest, GeneralError.UnProcessableRequest)
                .Map(request => new ReSendVerifyCodeCommand(request.Email))
                .Bind(command => _mediator.Send(command))
                .Match(Ok, BadRequest);

        [AllowAnonymous]
        [HttpPatch(AuthenticationRoute.RefreshToken)]
        [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest) =>
            await Result.Create(refreshTokenRequest, GeneralError.UnProcessableRequest)
                .Map(request => new RefreshTokenCommand(request.UserId, request.RefreshToken))
                .Bind(command => _mediator.Send(command))
                .Match(Ok, BadRequest);

        [HttpGet(AuthenticationRoute.GetMe)]
        [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetMe()
        {
            // await _notificationsHubContext.Clients.User("ce9dde53-6e85-4e0e-a1ce-56db0e6bc8cb").ReceiveNotification("\"Get me successfully !!!.\"");
            // await _notificationsHubContext.Clients.All.ReceiveNotification("\"Get me successfully !!!.\"");
            return await Maybe<GetMeQuery>
                .From(new GetMeQuery())
                .Bind(query => _mediator.Send(query))
                .Match(Ok, NotFound);
        }
    }
}
