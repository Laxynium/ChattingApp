using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class AddPermissionToRoleTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(userId, groupId, roleId, "Administrator"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Fails_when_role_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(userId, groupId, roleId, "Administrator"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Fails_when_permission_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddRoleCommand(userId, groupId, roleId, "roleName"));

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(userId, groupId, roleId, "SomeMissingPermission"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Member_without_correct_permissions_cannot_add_permission_to_role() => await Run(async sut =>
        {
            var (userId, groupId, roleId, role2Id, user2Id) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.AddRole(userId, groupId, roleId, "role1");
            await sut.AddMember(groupId, user2Id);
            await sut.AssignRole(userId,groupId,user2Id,roleId);
            await sut.AddRole(userId, groupId, role2Id, "role2");

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(user2Id, groupId, role2Id, "Administrator"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Owner_can_add_permission_to_role() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddRoleCommand(userId, groupId,roleId, "roleName"));

            await sut.SendAsync(new AddPermissionToRoleCommand(userId, groupId, roleId,"Administrator"));

            var permissions = await sut.QueryAsync(new GetRolePermissionsQuery(groupId, roleId));
            permissions.Should().SatisfyRespectively(
                x =>
                {
                    x.Name.Should().Be("Administrator");
                    x.Code.Should().Be(0x1);
                }
            );
        });

        [Fact]
        public async Task Member_with_administrator_permission_cannot_add_permission_to_role_with_higher_priority() => await Run(async sut =>
        {
            var ids = new IdsSet();
            await sut.CreateGroup(ids.UserId, ids.GroupId, "group1");
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(1), "role1");
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(2), "role2");
            await sut.AddPermission(ids.UserId, ids.GroupId, ids.RoleId(2), "Administrator");
            await sut.AddMember(ids.GroupId, ids.UserIdOfMember(1));
            await sut.AssignRole(ids.UserId, ids.GroupId, ids.UserIdOfMember(1), ids.RoleId(2));

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(ids.UserIdOfMember(1), ids.GroupId, ids.RoleId(1), "ManageRoles"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Member_with_administrator_permission_cannot_add_permission_to_role_with_same_priority() => await Run(async sut =>
        {
            var ids = new IdsSet();
            await sut.CreateGroup(ids.UserId, ids.GroupId, "group1");
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(1), "role1");
            await sut.AddPermission(ids.UserId, ids.GroupId, ids.RoleId(1), "Administrator");
            await sut.AddMember(ids.GroupId, ids.UserIdOfMember(1));
            await sut.AssignRole(ids.UserId, ids.GroupId, ids.UserIdOfMember(1), ids.RoleId(1));

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(ids.UserIdOfMember(1), ids.GroupId, ids.RoleId(1), "ManageRoles"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Member_with_administrator_permission_can_add_permission_to_role() => await Run(async sut =>
        {
            var (userId, groupId, roleId,role2Id,user2Id) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.AddRole(userId, groupId, roleId, "role1");
            await sut.AddPermission(userId, groupId, roleId, "Administrator");
            await sut.AddMember(groupId, user2Id);
            await sut.AssignRole(userId,groupId, user2Id, roleId);
            await sut.AddRole(userId, groupId, role2Id, "role2");

            await sut.SendAsync(new AddPermissionToRoleCommand(user2Id, groupId, role2Id,"ManageRoles"));

            var permissions = await sut.QueryAsync(new GetRolePermissionsQuery(groupId, role2Id));
            permissions.Should().SatisfyRespectively(
                x =>
                {
                    x.Name.Should().Be("ManageRoles");
                    x.Code.Should().Be(0x4);
                }
            );
        });


        [Fact]
        public async Task Member_with_manage_roles_permission_cannot_add_permission_to_role_with_higher_priority() => await Run(async sut =>
        {
            var ids = new IdsSet();
            await sut.CreateGroup(ids.UserId, ids.GroupId, "group1");
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(1), "role1");
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(2), "role2");
            await sut.AddPermission(ids.UserId, ids.GroupId, ids.RoleId(2), "ManageRoles");
            await sut.AddMember(ids.GroupId, ids.UserIdOfMember(1));
            await sut.AssignRole(ids.UserId, ids.GroupId, ids.UserIdOfMember(1), ids.RoleId(2));

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(ids.UserIdOfMember(1), ids.GroupId, ids.RoleId(1), "ManageRoles"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Member_with_manage_roles_permission_cannot_add_permission_to_role_with_same_priority() => await Run(async sut =>
        {
            var ids = new IdsSet();
            await sut.CreateGroup(ids.UserId, ids.GroupId, "group1");
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(1), "role1");
            await sut.AddPermission(ids.UserId, ids.GroupId, ids.RoleId(1), "ManageRoles");
            await sut.AddMember(ids.GroupId, ids.UserIdOfMember(1));
            await sut.AssignRole(ids.UserId, ids.GroupId, ids.UserIdOfMember(1), ids.RoleId(1));

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(ids.UserIdOfMember(1), ids.GroupId, ids.RoleId(1), "ManageRoles"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Member_with_manage_roles_permission_cannot_add_permission_to_role_which_he_do_not_possess() => await Run(async sut =>
        {
            var ids = new IdsSet();
            await sut.CreateGroup(ids.UserId, ids.GroupId, "group1");
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(1), "role1");
            await sut.AddPermission(ids.UserId, ids.GroupId, ids.RoleId(1), "ManageRoles");
            await sut.AddMember(ids.GroupId, ids.UserIdOfMember(1));
            await sut.AssignRole(ids.UserId, ids.GroupId, ids.UserIdOfMember(1), ids.RoleId(1));
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(2), "role2");

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(ids.UserIdOfMember(1), ids.GroupId, ids.RoleId(2), "ManageGroup"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Member_with_manage_roles_permission_can_add_permission_to_role() => await Run(async sut =>
        {
            var (userId, groupId, roleId, role2Id, user2Id) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.AddRole(userId, groupId, roleId, "role1");
            await sut.AddPermission(userId, groupId, roleId, "ManageRoles");
            await sut.AddMember(groupId, user2Id);
            await sut.AssignRole(userId, groupId, user2Id, roleId);
            await sut.AddRole(userId, groupId, role2Id, "role2");

            await sut.SendAsync(new AddPermissionToRoleCommand(user2Id, groupId, role2Id, "ManageRoles"));

            var permissions = await sut.QueryAsync(new GetRolePermissionsQuery(groupId, role2Id));
            permissions.Should().SatisfyRespectively(
                x =>
                {
                    x.Name.Should().Be("ManageRoles");
                    x.Code.Should().Be(0x4);
                }
            );
        });
    }
}