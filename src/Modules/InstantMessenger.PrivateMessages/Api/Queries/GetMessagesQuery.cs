using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Infrastructure.Database;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.PrivateMessages.Api.Queries
{
    public class MessageDto
    {
        public Guid MessageId { get; set; }
        public Guid ConversationId { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
        public string Text { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? ReadAt { get; set; }
    }
    public class GetMessagesQuery : IQuery<IEnumerable<MessageDto>>
    {
        public Guid Participant { get; }
        public Guid ConversationId { get; }

        public GetMessagesQuery(Guid participant, Guid conversationId)
        {
            Participant = participant;
            ConversationId = conversationId;
        }
    }

    public class GetMessagesHandler : IQueryHandler<GetMessagesQuery, IEnumerable<MessageDto>>
    {
        private readonly PrivateMessagesContext _context;

        public GetMessagesHandler(PrivateMessagesContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MessageDto>> HandleAsync(GetMessagesQuery query) => await _context.Messages.AsNoTracking()
            .OrderBy(x=>x.CreatedAt)
            .Where(x => x.ConversationId == new ConversationId(query.ConversationId)
                        &&
                        (x.From == new Participant(query.Participant) || x.To == new Participant(query.Participant)))
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
            ).ToListAsync();
    }
}