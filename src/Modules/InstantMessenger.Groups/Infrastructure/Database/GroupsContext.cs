using System;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstantMessenger.Groups.Infrastructure.Database
{
    public class GroupsContext : DbContext
    {
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Invitation> Invitations { get; set; }
        public GroupsContext(DbContextOptions<GroupsContext>options):base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Groups");

            BuildGroup(modelBuilder.Entity<Group>());
            BuildInvitation(modelBuilder.Entity<Invitation>());
        }

        private static void BuildInvitation(EntityTypeBuilder<Invitation> invitation)
        {
            invitation.ToTable("Invitations");
            invitation.HasKey(x => x.Id)
                .HasName("PK_Invitations");
            invitation.Property(x => x.Id)
                .HasConversion(x=>x.Value,x=>InvitationId.From(x))
                .ValueGeneratedNever()
                .HasColumnName("InvitationId")
                .IsRequired();
            invitation.Property(x => x.GroupId).IsRequired();
            invitation.HasOne<Group>()
                .WithMany()
                .HasForeignKey(x => x.GroupId)
                .HasConstraintName("FK_Invitations_Groups")
                .IsRequired();

            invitation.OwnsOne<ExpirationTimeContainer>(
                "_expirationTime",
                b =>
                {
                    b.Property<ExpirationTimeType>("_type")
                        .HasConversion<string>()
                        .IsRequired().HasColumnName("ExpirationTimeType");
                    b.Property<DateTimeOffset>("_start").IsRequired().HasColumnName("ExpirationStart");
                    b.Property<TimeSpan?>("_period").IsRequired(false).HasColumnName("ExpirationPeriod");
                    b.Ignore(x => x.ExpirationTime);
                }
            );
            invitation.Ignore(x => x.ExpirationTime);

            invitation.OwnsOne<UsageCounterContainer>(
                "_usageCounter",
                b =>
                {
                    b.Property<UsageCounterType>("_type")
                        .HasConversion<string>()
                        .IsRequired().HasColumnName("UsageCounterType");
                    b.Property<int?>("_value").IsRequired(false).HasColumnName("UsageCounter");
                    b.Ignore(x => x.UsageCounter);
                }
            );
            invitation.Ignore(x => x.UsageCounter);
            
            invitation.HasIndex(x => x.InvitationCode)
                .IsUnique();

            invitation.Property(x => x.InvitationCode)
                .HasConversion(x => x.Value, x => InvitationCode.From(x))
                .HasMaxLength(8)
                .IsRequired();
        }

        private static void BuildGroup(EntityTypeBuilder<Group> group)
        {
            group.ToTable("Groups");
            group.HasKey(x => x.Id)
                .HasName("PK_Groups");
            group.Property(x => x.Id)
                .HasConversion(x => x.Value, x => GroupId.From(x))
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

            group.Metadata.FindNavigation(nameof(Group.Members)).SetPropertyAccessMode(PropertyAccessMode.Field);
            group.OwnsMany(
                x => x.Members,
                m =>
                {
                    m.ToTable("Members");
                    m.HasKey(x => x.Id)
                        .HasName("PK_Members");
                    m.Property(x => x.Id)
                        .HasConversion(x => x.Value, x => MemberId.From(x))
                        .ValueGeneratedNever()
                        .IsRequired();
                    m.Property(x => x.UserId)
                        .HasConversion(x => x.Value, x => UserId.From(x))
                        .ValueGeneratedNever()
                        .IsRequired();
                    m.Property(x => x.IsOwner).IsRequired();
                    m.Property(x => x.Name)
                        .HasConversion(x => x.Value, x => MemberName.Create(x))
                        .IsRequired();
                    m.Property(x => x.CreatedAt).IsRequired();
                    m.OwnsMany(
                        x => x.Roles,
                        r =>
                        {
                            r.UsePropertyAccessMode(PropertyAccessMode.Field);
                            r.ToTable("MemberRoles");
                            r.HasKey("Value", "MemberId");
                            r.Property(x => x.Value)
                                .ValueGeneratedNever()
                                .HasColumnName("RoleId")
                                .IsRequired();
                        }
                    );
                }
            );

            group.Metadata.FindNavigation(nameof(Group.Roles)).SetPropertyAccessMode(PropertyAccessMode.Field);
            group.OwnsMany(
                x => x.Roles,
                r =>
                {
                    r.ToTable("Roles");
                    r.HasKey(x => x.Id)
                        .HasName("PK_Roles");
                    r.Property(x => x.Id)
                        .HasConversion(x => x.Value, x => RoleId.From(x))
                        .ValueGeneratedNever()
                        .IsRequired();
                    r.Property(x => x.Priority)
                        .HasConversion(x => x.Value, x => RolePriority.Create(x))
                        .IsRequired();
                    r.Property(x => x.Name)
                        .HasConversion(x => x.Value, x => RoleName.Create(x))
                        .IsRequired();
                    r.Property(x => x.Permissions)
                        .HasConversion(x => x.Value, x => Permissions.From(x))
                        .IsRequired();
                }
            );
        }
    }
}