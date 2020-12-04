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
    public class GetLatestConversationsResponseItem
    {
        public Guid ConversationId { get; set; }
        public Guid FirstParticipant { get; set; }
        public Guid SecondParticipant { get; set; }
        public int UnreadMessagesCount { get; set; }
    }

    public class GetLatestConversationsQuery : IQuery<IEnumerable<GetLatestConversationsResponseItem>>
    {
        public Guid ParticipantId { get; }
        public int Number { get; }

        public GetLatestConversationsQuery(Guid participantId, int number)
        {
            ParticipantId = participantId;
            Number = number;
        }
    }

    public class GetLatestConversationsQueryHandler : IQueryHandler<GetLatestConversationsQuery, IEnumerable<GetLatestConversationsResponseItem>>
    {
        private readonly PrivateMessagesContext _context;

        public GetLatestConversationsQueryHandler(PrivateMessagesContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetLatestConversationsResponseItem>> HandleAsync(GetLatestConversationsQuery query)
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
                    }
                );

            var result = await conversations.Join(messages, x => x.Id, x => x.conversationId, 
                (x, y) => new {conversation = x, y.updatedAt})
                .OrderByDescending(x=>x.updatedAt)
                .Select(x=>new {x.conversation})
                .Take(query.Number)
                .Select(
                    x => new GetLatestConversationsResponseItem
                    {
                        ConversationId = x.conversation.Id.Value,
                        FirstParticipant = x.conversation.FirstParticipant.Id,
                        SecondParticipant = x.conversation.SecondParticipant.Id,
                        UnreadMessagesCount = _context.Messages
                            .AsNoTracking()
                            .Where(y=>y.ReadAt == null)
                            .Count(y => y.ConversationId == x.conversation.Id && y.To == new Participant(query.ParticipantId))
                    }
                )
                .ToListAsync();

            return result;
        }
    }
}