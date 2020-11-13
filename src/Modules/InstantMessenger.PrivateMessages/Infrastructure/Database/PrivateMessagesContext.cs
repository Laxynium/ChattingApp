using InstantMessenger.PrivateMessages.Domain;
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
        }

        private static void BuildMessage(EntityTypeBuilder<Message> message)
        {
            message.HasKey(x => x.Id)
                .HasName("PK_Messages");
            message.Property(x => x.Id)
                .HasConversion(x => x.Value, x => MessageId.From(x))
                .ValueGeneratedNever()
                .IsRequired();
            message.OwnsOne(
                x => x.From,
                c =>
                {
                    c.Property(x => x.Id)
                        .HasColumnName(nameof(Message.From))
                        .IsRequired();
                }
            );
            message.OwnsOne(
                x => x.To,
                c =>
                {
                    c.Property(x => x.Id)
                        .HasColumnName(nameof(Message.To))
                        .IsRequired();
                }
            );
            message.OwnsOne(
                x => x.ConversationId,
                c =>
                {
                    c.Property(x => x.Value)
                        .HasColumnName(nameof(Message.ConversationId))
                        .IsRequired();
                }
            );
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
            conversation.OwnsOne(
                x => x.FirstParticipant,
                c =>
                {
                    c.Property(x => x.Id)
                        .HasColumnName(nameof(Conversation.FirstParticipant))
                        .IsRequired();
                }
            );
            conversation.OwnsOne(
                x => x.SecondParticipant,
                c =>
                {
                    c.Property(x => x.Id)
                        .HasColumnName(nameof(Conversation.SecondParticipant))
                        .IsRequired();
                }
            );
            conversation.ToTable("Conversations");
        }
    }
}