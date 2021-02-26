﻿// <auto-generated />
using System;
using InstantMessenger.Groups.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InstantMessenger.Groups.Infrastructure.Database.Migrations
{
    [DbContext(typeof(GroupsContext))]
    [Migration("20210226093036_Add Messages View")]
    partial class AddMessagesView
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Groups")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InstantMessenger.Groups.Domain.Entities.Channel", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ChannelId");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("GroupId");

                    b.ToTable("Channels");
                });

            modelBuilder.Entity("InstantMessenger.Groups.Domain.Entities.Group", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("GroupId");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id")
                        .HasName("PK_Groups");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("InstantMessenger.Groups.Domain.Entities.Invitation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("InvitationId");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("InvitationCode")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("Id")
                        .HasName("PK_Invitations");

                    b.HasIndex("GroupId");

                    b.HasIndex("InvitationCode")
                        .IsUnique();

                    b.ToTable("Invitations");
                });

            modelBuilder.Entity("InstantMessenger.Groups.Domain.Messages.Entities.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("MessageId");

                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("From")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("GroupId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("InstantMessenger.Groups.Infrastructure.Database.GroupMessageView", b =>
                {
                    b.Property<Guid>("ChannelId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("GroupId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("SenderAvatar")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("SenderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToView("View_GroupMessages", "Groups");
                });

            modelBuilder.Entity("InstantMessenger.Shared.Outbox.OutboxMessage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("OccurredOn")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("ProcessedDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("OutboxMessage");
                });

            modelBuilder.Entity("InstantMessenger.Groups.Domain.Entities.Channel", b =>
                {
                    b.HasOne("InstantMessenger.Groups.Domain.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("InstantMessenger.Groups.Domain.Entities.MemberPermissionOverride", "MemberPermissionOverrides", b1 =>
                        {
                            b1.Property<Guid>("ChannelId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("UserIdOfMember")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Permission")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Type")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ChannelId", "UserIdOfMember", "Permission");

                            b1.ToTable("ChannelMemberPermissionOverrides");

                            b1.WithOwner()
                                .HasForeignKey("ChannelId");
                        });

                    b.OwnsMany("InstantMessenger.Groups.Domain.Entities.RolePermissionOverride", "RolePermissionOverrides", b1 =>
                        {
                            b1.Property<Guid>("ChannelId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("RoleId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Permission")
                                .HasColumnType("nvarchar(450)");

                            b1.Property<string>("Type")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("ChannelId", "RoleId", "Permission");

                            b1.ToTable("ChannelRolePermissionOverrides");

                            b1.WithOwner()
                                .HasForeignKey("ChannelId");
                        });

                    b.Navigation("MemberPermissionOverrides");

                    b.Navigation("RolePermissionOverrides");
                });

            modelBuilder.Entity("InstantMessenger.Groups.Domain.Entities.Group", b =>
                {
                    b.OwnsMany("InstantMessenger.Groups.Domain.Entities.Member", "Members", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<DateTimeOffset>("CreatedAt")
                                .HasColumnType("datetimeoffset");

                            b1.Property<Guid>("GroupId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<bool>("IsOwner")
                                .HasColumnType("bit");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("Id")
                                .HasName("PK_Members");

                            b1.HasIndex("GroupId");

                            b1.ToTable("Members");

                            b1.WithOwner()
                                .HasForeignKey("GroupId");

                            b1.OwnsMany("InstantMessenger.Groups.Domain.ValueObjects.RoleId", "Roles", b2 =>
                                {
                                    b2.Property<Guid>("Value")
                                        .HasColumnType("uniqueidentifier")
                                        .HasColumnName("RoleId");

                                    b2.Property<Guid>("MemberId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.HasKey("Value", "MemberId");

                                    b2.HasIndex("MemberId");

                                    b2.ToTable("MemberRoles");

                                    b2.WithOwner()
                                        .HasForeignKey("MemberId");
                                });

                            b1.OwnsOne("InstantMessenger.SharedKernel.Avatar", "Avatar", b2 =>
                                {
                                    b2.Property<Guid>("MemberId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<byte[]>("Value")
                                        .IsRequired()
                                        .HasColumnType("varbinary(max)")
                                        .HasColumnName("Avatar");

                                    b2.HasKey("MemberId");

                                    b2.ToTable("Members");

                                    b2.WithOwner()
                                        .HasForeignKey("MemberId");
                                });

                            b1.Navigation("Avatar");

                            b1.Navigation("Roles");
                        });

                    b.OwnsMany("InstantMessenger.Groups.Domain.Entities.Role", "Roles", b1 =>
                        {
                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("GroupId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<int>("Permissions")
                                .HasColumnType("int");

                            b1.Property<int>("Priority")
                                .HasColumnType("int");

                            b1.HasKey("Id")
                                .HasName("PK_Roles");

                            b1.HasIndex("GroupId");

                            b1.ToTable("Roles");

                            b1.WithOwner()
                                .HasForeignKey("GroupId");
                        });

                    b.OwnsOne("InstantMessenger.Groups.Domain.ValueObjects.GroupName", "Name", b1 =>
                        {
                            b1.Property<Guid>("GroupId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("Name");

                            b1.HasKey("GroupId");

                            b1.ToTable("Groups");

                            b1.WithOwner()
                                .HasForeignKey("GroupId");
                        });

                    b.Navigation("Members");

                    b.Navigation("Name")
                        .IsRequired();

                    b.Navigation("Roles");
                });

            modelBuilder.Entity("InstantMessenger.Groups.Domain.Entities.Invitation", b =>
                {
                    b.HasOne("InstantMessenger.Groups.Domain.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .HasConstraintName("FK_Invitations_Groups")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("InstantMessenger.Groups.Domain.ValueObjects.ExpirationTimeContainer", "_expirationTime", b1 =>
                        {
                            b1.Property<Guid>("InvitationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<TimeSpan?>("_period")
                                .HasColumnType("time")
                                .HasColumnName("ExpirationPeriod");

                            b1.Property<DateTimeOffset>("_start")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("ExpirationStart");

                            b1.Property<string>("_type")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("ExpirationTimeType");

                            b1.HasKey("InvitationId");

                            b1.ToTable("Invitations");

                            b1.WithOwner()
                                .HasForeignKey("InvitationId");
                        });

                    b.OwnsOne("InstantMessenger.Groups.Domain.ValueObjects.UsageCounterContainer", "_usageCounter", b1 =>
                        {
                            b1.Property<Guid>("InvitationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("_type")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("UsageCounterType");

                            b1.Property<int?>("_value")
                                .HasColumnType("int")
                                .HasColumnName("UsageCounter");

                            b1.HasKey("InvitationId");

                            b1.ToTable("Invitations");

                            b1.WithOwner()
                                .HasForeignKey("InvitationId");
                        });

                    b.Navigation("_expirationTime");

                    b.Navigation("_usageCounter");
                });

            modelBuilder.Entity("InstantMessenger.Groups.Domain.Messages.Entities.Message", b =>
                {
                    b.HasOne("InstantMessenger.Groups.Domain.Entities.Channel", null)
                        .WithMany()
                        .HasForeignKey("ChannelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InstantMessenger.Groups.Domain.Entities.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
