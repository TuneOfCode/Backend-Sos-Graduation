﻿using Microsoft.EntityFrameworkCore;
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

            builder.Property(user => user.Role)
                .HasConversion<string>()
                .IsRequired();

            builder.Property(user => user.RefreshToken);

            builder.Property(user => user.CreatedOnUtc).IsRequired();

            builder.Property(user => user.ModifiedOnUtc);

            builder.Property(user => user.DeletedOnUtc);

            builder.Property(user => user.Deleted).HasDefaultValue(false);

            builder.HasQueryFilter(user => !user.Deleted);

            builder.Ignore(user => user.FullName);
        }
    }
}
