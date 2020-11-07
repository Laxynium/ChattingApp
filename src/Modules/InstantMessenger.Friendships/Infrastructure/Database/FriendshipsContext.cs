using InstantMessenger.Friendships.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstantMessenger.Friendships.Infrastructure.Database
{
    public class FriendshipsContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public FriendshipsContext(DbContextOptions<FriendshipsContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Friendships");

            BuildPerson(modelBuilder.Entity<Person>());

            BuildInvitation(modelBuilder.Entity<Invitation>());

            BuildFriendship(modelBuilder.Entity<Friendship>());
        }

        private static void BuildPerson(EntityTypeBuilder<Person> person)
        {
            person.HasKey(x => x.Id)
                .HasName("PK_Persons");
            person.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("PersonId");
            person.ToTable("Persons");
        }

        private static void BuildInvitation(EntityTypeBuilder<Invitation> invitation)
        {
            invitation.HasKey(x => x.Id)
                .HasName("PK_Invitations");
            invitation.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("InvitationId");
            invitation.Property(x => x.SenderId)
                .IsRequired();
            invitation.HasOne<Person>()
                .WithMany()
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired()
                .HasConstraintName("FK_Sender_Invitations_Persons");
            invitation.Property(x => x.ReceiverId)
                .IsRequired();
            invitation.HasOne<Person>()
                .WithMany()
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired()
                .HasConstraintName("FK_Receiver_Invitations_Persons");
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
                .HasColumnName("FriendshipId");
            friendship.Property(x => x.CreatedAt)
                .IsRequired();
            friendship.Property(x => x.FirstPerson)
                .IsRequired();
            friendship.HasOne<Person>()
                .WithMany()
                .HasForeignKey(x => x.FirstPerson)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired()
                .HasConstraintName("FK_FirstPerson_Friendships_Persons");
            friendship.Property(x => x.SecondPerson)
                .IsRequired();
            friendship.HasOne<Person>()
                .WithMany()
                .HasForeignKey(x => x.SecondPerson)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired()
                .HasConstraintName("FK_SecondPerson_Friendships_Persons");
        }
    }
}