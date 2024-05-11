using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sos.Domain.UserAggregate;
using Sos.Domain.UserAggregate.Enums;

namespace Sos.Persistence.Modules.Users
{
    /// <summary>
    /// Respresents the configuration for the <see cref="User"/> entity.
    /// </summary>
    internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
    {
        // <inheritdoc/>
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(user => user.Id);

            builder.Property(user => user.FirstName)
                .HasMaxLength(UserRequirementEnum.MaxFirstNameLength)
                .IsRequired();

            builder.Property(user => user.LastName)
                .HasMaxLength(UserRequirementEnum.MaxLastNameLength)
                .IsRequired();

            builder.OwnsOne(user => user.Email, emailBuilder =>
            {
                emailBuilder.WithOwner();

                emailBuilder.Property(email => email.Value)
                    .HasColumnName(nameof(User.Email))
                    .HasMaxLength(UserRequirementEnum.MaxEmailLength)
                    .IsRequired();

            });

            builder.OwnsOne(user => user.ContactPhone, phoneBuilder =>
            {
                phoneBuilder.WithOwner();

                phoneBuilder.Property(phone => phone.Value)
                    .HasColumnName(nameof(User.ContactPhone))
                    .HasMaxLength(UserRequirementEnum.MaxPhoneLength)
                    .IsRequired();

            });

            builder.Property<string>("_passwordHash")
                .HasField("_passwordHash")
                .HasColumnName("PasswordHash")
                .IsRequired();

            builder.OwnsOne(user => user.Avatar, avatarBuilder =>
            {
                avatarBuilder.WithOwner();

                avatarBuilder.Property(avatar => avatar.AvatarUrl)
                    .HasColumnName(nameof(User.Avatar))
                    .IsRequired();
            });

            builder.OwnsOne(user => user.Location, locationBuilder =>
            {
                locationBuilder.WithOwner();

                locationBuilder.Property(location => location.Longitude)
                    .HasColumnName(nameof(User.Location.Longitude))
                    .HasDefaultValue(107.585217); // Hue, Vietnam

                locationBuilder.Property(location => location.Latitude)
                    .HasColumnName(nameof(User.Location.Latitude))
                    .HasDefaultValue(16.462622); // Hue, Vietnam
            });

            builder.Property(user => user.VerifyCode);

            builder.Property(user => user.VerifyCodeExpired);

            builder.Property(user => user.Role)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(user => user.RefreshToken);

            builder.Property(user => user.CreatedAt).IsRequired();

            builder.Property(user => user.ModifiedAt);

            builder.Property(user => user.DeletedAt);

            builder.Property(user => user.Deleted).HasDefaultValue(false);

            builder.HasQueryFilter(user => !user.Deleted);

            builder.Ignore(user => user.FullName);
        }
    }
}
