using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.PrivateMessages.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.PrivateMessages.Api.Queries
{
    public class GetConversationQuery : IQuery<ConversationDto>
    {
        public Guid ConversationId { get; }

        public GetConversationQuery(Guid conversationId)
        {
            ConversationId = conversationId;
        }
    }
    public class GetConversationHandler : IQueryHandler<GetConversationQuery, ConversationDto>
    {
        private readonly PrivateMessagesContext _context;

        public GetConversationHandler(PrivateMessagesContext context)
        {
            _context = context;
        }
        public async Task<ConversationDto> HandleAsync(GetConversationQuery query)
        {
            return await _context.Conversations.AsNoTracking()
                .Where(x => x.Id == new ConversationId(query.ConversationId))
                .Select(x => new ConversationDto
                {
                    ConversationId = x.Id.Value,
                    FirstParticipant = x.FirstParticipant.Id,
                    SecondParticipant = x.SecondParticipant.Id
                })
                .FirstOrDefaultAsync();
        }
    }

}