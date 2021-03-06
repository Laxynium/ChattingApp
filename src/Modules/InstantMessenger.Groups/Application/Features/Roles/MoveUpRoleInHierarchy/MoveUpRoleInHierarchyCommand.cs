﻿using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Application.Features.Roles.MoveUpRoleInHierarchy
{
    public class MoveUpRoleInHierarchyCommand : ICommand
    {
        public Guid UserId { get; }
        public Guid GroupId { get; }
        public Guid RoleId { get; }

        public MoveUpRoleInHierarchyCommand(Guid userId, Guid groupId, Guid roleId)
        {
            UserId = userId;
            GroupId = groupId;
            RoleId = roleId;
        }
    }
    public class MoveUpRoleApiRequest : ICommand
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }

        public MoveUpRoleApiRequest(Guid groupId, Guid roleId)
        {
            GroupId = groupId;
            RoleId = roleId;
        }
    }

    internal sealed class MoveUpRoleInHierarchyHandler : ICommandHandler<MoveUpRoleInHierarchyCommand>
    {
        private readonly IGroupRepository _groupRepository;

        public MoveUpRoleInHierarchyHandler(IGroupRepository groupRepository)
        {
            _groupRepository = groupRepository;
        }
        public async Task HandleAsync(MoveUpRoleInHierarchyCommand command)
        {
            var groupId = GroupId.From(command.GroupId);
            var group = await _groupRepository.GetAsync(groupId) ?? throw new GroupNotFoundException(groupId);

            group.MoveUpRole(UserId.From(command.UserId), RoleId.From(command.RoleId));
        }
    }
}