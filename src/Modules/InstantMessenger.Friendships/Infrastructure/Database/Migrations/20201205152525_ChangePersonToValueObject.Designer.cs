﻿// <auto-generated />
using System;
using InstantMessenger.Friendships.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InstantMessenger.Friendships.Infrastructure.Database.Migrations
{
    [DbContext(typeof(FriendshipsContext))]
    [Migration("20201205152525_ChangePersonToValueObject")]
    partial class ChangePersonToValueObject
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Friendships")
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InstantMessenger.Friendships.Domain.Entities.Friendship", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("FriendshipId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("FirstPerson")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SecondPerson")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("PK_Friendships");

                    b.ToTable("Friendships");
                });

            modelBuilder.Entity("InstantMessenger.Friendships.Domain.Entities.Invitation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("InvitationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SenderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK_Invitations");

                    b.ToTable("Invitations");
                });
#pragma warning restore 612, 618
        }
    }
}
