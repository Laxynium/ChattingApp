using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InstantMessenger.Shared.Outbox.EFCore
{
    public class OutboxEntityConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id)
                .ValueGeneratedNever()
                .IsRequired();
            builder.Property(x => x.Data).IsRequired();
            builder.Property(x => x.OccurredOn).IsRequired();
            builder.Property(x => x.ProcessedDate).IsRequired(false);
        }
    }
}