﻿// <auto-generated />
using System;
using InstantMessenger.Identity.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InstantMessenger.Identity.Infrastructure.Database.Migrations
{
    [DbContext(typeof(IdentityContext))]
    partial class IdentityContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Identity")
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InstantMessenger.Identity.Domain.Entities.ActivationLink", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("ActivationLinkId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("PK_ActivationLinks");

                    b.HasIndex("UserId");

                    b.ToTable("ActivationLinks");
                });

            modelBuilder.Entity("InstantMessenger.Identity.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnName("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("bit");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id")
                        .HasName("PK_Users");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("InstantMessenger.Identity.Domain.Entities.ActivationLink", b =>
                {
                    b.HasOne("InstantMessenger.Identity.Domain.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("FK_ActivationLinks_Users")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("InstantMessenger.Identity.Domain.Entities.User", b =>
                {
                    b.OwnsOne("InstantMessenger.Identity.Domain.ValueObjects.Email", "Email", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnName("Email")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
