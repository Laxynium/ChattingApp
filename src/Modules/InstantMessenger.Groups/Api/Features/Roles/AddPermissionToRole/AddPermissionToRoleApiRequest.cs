﻿using System;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole
{
    public class AddPermissionToRoleApiRequest : ICommand
    {
        public Guid GroupId { get; }
        public Guid RoleId { get; }
        public string PermissionName { get; }
        public AddPermissionToRoleApiRequest( Guid groupId, Guid roleId, string permissionName)
        {
            GroupId = groupId;
            RoleId = roleId;
            PermissionName = permissionName;
        }
    }
}