using InstantMessenger.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstantMessenger.Identity.Infrastructure.Database
{
    public class IdentityContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<ActivationLink> VerificationLinks { get; set; }

        public IdentityContext(DbContextOptions<IdentityContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.HasDefaultSchema("Identity");
            Build(modelBuilder.Entity<User>());
            Build(modelBuilder.Entity<ActivationLink>());
        }

        private static void Build(EntityTypeBuilder<ActivationLink> builder)
        {
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("ActivationLinkId");
            builder.HasKey(x => x.Id)
                .HasName("PK_ActivationLinks");
            builder.Property(x => x.Token).IsRequired();
            builder.Property(x => x.CreatedAt).IsRequired();
            builder.Property(x => x.UserId).IsRequired();
            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .IsRequired()
                .HasConstraintName("FK_ActivationLinks_Users");
            builder.ToTable("ActivationLinks");
        }

        private static void Build(EntityTypeBuilder<User> builder)
        {
            builder.Property(x=>x.Id)
                .ValueGeneratedNever()
                .HasColumnName("UserId");
            builder
                .HasKey(x=>x.Id)
                .HasName("PK_Users");
            builder.OwnsOne(x => x.Email, e =>
            {
                e.Property(x => x.Value).HasColumnName("Email").IsRequired();
            });
            builder
                .Property(x => x.PasswordHash).IsRequired();
            builder.Property(x => x.IsVerified).IsRequired();
            builder.Property(x => x.Nickname)
                .HasConversion(x => x.Value, x => Nickname.Create(x))
                .IsRequired(false);
            builder.ToTable("Users");
        }
    }
}