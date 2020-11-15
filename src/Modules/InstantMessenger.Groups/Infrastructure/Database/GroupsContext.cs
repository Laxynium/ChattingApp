using InstantMessenger.Groups.Domain;
using Microsoft.EntityFrameworkCore;
namespace InstantMessenger.Groups.Infrastructure.Database
{
    public class GroupsContext : DbContext
    {
        public virtual DbSet<Group> Groups { get; set; }
        public GroupsContext(DbContextOptions<GroupsContext>options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Groups");

            var group = modelBuilder.Entity<Group>();
            group.ToTable("Groups");
            group.HasKey(x => x.Id)
                .HasName("PK_Groups");
            group.Property(x => x.Id)
                .HasConversion(x=>x.Value,x=>GroupId.From(x))
                .ValueGeneratedNever()
                .HasColumnName("GroupId");
            group.OwnsOne(
                x => x.Name,
                n =>
                {
                    n.Property(x => x.Value).HasColumnName(nameof(Group.Name))
                        .IsRequired();
                }
            );
            group.Property(x => x.CreatedAt).IsRequired();

            @group.Metadata.FindNavigation(nameof(Group.Members)).SetPropertyAccessMode(PropertyAccessMode.Field);
            group.OwnsMany(x => x.Members,
                m =>
                {
                    m.ToTable("Members");
                    m.HasKey(x => x.Id)
                        .HasName("FK_Members");
                    m.Property(x => x.Id)
                        .HasConversion(x=>x.Value,x=>MemberId.From(x))
                        .ValueGeneratedNever()
                        .IsRequired();
                    m.Property(x => x.UserId)
                        .HasConversion(x=>x.Value, x=>UserId.From(x))
                        .ValueGeneratedNever()
                        .IsRequired();
                    m.Property(x => x.IsOwner).IsRequired();
                    m.Property(x => x.Name)
                        .HasConversion(x => x.Value, x => MemberName.Create(x))
                        .IsRequired();
                    m.Property(x => x.CreatedAt).IsRequired();
                });
        }
    }
}