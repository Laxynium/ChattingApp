using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Members.Add;
using InstantMessenger.Groups.Api.Features.Members.AssignRole;
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
            var (userId, groupId, user2Id, roleId,role2Id,user3Id) =
                (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            await sut.CreateGroup(userId, groupId, "groupName");
            await sut.AddRole(userId, groupId, roleId, "role1");
            await sut.AddPermission(userId, groupId, roleId, "Administrator");
            await sut.AddMember(groupId, user2Id);
            await sut.AssignRole(userId, groupId, user2Id, roleId);
            await sut.AddRole(userId, groupId, role2Id, "role2");
            await sut.AddMember(groupId, user3Id);

            await sut.SendAsync(new AssignRoleToMemberCommand(user2Id, groupId, user3Id, role2Id));

            var roles = await sut.QueryAsync(new GetMemberRolesQuery(user2Id, groupId, user3Id));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(role2Id);
                    x.Priority.Should().Be(0);
                    x.Name.Should().Be("role2");
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
        public async Task Member_with_manage_roles_cannot_assign_roles_with_higher_priority_than_his_highest_one() => await Run(async sut =>
        {
            var ids = new IdsSet();
            await sut.CreateGroup(ids.UserId, ids.GroupId, "groupName");
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(3), "role3");
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(2), "role2");
            await sut.AddRole(ids.UserId, ids.GroupId, ids.RoleId(1), "role1");
            await sut.AddPermission(ids.UserId, ids.GroupId, ids.RoleId(1), "ManageRoles");
            await sut.AddMember(ids.GroupId, ids.UserIdOfMember(2));
            await sut.AddMember(ids.GroupId, ids.UserIdOfMember(1));
            await sut.AssignRole(ids.UserId, ids.GroupId, ids.UserIdOfMember(1), ids.RoleId(2));
            await sut.AssignRole(ids.UserId, ids.GroupId, ids.UserIdOfMember(1), ids.RoleId(1));

            Func<Task> action = async () => await sut.AssignRole(ids.UserIdOfMember(1), ids.GroupId, ids.UserIdOfMember(2), ids.RoleId(3));

            await action.Should().ThrowAsync<Exception>();
        });
        [Fact]
        public async Task Member_with_manage_roles_permission_can_assign_role_to_member() => await Run(async sut =>
        {
            var (userId, groupId, user2Id, roleId,role2Id,user3Id) =
                (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            await sut.CreateGroup(userId, groupId, "groupName");
            await sut.AddRole(userId, groupId, roleId, "role1");
            await sut.AddPermission(userId, groupId, roleId, "ManageRoles");
            await sut.AddMember(groupId, user2Id);
            await sut.AssignRole(userId, groupId, user2Id, roleId);
            await sut.AddRole(userId, groupId, role2Id, "role2");
            await sut.AddMember(groupId, user3Id);

            await sut.SendAsync(new AssignRoleToMemberCommand(user2Id, groupId, user3Id, role2Id));

            var roles = await sut.QueryAsync(new GetMemberRolesQuery(user2Id, groupId, user3Id));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(role2Id);
                    x.Priority.Should().Be(0);
                    x.Name.Should().Be("role2");
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
}