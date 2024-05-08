using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sos.Domain.FriendshipAggregate;
using Sos.Domain.UserAggregate;

namespace Sos.Persistence.Modules.Friendships
{
    /// <summary>
    /// Represents the configuration for the <see cref="FriendshipRequest"/> entity.
    /// </summary>
    internal class FriendshipRequestConfiguration : IEntityTypeConfiguration<FriendshipRequest>
    {
        // < inheritdoc />
        public void Configure(EntityTypeBuilder<FriendshipRequest> builder)
        {
            builder.HasKey(friendRequest => friendRequest.Id);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(friendshipRequest => friendshipRequest.SenderId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(friendshipRequest => friendshipRequest.ReceiverId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder.OwnsOne(friendship => friendship.StatusRequest, statusBuilder =>
            {
                statusBuilder.WithOwner();

                statusBuilder.Property(status => status.Value)
                    .HasColumnName("Status")
                    .IsRequired();
            });

            builder.Property(friendship => friendship.CreatedAt).IsRequired();

            builder.Property(friendship => friendship.ModifiedAt);

            builder.Property(friendship => friendship.DeletedAt);

            builder.Property(friendship => friendship.Deleted).HasDefaultValue(false);

            builder.HasQueryFilter(friendship => !friendship.Deleted);
        }
    }
}
