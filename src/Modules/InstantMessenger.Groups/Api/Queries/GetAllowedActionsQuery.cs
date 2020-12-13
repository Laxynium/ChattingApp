using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Messages.Queries;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Queries
{
    public class AllowedActions : ValueObject
    {
        public static readonly AllowedActions All = new AllowedActions("all");
        public static readonly AllowedActions ManageInvitations = new AllowedActions("manage_invitations");
        public static readonly AllowedActions ManageRolesGeneral = new AllowedActions("manage_roles_general");
        public static readonly AllowedActions ManageChannelsGeneral = new AllowedActions("manage_channels_general");
        public static readonly AllowedActions KickMembers = new AllowedActions("kick_members");
        public static AllowedActions ManageRoles(IEnumerable<Guid> channels) => new AllowedActions("manage_roles",true, channels.ToList());
        public static AllowedActions ManageChannels(IEnumerable<Guid> channels) => new AllowedActions("manage_channels",true, channels.ToList());
        public static AllowedActions SendMessages(IEnumerable<Guid>channels) => new AllowedActions("send_messages",true,channels.ToList());
        public static AllowedActions ReadMessages(IEnumerable<Guid>channels) => new AllowedActions("read_messages", true,channels.ToList());
        public string Name { get; }
        public bool IsChannelSpecific { get; } = false;
        public IEnumerable<Guid> Channels { get; }

        public AllowedActions(string name, bool isChannelSpecific = false, IReadOnlyList<Guid> channels = null)
        {
            Name = name;
            IsChannelSpecific = isChannelSpecific;
            Channels = !isChannelSpecific ? new List<Guid>() : channels ?? new List<Guid>();
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Name;
            yield return IsChannelSpecific;
            yield return Channels.OrderBy(x=>x.ToString());
        }
    }

    public class GetAllowedActionsQuery : IQuery<IEnumerable<AllowedActions>>
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }

        public GetAllowedActionsQuery(Guid userId, Guid groupId)
        {
            UserId = userId;
            GroupId = groupId;
        }
    }

    public class GetAllowedActionsQueryHandler : IQueryHandler<GetAllowedActionsQuery, IEnumerable<AllowedActions>>
    {
        private readonly IGroupRepository _groupRepository;
        private readonly GroupsContext _context;

        public GetAllowedActionsQueryHandler(IGroupRepository groupRepository, GroupsContext context)
        {
            _groupRepository = groupRepository;
            _context = context;
        }
        public async Task<IEnumerable<AllowedActions>> HandleAsync(GetAllowedActionsQuery query)
        {
            var group = await _groupRepository.GetAsync(GroupId.From(query.GroupId));
            if(group is null)
                return new List<AllowedActions>();

            var channels = await _context.Channels
                .AsNoTracking()
                .Where(g => g.GroupId == GroupId.From(query.GroupId))
                .ToListAsync();

            var allowedActions = group.GetAllowedActions(UserId.From(query.UserId), channels);

            return allowedActions;
        }
    }
}