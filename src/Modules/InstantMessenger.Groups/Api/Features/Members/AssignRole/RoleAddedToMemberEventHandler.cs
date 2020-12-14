﻿using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Hubs;
using InstantMessenger.Groups.Api.IntegrationEvents;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Api.ResponseDtos;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace InstantMessenger.Groups.Api.Features.Members.AssignRole
{
    public class RoleAddedToMemberEventHandler : IIntegrationEventHandler<RoleAddedToMemberEvent>
    {
        private readonly IHubContext<GroupsHub, IGroupsHubContract> _contract;
        private readonly GroupsContext _groupsContext;

        public RoleAddedToMemberEventHandler(IHubContext<GroupsHub, IGroupsHubContract> contract, GroupsContext groupsContext)
        {
            _contract = contract;
            _groupsContext = groupsContext;
        }
        public async Task HandleAsync(RoleAddedToMemberEvent @event)
        {
            var members = (await _groupsContext.Groups
                    .AsNoTracking()
                    .Where(g => g.Id == GroupId.From(@event.GroupId))
                    .SelectMany(g => g.Members)
                    .Select(m => m.UserId)
                    .ToListAsync())
                .Select(id => id.Value.ToString("N"))
                .ToList();
            await _contract.Clients.Users(members).OnRoleAddedToMember(new MemberRoleDto(@event.UserId, new RoleDto(@event.GroupId, @event.RoleId, @event.RoleName, @event.RolePriority)));

            await _contract.Clients.Users(members).OnAllowedActionsChanged(new AllowedActionDto(@event.GroupId));
        }
    }
}