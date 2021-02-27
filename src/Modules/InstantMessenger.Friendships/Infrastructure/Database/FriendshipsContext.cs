using InstantMessenger.Friendships.Domain.Entities;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Shared.Outbox;
using InstantMessenger.Shared.Outbox.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstantMessenger.Friendships.Infrastructure.Database
{
    public class FriendshipsContext : DbContext
    {
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public FriendshipsContext(DbContextOptions<FriendshipsContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Friendships");

            BuildInvitation(modelBuilder.Entity<Invitation>());

            BuildFriendship(modelBuilder.Entity<Friendship>());

            modelBuilder.ApplyConfiguration(new OutboxEntityConfiguration());
        }

        private static void BuildInvitation(EntityTypeBuilder<Invitation> invitation)
        {
            invitation.HasKey(x => x.Id)
                .HasName("PK_Invitations");
            invitation.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasConversion(x=>x.Value,x=>new InvitationId(x))
                .HasColumnName("InvitationId");

            invitation.Property(x => x.SenderId)
                .HasConversion(x => x.Value, x => new PersonId(x))
                .IsRequired();

            invitation.Property(x => x.ReceiverId)
                .HasConversion(x => x.Value, x => new PersonId(x))
                .IsRequired();

            invitation.Property(x => x.CreatedAt).IsRequired();

            invitation.Property(x => x.Status).IsRequired()
                .HasConversion<string>();
        }

        private static void BuildFriendship(EntityTypeBuilder<Friendship> friendship)
        {
            friendship.HasKey(x => x.Id)
                .HasName("PK_Friendships");
            friendship.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasConversion(x=>x.Value,x=>new FriendshipId(x))
                .HasColumnName("FriendshipId");

            friendship.Property(x => x.CreatedAt)
                .IsRequired();

            friendship.Property(x => x.FirstPersonId)
                .HasConversion(x=>x.Value,x=>new PersonId(x))
                .IsRequired();

            friendship.Property(x => x.SecondPersonId)
                .HasConversion(x => x.Value, x => new PersonId(x))
                .IsRequired();
        }
    }
}