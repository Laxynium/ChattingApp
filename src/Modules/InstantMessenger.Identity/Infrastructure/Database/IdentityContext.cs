using InstantMessenger.Identity.Domain.Entities;
using InstantMessenger.Shared.BuildingBlocks;
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
            builder.HasKey(x => x.Id)
                .HasName("PK_ActivationLinks");
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasConversion(x=>x.Value, x=>new ActivationLinkId(x))
                .HasColumnName("ActivationLinkId");
            
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
            builder
                .HasKey(x => x.Id)
                .HasName("PK_Users");
            builder.Property(x=>x.Id)
                .ValueGeneratedNever()
                .HasConversion(x=>x.Value,x=>new UserId(x))
                .HasColumnName("UserId");

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
            builder.OwnsOne(
                u => u.Avatar,
                b =>
                {
                    b.Property(x => x.Value).IsRequired().HasColumnName("Avatar");
                }
            );
        }
    }
}