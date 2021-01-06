using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Application.Features.Group.Create;
using InstantMessenger.Groups.Application.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Application.Features.Roles.AddRole;
using InstantMessenger.Groups.Application.Features.Roles.RemovePermissionFromRole;
using InstantMessenger.Groups.Application.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class RemovePermissionFromRoleTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            Func<Task> action = async () => await sut.SendAsync(new RemovePermissionFromRoleCommand(userId, groupId, roleId, "Administrator"));

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_role_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));

            Func<Task> action = async ()=>await sut.SendAsync(new RemovePermissionFromRoleCommand(userId, groupId, roleId, "Administrator"));

            await action.Should().ThrowAsync<RoleNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_permission_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddRoleCommand(userId, groupId, roleId, "roleName"));

            Func<Task> action = async ()=>await sut.SendAsync(new RemovePermissionFromRoleCommand(userId, groupId, roleId, "SomeMissingPermission"));

            await action.Should().ThrowAsync<PermissionNotFoundException>();
        });

        [Fact]
        public async Task Member_without_correct_permissions_cannot_remove_permission_from_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateRole("role2").AddPermission("Administrator").Build()
                .CreateMember().AssignRole(1).Build()
                .Build()
                .Build();


            Func<Task> action = async () => await sut.SendAsync(new RemovePermissionFromRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId, "Administrator"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Owner_can_remove_permission_from_role() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddRoleCommand(userId, groupId, roleId, "roleName"));
            await sut.SendAsync(new AddPermissionToRoleCommand(userId, groupId, roleId, "Administrator"));

            await sut.SendAsync(new RemovePermissionFromRoleCommand(userId, groupId, roleId, "Administrator"));

            var permissions = await sut.QueryAsync(new GetRolePermissionsQuery(groupId, roleId));
            permissions.Should().BeEmpty();
        });

        [Fact]
        public async Task Member_with_administrator_permission_cannot_remove_permission_from_role_with_higher_priority() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("Administrator").Build()
                .CreateRole("role2").AddPermission("Administrator").Build()
                .CreateMember().AssignRole(2).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemovePermissionFromRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId, "Administrator"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_administrator_permission_cannot_remove_permission_from_role_with_same_priority() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("Administrator").Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemovePermissionFromRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId, "Administrator"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_administrator_permission_can_remove_permission_from_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("Administrator").Build()
                .CreateRole("role2").AddPermission("ManageInvitations").Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            await sut.SendAsync(new RemovePermissionFromRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(2).RoleId, "ManageInvitations"));

            var permissions = await sut.QueryAsync(new GetRolePermissionsQuery(group.GroupId, group.Role(2).RoleId));
            permissions.Should().BeNullOrEmpty();
        });

        [Fact]
        public async Task Member_with_manage_roles_permission_cannot_remove_permission_from_role_with_higher_priority() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("Administrator").Build()
                .CreateRole("role2").AddPermission("ManageRoles").Build()
                .CreateMember().AssignRole(2).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemovePermissionFromRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId, "Administrator"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_manage_roles_permission_cannot_remove_permission_from_role_with_same_priority() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageRoles").Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemovePermissionFromRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId, "ManageRoles"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_manage_roles_permission_cannot_remove_permission_which_he_do_not_possess() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageRoles").Build()
                .CreateRole("role2").AddPermission("ManageInvitations").Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemovePermissionFromRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(2).RoleId, "ManageInvitations"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_manage_roles_permission_can_remove_permission_from_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageRoles").Build()
                .CreateRole("role2").AddPermission("ManageRoles").Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            await sut.SendAsync(new RemovePermissionFromRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(2).RoleId, "ManageRoles"));

            var permissions = await sut.QueryAsync(new GetRolePermissionsQuery(group.GroupId, group.Role(2).RoleId));
            permissions.Should().BeNullOrEmpty();
        });

    }
}