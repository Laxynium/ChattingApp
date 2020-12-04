using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Infrastructure.Database;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.PrivateMessages.Api.Queries
{
    public class GetLatestConversationsQuery : IQuery<IEnumerable<ConversationDto>>
    {
        public Guid ParticipantId { get; }
        public int Number { get; }

        public GetLatestConversationsQuery(Guid participantId, int number)
        {
            ParticipantId = participantId;
            Number = number;
        }
    }

    public class GetLatestConversationsQueryHandler : IQueryHandler<GetLatestConversationsQuery, IEnumerable<ConversationDto>>
    {
        private readonly PrivateMessagesContext _context;

        public GetLatestConversationsQueryHandler(PrivateMessagesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ConversationDto>> HandleAsync(GetLatestConversationsQuery query)
        {


            var conversations = _context.Conversations.AsNoTracking()
                .Where(
                    x => x.FirstParticipant == new Participant(query.ParticipantId) ||
                         x.SecondParticipant == new Participant(query.ParticipantId)
                );

            var messages = _context.Messages.AsNoTracking()
                .Where(m => conversations.Select(x => x.Id).Contains(m.ConversationId))
                .GroupBy(x => x.ConversationId)
                .Select(
                    g => new
                    {
                        conversationId = g.Key,
                        updatedAt = g.Max(x => x.CreatedAt),
                        unreadCount = g.Count(x=>x.ReadAt == null)
                    }
                );

            var result = await conversations.Join(messages, x => x.Id, x => x.conversationId, 
                (x, y) => new {conversation = x, updatedAt = y.updatedAt})
                .OrderByDescending(x=>x.updatedAt)
                .Select(x=>x.conversation)
                .Take(query.Number)
                .Select(
                    x => new ConversationDto
                    {
                        ConversationId = x.Id.Value,
                        FirstParticipant = x.FirstParticipant.Id,
                        SecondParticipant = x.SecondParticipant.Id
                    }
                )
                .ToListAsync();

            return result;
        }
    }
}