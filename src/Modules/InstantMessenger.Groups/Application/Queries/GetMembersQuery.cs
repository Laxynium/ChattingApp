using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Application.Queries
{
    public class MemberDto
    {
        public Guid GroupId { get; set; }
        public Guid UserId { get; set; }
        public Guid MemberId { get; set; }
        public string Name { get; set; }
        public bool IsOwner { get; set; }
        public DateTimeOffset CreatedAt { get; set;}
    }
    public class GetMembersQuery : IQuery<IEnumerable<MemberDto>>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public bool IsOwner { get; }
        public Guid? UserIdOfMember { get; }

        public GetMembersQuery(Guid userId, Guid groupId, bool isOwner = false, Guid? userIdOfMember = null)
        {
            UserId = userId;
            GroupId = groupId;
            IsOwner = isOwner;
            UserIdOfMember = userIdOfMember;
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
                .Where(x=>x.Members.Select(m=>m.UserId).Any(id=>id == UserId.From(query.UserId)))
                .SelectMany(x => x.Members);
            if (query.IsOwner)
            {
                members = members.Where(x => x.IsOwner);
            }

            if (query.UserIdOfMember.HasValue)
            {
                members = members.Where(x => x.UserId == UserId.From(query.UserIdOfMember.Value));
            }
            return await members
                .Select(
                    x => new MemberDto
                    {
                        GroupId = query.GroupId,
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