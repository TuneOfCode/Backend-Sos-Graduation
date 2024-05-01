using Sos.Application.Core.Abstractions.Authentication;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Friendships;
using Sos.Domain.Core.Commons.Maybe;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.FriendshipAggregate.Enums;
using Sos.Domain.FriendshipAggregate.Errors;
using Sos.Domain.FriendshipAggregate.Repositories;
using Sos.Domain.FriendshipAggregate.ValueObjects;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Repositories;

namespace Sos.Application.Modules.Friendships.Commands.CreateFriendshipRequest
{
    public sealed class CreateFriendshipRequestCommandHandler
        : ICommandHandler<CreateFriendshipRequestCommand, Result<FriendshipRequestResponse>>
    {
        private readonly IUserIdentifierProvider _userIdentifierProvider;
        private readonly IUserRepository _userRepository;
        private readonly IFriendshipRequestRepository _friendshipRequestRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFriendshipRequestCommandHandler"/> class.
        /// </summary>
        /// <param name="userIdentifierProvider">The user identifier provider.</param>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="friendshipRequestRepository">The friendship request repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        public CreateFriendshipRequestCommandHandler(
            IUserIdentifierProvider userIdentifierProvider,
            IUserRepository userRepository,
            IFriendshipRequestRepository friendshipRequestRepository,
            IUnitOfWork unitOfWork
        )
        {
            _userIdentifierProvider = userIdentifierProvider;
            _userRepository = userRepository;
            _friendshipRequestRepository = friendshipRequestRepository;
            _unitOfWork = unitOfWork;
        }

        // < inheritdoc />
        public async Task<Result<FriendshipRequestResponse>> Handle(CreateFriendshipRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.SenderId == request.ReceiverId)
            {
                return Result.Failure<FriendshipRequestResponse>(FriendshipDomainError.SenderIsReceiver);
            }

            if (request.SenderId != _userIdentifierProvider.UserId)
            {
                return Result.Failure<FriendshipRequestResponse>(FriendshipDomainError.SenderIsNotCurrentUser);
            }

            if (request.ReceiverId == _userIdentifierProvider.UserId)
            {
                return Result.Failure<FriendshipRequestResponse>(FriendshipDomainError.ReceiverIsCurrentUser);
            }

            Maybe<User> maybeSender = await _userRepository.GetByIdAsync(request.SenderId);

            if (maybeSender.HasNoValue)
            {
                return Result.Failure<FriendshipRequestResponse>(FriendshipDomainError.SenderNotFound);
            }

            Maybe<User> maybeReceiver = await _userRepository.GetByIdAsync(request.ReceiverId);

            if (maybeReceiver.HasNoValue)
            {
                return Result.Failure<FriendshipRequestResponse>(FriendshipDomainError.ReceiverNotFound);
            }

            User sender = maybeSender.Value;

            User receiver = maybeReceiver.Value;

            bool isFriend = await _friendshipRequestRepository.CheckFriendshipRequestIsPendingAsync(sender, receiver);

            if (isFriend)
            {
                return Result.Failure<FriendshipRequestResponse>(FriendshipDomainError.FriendshipRequestIsExisted);
            }

            Status status = Status.Create(StatusEnum.Pending).Value;

            FriendshipRequest friendshipRequest = new(sender, receiver, status);

            //_friendshipRequestRepository.Insert(friendshipRequest);

            //await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(new FriendshipRequestResponse(
                friendshipRequest.Id,
                sender.Id,
                sender.FullName!,
                sender.Avatar!.AvatarUrl,
                receiver.Id,
                receiver.FullName!,
                receiver.Avatar!.AvatarUrl,
                status.Value,
                friendshipRequest.CreatedOnUtc,
                friendshipRequest.ModifiedOnUtc
            ));
        }
    }
}
