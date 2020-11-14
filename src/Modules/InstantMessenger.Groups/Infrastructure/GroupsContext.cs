using InstantMessenger.Groups.Domain;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Infrastructure
{
    public class GroupsContext : DbContext
    {
        public DbSet<Group> Groups { get; set; }
        public GroupsContext(DbContextOptions<GroupsContext>options):base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var group = modelBuilder.Entity<Group>();
            modelBuilder.HasDefaultSchema("Groups");
            group.ToTable("Groups");
            group.HasKey(x => x.Id)
                .HasName("PK_Groups");
            group.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("GroupId");
        }
    }
}