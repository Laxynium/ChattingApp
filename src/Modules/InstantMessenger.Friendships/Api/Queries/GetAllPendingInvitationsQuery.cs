using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Friendships.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Friendships.Api.Queries
{
    public class GetAllPendingInvitationsQuery : IQuery<IEnumerable<InvitationDto>>
    {
        public Guid PersonId { get; }

        public GetAllPendingInvitationsQuery(Guid personId)
        {
            PersonId = personId;
        }
    }
    public class GetAllPendingInvitationsHandler : IQueryHandler<GetAllPendingInvitationsQuery, IEnumerable<InvitationDto>>
    {
        private readonly FriendshipsContext _context;

        public GetAllPendingInvitationsHandler(FriendshipsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<InvitationDto>> HandleAsync(GetAllPendingInvitationsQuery query) =>
            await _context.Invitations.AsNoTracking()
                .Where(x => (x.ReceiverId == query.PersonId || x.SenderId == query.PersonId) && x.Status == InvitationStatus.Pending)
                .Select(x => new InvitationDto
                {
                    InvitationId = x.Id,
                    ReceiverId = x.ReceiverId,
                    SenderId = x.SenderId,
                    Status = x.Status.ToString(),
                    CreatedAt = x.CreatedAt,
                    Type = x.ReceiverId == query.PersonId ? "Incoming" : "Outgoing"
                })
                .ToListAsync();
    }
}