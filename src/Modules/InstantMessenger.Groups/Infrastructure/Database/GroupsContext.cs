using System;
using System.Linq;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Messages.Entities;
using InstantMessenger.Groups.Domain.Messages.ValueObjects;
using InstantMessenger.Groups.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstantMessenger.Groups.Infrastructure.Database
{
    public interface IQueryGroupContext
    {
        IQueryable<Group> GroupsQuery { get; }
        IQueryable<Invitation> InvitationsQuery { get; }
        IQueryable<Channel> ChannelsQuery { get; }
        IQueryable<Message> MessagesQuery { get; }
    }
    public class GroupsContext : DbContext, IQueryGroupContext
    {
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Invitation> Invitations { get; set; }
        public virtual DbSet<Channel> Channels { get; set; }
        public virtual DbSet<Message> Messages { get; set; }

        public IQueryable<Group> GroupsQuery => Groups.AsNoTracking();
        public IQueryable<Invitation> InvitationsQuery => Invitations.AsNoTracking();
        public IQueryable<Channel> ChannelsQuery => Channels.AsNoTracking();
        public IQueryable<Message> MessagesQuery => Messages.AsNoTracking();

        public GroupsContext(DbContextOptions<GroupsContext>options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("Groups");
            modelBuilder.Entity<Group>(Build);
            modelBuilder.Entity<Invitation>(Build);
            modelBuilder.Entity<Channel>(Build);
            modelBuilder.Entity<Message>(Build);
        }

        private static void Build(EntityTypeBuilder<Invitation> invitation)
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

        private static void Build(EntityTypeBuilder<Group> group)
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

        private static void Build(EntityTypeBuilder<Channel> channel)
        {
            channel.ToTable("Channels");
            channel.HasKey(x => x.Id);
            channel.Property(x => x.Id)
                .HasConversion(x => x.Value, x => ChannelId.From(x))
                .ValueGeneratedNever()
                .HasColumnName("ChannelId");

            channel.Property(x => x.GroupId)
                .HasConversion(x=>x.Value,x=>GroupId.From(x))
                .IsRequired();

            channel.HasOne<Group>()
                .WithMany()
                .HasForeignKey(x=>x.GroupId)
                .IsRequired();

            channel.Property(x => x.Name)
                .HasConversion(x => x.Value, x => ChannelName.Create(x))
                .IsRequired();

            channel.Metadata.FindNavigation(nameof(Channel.MemberPermissionOverrides)).SetPropertyAccessMode(PropertyAccessMode.Field);
            channel.OwnsMany(
                x => x.MemberPermissionOverrides,
                b =>
                {
                    b.ToTable("ChannelMemberPermissionOverrides");
                    b.HasKey(x => new {x.ChannelId,x.UserIdOfMember, x.Permission});
                    b.Property(x => x.ChannelId)
                        .HasConversion(x => x.Value, x => ChannelId.From(x))
                        .IsRequired();
                    b.Property(x => x.UserIdOfMember)
                        .HasConversion(x=>x.Value,x=>UserId.From(x))
                        .IsRequired();
                    b.Property(x => x.Type).HasConversion<string>()
                        .IsRequired();
                    b.Property(x => x.Permission)
                        .HasConversion(x => x.Name, x => Permission.FromName(x, true))
                        .IsRequired();
                }
            );

            channel.Metadata.FindNavigation(nameof(Channel.RolePermissionOverrides)).SetPropertyAccessMode(PropertyAccessMode.Field);
            channel.OwnsMany(
                x => x.RolePermissionOverrides,
                b =>
                {
                    b.ToTable("ChannelRolePermissionOverrides");
                    b.HasKey(x => new { x.ChannelId, x.RoleId, x.Permission });
                    b.Property(x => x.ChannelId)
                        .HasConversion(x => x.Value, x => ChannelId.From(x))
                        .IsRequired();
                    b.Property(x => x.RoleId)
                        .HasConversion(x=>x.Value,x=>RoleId.From(x))
                        .IsRequired();
                    b.Property(x => x.Type).HasConversion<string>()
                        .IsRequired();
                    b.Property(x => x.Permission)
                        .HasConversion(x => x.Name, x => Permission.FromName(x, true))
                        .IsRequired();
                }
            );
        }

        private static void Build(EntityTypeBuilder<Message> message)
        {
            message.HasKey(x => x.Id);
            message.Property(x => x.Id)
                .ValueGeneratedNever()
                .HasConversion(x=>x.Value,x=>MessageId.From(x))
                .IsRequired()
                .HasColumnName("MessageId");

            message.Property(x => x.GroupId)
                .HasConversion(x => x.Value, x => GroupId.From(x))
                .IsRequired();

            message.HasOne<Group>()
                .WithMany()
                .HasForeignKey(x => x.GroupId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            message.Property(x => x.ChannelId)
                .HasConversion(x => x.Value, 
                    x => ChannelId.From(x))
                .IsRequired();
            message.HasOne<Channel>()
                .WithMany()
                .HasForeignKey(x => x.ChannelId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            message.Property(x => x.From)
                .HasConversion(x => x.Value, x => UserId.From(x))
                .IsRequired();

            message.Property(x => x.CreatedAt).IsRequired();
            message.Property(x => x.Content)
                .HasConversion(x=>x.Value,x=>MessageContent.From(x))
                .IsRequired();
        }
    }
}