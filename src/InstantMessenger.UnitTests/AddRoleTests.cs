using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Members.Add;
using InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class AddRoleTests : GroupsModuleUnitTestBase
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task Fails_when_role_name_is_invalid(string roleName) => await Run(async sut =>
        {
            var command = new AddRoleCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), roleName);

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Fails_when_member_do_not_have_correct_permission() => await Run(async sut =>
        {
            var (userId, groupId,newMemberUserId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddGroupMember(groupId, newMemberUserId));

            Func<Task> action = async () => await sut.SendAsync(new AddRoleCommand(newMemberUserId, groupId, Guid.NewGuid(), "roleName"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Owner_can_add_new_role() => await Run(async sut =>
        {
            var (userId, groupId) = (Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));

            await sut.SendAsync(new AddRoleCommand(userId, groupId, Guid.NewGuid(), "roleName"));

            var roles = await sut.QueryAsync(new GetRolesQuery(userId, groupId));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
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
        public async Task Member_with_administrator_permission_can_add_new_role() => await Run(async sut =>
        {
            var (userId, groupId,userIdOfMember,roleId,newRoleId) = 
                (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddRoleCommand(userId, groupId, roleId, "roleName"));
            await sut.SendAsync(new AddPermissionToRoleCommand(userId, groupId, roleId,"Administrator"));
            await sut.SendAsync(new AddGroupMember(groupId, userIdOfMember));
            await sut.AssignRole(userId, groupId, userIdOfMember, roleId);

            await sut.SendAsync(new AddRoleCommand(userIdOfMember, groupId, newRoleId, "newRole"));

            var roles = await sut.QueryAsync(new GetRolesQuery(userId, groupId));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(roleId);
                    x.Priority.Should().Be(1);
                    x.Name.Should().Be("roleName");
                },
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
}