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
    public class ConversationDto
    {
        public Guid ConversationId { get; set; }
        public Guid FirstParticipant { get; set; }
        public Guid SecondParticipant { get; set; }
    }
    public class GetConversationsQuery:IQuery<IEnumerable<ConversationDto>>
    {
        public Guid Participant { get; }

        public GetConversationsQuery(Guid participant)
        {
            Participant = participant;
        }
    }

    public class GetConversationsHandler : IQueryHandler<GetConversationsQuery, IEnumerable<ConversationDto>>
    {
        private readonly PrivateMessagesContext _context;

        public GetConversationsHandler(PrivateMessagesContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ConversationDto>> HandleAsync(GetConversationsQuery query)
        {
            return await _context.Conversations.AsNoTracking()
                .Where(x => x.FirstParticipant == new Participant(query.Participant) || x.SecondParticipant == new Participant(query.Participant))
                .Select(x=>new ConversationDto
                {
                    ConversationId = x.Id.Value,
                    FirstParticipant = x.FirstParticipant.Id,
                    SecondParticipant = x.SecondParticipant.Id
                })
                .ToListAsync();
        }
    }
}