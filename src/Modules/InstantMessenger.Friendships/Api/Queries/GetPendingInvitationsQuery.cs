using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Friendships.Infrastructure.Database;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Friendships.Api.Queries
{
    public class InvitationDto
    {
        public Guid InvitationId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
    public class GetPendingInvitationsQuery : IQuery<IEnumerable<InvitationDto>>
    {
        public Guid PersonId { get; }

        public GetPendingInvitationsQuery(Guid personId)
        {
            PersonId = personId;
        }
    }
    public class GetPendingInvitationsHandler: IQueryHandler<GetPendingInvitationsQuery, IEnumerable<InvitationDto>>
    {
        private readonly FriendshipsContext _context;

        public GetPendingInvitationsHandler(FriendshipsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<InvitationDto>> HandleAsync(GetPendingInvitationsQuery query) =>
            await _context.Invitations.AsNoTracking()
                .Where(x => x.ReceiverId == query.PersonId && x.Status == InvitationStatus.Pending)
                .Select(x => new InvitationDto
                {
                    InvitationId = x.Id,
                    ReceiverId = x.ReceiverId,
                    SenderId = x.SenderId,
                    Status =  x.Status.ToString(),
                    CreatedAt = x.CreatedAt
                })
                .ToListAsync();
    }
} 