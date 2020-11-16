using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Members.Add;
using InstantMessenger.Groups.Api.Features.Members.AssignRole;
using InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class AssignRoleToMemberTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, userIdOfMember, roleId) =
                (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            
            Func<Task> action = async ()=>await sut.SendAsync(new AssignRoleToMemberCommand(userId, groupId, userIdOfMember, roleId));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Fails_when_member_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, userIdOfMember, roleId) =
                (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));

            Func<Task> action = async ()=>await sut.SendAsync(new AssignRoleToMemberCommand(userId, groupId, userIdOfMember, roleId));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Fails_when_role_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, userIdOfMember, roleId) =
                (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddGroupMember(groupId, userIdOfMember));

            Func<Task> action = async ()=>await sut.SendAsync(new AssignRoleToMemberCommand(userId, groupId, userIdOfMember, roleId));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Owner_can_assign_role_to_member() => await Run(async sut =>
        {
            var (userId, groupId, userIdOfMember, roleId) =
                (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.CreateGroup(userId, groupId, "groupName");
            await sut.AddRole(userId, groupId, roleId, "roleName");
            await sut.AddMember(groupId, userIdOfMember);

            await sut.AssignRole(userId, groupId, userIdOfMember, roleId);

            var roles = await sut.QueryAsync(new GetMemberRolesQuery(userId, groupId, userIdOfMember));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(roleId);
                    x.Priority.Should().Be(0);
                    x.Name.Should().Be("roleName");
                },
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );
        });

        [Fact]
        public async Task Member_with_administrator_permission_cannot_assign_roles_with_higher_priority_than_his_highest_one() => await Run(
            async sut =>
            {
                var ids = new IdsSet();
                await sut.CreateGroup(ids.UserId, ids.GroupId, "groupName");
                await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(3), "role3");
                await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(2), "role2");
                await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(1), "role1");
                await sut.AddPermission(ids.UserId, ids.GroupId, ids.RoleId(1), "Administrator");
                await sut.AddMember(ids.GroupId, ids.UserIdOfMember(2));
                await sut.AddMember(ids.GroupId, ids.UserIdOfMember(1));
                await sut.AssignRole(ids.UserId, ids.GroupId, ids.UserIdOfMember(1), ids.RoleId(2));
                await sut.AssignRole(ids.UserId, ids.GroupId, ids.UserIdOfMember(1), ids.RoleId(1));

                Func<Task> action = async () => await sut.AssignRole(ids.UserIdOfMember(1), ids.GroupId, ids.UserIdOfMember(2), ids.RoleId(3));
                
                await action.Should().ThrowAsync<Exception>();
            }
        );

        [Fact]
        public async Task Member_with_administrator_permission_can_assign_role_to_member() => await Run(async sut =>
        {
            var (userId, groupId, userIdOfMember, roleId,newRoleId,userIdOfNewMember) =
                (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            await sut.CreateGroup(userId, groupId, "groupName");
            await sut.AddRole(userId, groupId, roleId, "roleName");
            await sut.AddRole(userId, groupId, newRoleId, "newRole");
            await sut.AddPermission(userId, groupId, roleId, "Administrator");
            await sut.AddMember(groupId, userIdOfMember);
            await sut.AssignRole(userId, groupId, userIdOfMember, roleId);
            await sut.AddMember(groupId, userIdOfNewMember);

            await sut.SendAsync(new AssignRoleToMemberCommand(userIdOfMember, groupId, userIdOfNewMember, newRoleId));

            var roles = await sut.QueryAsync(new GetMemberRolesQuery(userIdOfMember, groupId, userIdOfNewMember));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(newRoleId);
                    x.Priority.Should().Be(0);
                    x.Name.Should().Be("newRole");
                },
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );
        });
    }
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
    }
}