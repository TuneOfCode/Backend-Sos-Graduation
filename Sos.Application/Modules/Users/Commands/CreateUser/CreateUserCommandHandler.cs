using Sos.Application.Core.Abstractions.Cryptography;
using Sos.Application.Core.Abstractions.Data;
using Sos.Application.Core.Abstractions.Email;
using Sos.Application.Core.Abstractions.Messaging;
using Sos.Contracts.Email;
using Sos.Contracts.Users;
using Sos.Domain.Core.Commons.Result;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Errors;
using Sos.Domain.UserAggregate.Repositories;
using Sos.Domain.UserAggregate.ValueObjects;

namespace Sos.Application.Modules.Users.Commands.CreateUser
{
    /// <summary>
    /// Represents the <see cref="CreateUserCommand"/> handler.
    /// </summary>
    public sealed class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Result<UserResponse>>
    {
        private const string _defaultAvatarUrl = "/images/defaultAvatar.png";
        private const long _defaultAvatarSize = 0;
        private const string Subject = "Xác minh tài khoản trong ứng dụng SOS";
        private const int ExprireMinutes = 3;

        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IVerifyCodeGenerator _verifyCodeGenerator;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateUserCommandHandler"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="passwordHasher">The password hasher.</param>
        /// <param name="verifyCodeGenerator">The verify code generator.</param>
        /// <param name="emailService">The email service.</param>
        public CreateUserCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            IVerifyCodeGenerator verifyCodeGenerator,
            IEmailService emailService
        )
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _verifyCodeGenerator = verifyCodeGenerator;
            _emailService = emailService;
        }

        // <inheritdoc />
        public async Task<Result<UserResponse>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            Result<Email> emailResult = Email.Create(request.Email);
            Result<Phone> phoneResult = Phone.Create(request.ContactPhone);
            Result<Password> passwordResult = Password.Create(request.Password);

            Result firstFailureOrSuccess = Result.FirstFailureOrSuccess(emailResult, phoneResult, passwordResult);

            if (firstFailureOrSuccess.IsFailure)
            {
                return Result.Failure<UserResponse>(firstFailureOrSuccess.Error);
            }

            bool isEmailUnique = await _userRepository.IsEmailUniqueAsync(emailResult.Value);
            if (!isEmailUnique)
            {
                return Result.Failure<UserResponse>(UserDomainError.EmailAlreadyExists);
            }

            string passwordHash = _passwordHasher.HashPassword(passwordResult.Value);

            var newUser = User.Create(
                request.FirstName,
                request.LastName,
                emailResult.Value,
                phoneResult.Value,
                passwordHash
            );

            Result<Avatar> avatarResult = Avatar.Create(_defaultAvatarUrl, _defaultAvatarSize);

            newUser.Avatar = avatarResult.Value;

            newUser.VerifyCode = _verifyCodeGenerator.Generate();

            newUser.VerifyCodeExpired = DateTime.Now.AddMinutes(ExprireMinutes);

            _userRepository.Insert(newUser);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await _emailService.SendEmailAsync(new MailRequest(
                    newUser.Email!.Value,
                    Subject,
                    newUser.VerifyCode
                ));

            return Result.Success(new UserResponse(
                newUser.Id,
                newUser.FullName!,
                newUser.FirstName!,
                newUser.LastName!,
                newUser.Email!.Value,
                newUser.ContactPhone!.Value,
                newUser.Avatar!.AvatarUrl,
                newUser.Location!.Longitude,
                newUser.Location!.Latitude,
                newUser.VerifiedAt,
                newUser.CreatedAt
            ));
        }
    }
}
