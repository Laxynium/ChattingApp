﻿using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Members.AssignRole
{
    public class AssignRoleToMemberCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid UserIdOfMember { get; }
        public Guid RoleId { get; }

        public AssignRoleToMemberCommand(Guid userId, Guid groupId, Guid userIdOfMember, Guid roleId)
        {
            UserId = userId;
            GroupId = groupId;
            UserIdOfMember = userIdOfMember;
            RoleId = roleId;
        }
    }

    internal sealed class AssignRoleToMemberHandler : ICommandHandler<AssignRoleToMemberCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public AssignRoleToMemberHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task HandleAsync(AssignRoleToMemberCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            group.AssignRole(UserId.From(command.UserId), UserId.From(command.UserIdOfMember), RoleId.From(command.RoleId));
        }
    }
}