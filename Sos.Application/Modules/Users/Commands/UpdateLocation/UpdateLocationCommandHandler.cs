using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Errors;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Domain.UserAggregate.ValueObjects;

namespace Sos.Application.Modules.Users.Commands.UpdateLocation
{
    /// <summary>
    /// Represents the handler for the command to update user location.
    /// </summary>
    public sealed class UpdateLocationCommandHandler : ICommandHandler<UpdateLocationCommand, Result>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateLocationCommandHandler(
            IUserIdentifierProvider userIdentifierProvider,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        // < inheritdoc />
        public async Task<Result> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
        {
            Maybe<User> maybeUser = await _userRepository.GetByIdAsync(request.UserId);

            if (maybeUser.HasNoValue)
            {
                return Result.Failure(UserDomainError.NotFound);
            }

            User user = maybeUser.Value;

            if (user.Id != _userIdentifierProvider.UserId)
            {
                return Result.Failure(UserDomainError.InvalidPermissions);
            }

            Result<Location> locationResult = Location.Create(request.Longitude, request.Latitude);

            Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(locationResult);

            if (firstFailureOrSuccess.IsFailure)
            {
                return Result.Failure(firstFailureOrSuccess.Error);
            }

            user.UpdateLocation(locationResult.Value.Longitude, locationResult.Value.Latitude);

            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
