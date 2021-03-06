﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Friendships.Domain.ValueObjects;
using InstantMessenger.Friendships.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Friendships.Application.Queries
{
    public class InvitationDto
    {
        public Guid InvitationId { get; set; }
        public Guid SenderId { get; set; }
        public Guid ReceiverId { get; set; }
        public string Status { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string Type { get; set; }
    }
    public class GetIncomingPendingInvitationsQuery : IQuery<IEnumerable<InvitationDto>>
    {
        public Guid PersonId { get; }

        public GetIncomingPendingInvitationsQuery(Guid personId)
        {
            PersonId = personId;
        }
    }
    public class GetIncomingPendingInvitationsHandler: IQueryHandler<GetIncomingPendingInvitationsQuery, IEnumerable<InvitationDto>>
    {
        private readonly FriendshipsContext _context;

        public GetIncomingPendingInvitationsHandler(FriendshipsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<InvitationDto>> HandleAsync(GetIncomingPendingInvitationsQuery query) =>
            await _context.Invitations.AsNoTracking()
                .Where(x => x.ReceiverId == query.PersonId && x.Status == InvitationStatus.Pending)
                .Select(x => new InvitationDto
                {
                    InvitationId = x.Id,
                    ReceiverId = x.ReceiverId,
                    SenderId = x.SenderId,
                    Status =  x.Status.ToString(),
                    CreatedAt = x.CreatedAt,
                    Type = "Incoming"
                })
                .ToListAsync();
    }
} 