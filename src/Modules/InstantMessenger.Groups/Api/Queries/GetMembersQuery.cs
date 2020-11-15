using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public class MemberDto
    {
        public Guid UserId { get; set; }
        public Guid MemberId { get; set; }
        public string Name { get; set; }
        public bool IsOwner { get; set; }
        public DateTimeOffset CreatedAt { get; set;}
    }
    public class GetMembersQuery : IQuery<IEnumerable<MemberDto>>
    {
        public Guid GroupId { get; }
        public bool IsOwner { get; }

        public GetMembersQuery( Guid groupId, bool isOwner = false)
        {
            GroupId = groupId;
            IsOwner = isOwner;
        }
    }

    public class GetMembersHandler : IQueryHandler<GetMembersQuery, IEnumerable<MemberDto>>
    {
        private readonly GroupsContext _context;

        public GetMembersHandler(GroupsContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MemberDto>> HandleAsync(GetMembersQuery query)
        {
            var members = _context.Groups.AsNoTracking()
                .Where(x => x.Id == GroupId.From(query.GroupId))
                .SelectMany(x => x.Members);
            if (query.IsOwner)
            {
                members = members.Where(x => x.IsOwner);
            }
            return await members
                .Select(
                    x => new MemberDto
                    {
                        UserId = x.UserId.Value,
                        MemberId = x.Id.Value,
                        IsOwner = x.IsOwner,
                        CreatedAt = x.CreatedAt,
                        Name = x.Name.Value
                    }
                )
                .ToListAsync();
        }
    }
}