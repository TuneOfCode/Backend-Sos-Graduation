using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.UserAggregate;

namespace Sos.Persistence.Modules.Friendships
{
    /// <summary>
    /// Represents the configuration for the <see cref="Friendship"/> entity.
    /// </summary>
    internal class FriendshipConfiguration : IEntityTypeConfiguration<Friendship>
    {
        // <inheritdoc />
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.HasKey(friendship => new
            {
                friendship.UserId,
                friendship.FriendId,
            });

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(friendship => friendship.UserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(friendship => friendship.FriendId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(friendship => friendship.CreatedAt)
                .IsRequired();

            builder.Property(friendship => friendship.ModifiedAt);

            builder.Ignore(friendship => friendship.Id);
        }
    }
}
