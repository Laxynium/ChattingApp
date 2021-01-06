using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.PrivateMessages.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.PrivateMessages.Application.Queries
{
    public class GetMessageQuery : IQuery<MessageDto>
    {
        public Guid MessageId { get; }

        public GetMessageQuery(Guid messageId)
        {
            MessageId = messageId;
        }
    }

    public class GetMessageHandler : IQueryHandler<GetMessageQuery, MessageDto>
    {
        private readonly PrivateMessagesContext _context;

        public GetMessageHandler(PrivateMessagesContext context)
        {
            _context = context;
        }
        public async Task<MessageDto> HandleAsync(GetMessageQuery query) => await _context.Messages.AsNoTracking()
            .Where(x => x.Id == MessageId.From(query.MessageId))
            .Select(
                x => new MessageDto
                {
                    MessageId = x.Id.Value,
                    ConversationId = x.ConversationId.Value,
                    Text = x.Body.TextContent,
                    CreatedAt = x.CreatedAt,
                    From = x.From.Id,
                    To = x.To.Id,
                    ReadAt = x.ReadAt
                }
            ).FirstOrDefaultAsync();
    }
}