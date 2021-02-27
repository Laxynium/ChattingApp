using InstantMessenger.PrivateMessages.Domain.Entities;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.Shared.Outbox;
using InstantMessenger.Shared.Outbox.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstantMessenger.PrivateMessages.Infrastructure.Database
{
    public class PrivateMessagesContext : DbContext
    {
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public PrivateMessagesContext(DbContextOptions<PrivateMessagesContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("PrivateMessages");

            BuildConversation(modelBuilder.Entity<Conversation>());

            BuildMessage(modelBuilder.Entity<Message>());

            modelBuilder.ApplyConfiguration(new OutboxEntityConfiguration());
        }

        private static void BuildMessage(EntityTypeBuilder<Message> message)
        {
            message.HasKey(x => x.Id)
                .HasName("PK_Messages");
            message.Property(x => x.Id)
                .HasConversion(x => x.Value, x => MessageId.From(x))
                .ValueGeneratedNever()
                .IsRequired();
            message.Property(x => x.From)
                .HasConversion(x => x.Id, x => new Participant(x))
                .IsRequired();
            message.Property(x => x.To)
                .HasConversion(x => x.Id, x => new Participant(x))
                .IsRequired();
            message.Property(x => x.ConversationId)
                .HasConversion(x => x.Value, x => new ConversationId(x))
                .IsRequired();

            message.HasOne<Conversation>()
                .WithMany()
                .HasForeignKey(x => x.ConversationId)
                .IsRequired();

            message.OwnsOne(x => x.Body, b =>
            {
                b.Property(x => x.TextContent)
                    .IsRequired()
                    .HasColumnName("Body");
            });
            message.Property(x => x.CreatedAt).IsRequired();
            message.Property(x => x.ReadAt).IsRequired(false);
            message.ToTable("Messages");
        }

        private static void BuildConversation(EntityTypeBuilder<Conversation> conversation)
        {
            conversation.HasKey(x => x.Id)
                .HasName("PK_Conversations");
            conversation.Property(x => x.Id)
                .HasConversion(x => x.Value, x => new ConversationId(x))
                .ValueGeneratedNever()
                .IsRequired();
            conversation.Property(x => x.FirstParticipant)
                .HasConversion(x => x.Id, x => new Participant(x))
                .IsRequired();
            conversation.Property(x => x.SecondParticipant)
                .HasConversion(x => x.Id, x => new Participant(x))
                .IsRequired();
            conversation.ToTable("Conversations");
        }
    }
}