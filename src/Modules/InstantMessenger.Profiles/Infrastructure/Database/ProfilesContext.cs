using InstantMessenger.Profiles.Domain;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Profiles.Infrastructure.Database
{
    public class ProfilesContext : DbContext
    {
        public DbSet<Profile> Profiles { get; set; }
        public ProfilesContext(DbContextOptions<ProfilesContext> options):base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.HasDefaultSchema("Profiles");

            var profile = builder.Entity<Profile>();

            profile.HasKey(x => x.Id)
                .HasName("PK_ProfileId");

            profile.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("ProfileId");

            profile.OwnsOne(
                p => p.Avatar,
                b =>
                {
                    b.Property(x => x.Value).IsRequired().HasColumnName("Avatar");
                }
            );
        }
    }
}