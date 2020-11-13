﻿// <auto-generated />
using System;
using InstantMessenger.PrivateMessages.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InstantMessenger.PrivateMessages.Infrastructure.Database.Migrations
{
    [DbContext(typeof(PrivateMessagesContext))]
    [Migration("20201111161518_Fix Messages")]
    partial class FixMessages
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("PrivateMessages")
                .HasAnnotation("ProductVersion", "3.1.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("InstantMessenger.PrivateMessages.Domain.Conversation", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id")
                        .HasName("PK_Conversations");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("InstantMessenger.PrivateMessages.Domain.Message", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id")
                        .HasName("PK_Messages");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("InstantMessenger.PrivateMessages.Domain.Conversation", b =>
                {
                    b.OwnsOne("InstantMessenger.PrivateMessages.Domain.Participant", "FirstParticipant", b1 =>
                        {
                            b1.Property<Guid>("ConversationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Id")
                                .HasColumnName("FirstParticipant")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("ConversationId");

                            b1.ToTable("Conversations");

                            b1.WithOwner()
                                .HasForeignKey("ConversationId");
                        });

                    b.OwnsOne("InstantMessenger.PrivateMessages.Domain.Participant", "SecondParticipant", b1 =>
                        {
                            b1.Property<Guid>("ConversationId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Id")
                                .HasColumnName("SecondParticipant")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("ConversationId");

                            b1.ToTable("Conversations");

                            b1.WithOwner()
                                .HasForeignKey("ConversationId");
                        });
                });

            modelBuilder.Entity("InstantMessenger.PrivateMessages.Domain.Message", b =>
                {
                    b.OwnsOne("InstantMessenger.PrivateMessages.Domain.ConversationId", "ConversationId", b1 =>
                        {
                            b1.Property<Guid>("MessageId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Value")
                                .HasColumnName("ConversationId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("MessageId");

                            b1.ToTable("Messages");

                            b1.WithOwner()
                                .HasForeignKey("MessageId");
                        });

                    b.OwnsOne("InstantMessenger.PrivateMessages.Domain.MessageBody", "Body", b1 =>
                        {
                            b1.Property<Guid>("MessageId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("TextContent")
                                .IsRequired()
                                .HasColumnName("Body")
                                .HasColumnType("nvarchar(max)");

                            b1.HasKey("MessageId");

                            b1.ToTable("Messages");

                            b1.WithOwner()
                                .HasForeignKey("MessageId");
                        });

                    b.OwnsOne("InstantMessenger.PrivateMessages.Domain.Participant", "From", b1 =>
                        {
                            b1.Property<Guid>("MessageId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Id")
                                .HasColumnName("From")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("MessageId");

                            b1.ToTable("Messages");

                            b1.WithOwner()
                                .HasForeignKey("MessageId");
                        });

                    b.OwnsOne("InstantMessenger.PrivateMessages.Domain.Participant", "To", b1 =>
                        {
                            b1.Property<Guid>("MessageId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Id")
                                .HasColumnName("To")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("MessageId");

                            b1.ToTable("Messages");

                            b1.WithOwner()
                                .HasForeignKey("MessageId");
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
