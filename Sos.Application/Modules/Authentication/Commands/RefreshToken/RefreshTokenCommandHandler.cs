using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Authentication;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Errors;
using Sos.Domain.UserAggregate.Repositories;

namespace Sos.Application.Modules.Authentication.Commands.RefreshToken
{
    /// <summary>
    /// Represents the refresh token command handler.
    /// </summary>
    public sealed class RefreshTokenCommandHandler : ICommandHandler<RefreshTokenCommand, Result<TokenResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJwtProvider _jwtProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="RefreshTokenCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="jwtProvider">The JWT provider.</param>
        public RefreshTokenCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IJwtProvider jwtProvider)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _jwtProvider = jwtProvider;
        }

        // <inheritdoc />
        public async Task<Result<TokenResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure<TokenResponse>(UserDomainError.NotFound);
            }

            User user = maybeUser.Value;

            if (user.RefreshToken != request.RefreshToken)
            {
                return Result.Failure<TokenResponse>(UserDomainError.InvalidRefreshToken);
            }

            string accessToken = _jwtProvider.GenerateAccessToken(user);

            string refreshToken = _jwtProvider.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new TokenResponse(accessToken, refreshToken));
        }
    }
}
