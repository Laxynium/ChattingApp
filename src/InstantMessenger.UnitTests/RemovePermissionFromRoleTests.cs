using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Groups.Api.Features.Roles.RemovePermissionFromRole;
using InstantMessenger.Groups.Api.Queries;
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

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Fails_when_role_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));

            Func<Task> action = async ()=>await sut.SendAsync(new RemovePermissionFromRoleCommand(userId, groupId, roleId, "Administrator"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Fails_when_permission_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddRoleCommand(userId, groupId, roleId, "roleName"));

            Func<Task> action = async ()=>await sut.SendAsync(new RemovePermissionFromRoleCommand(userId, groupId, roleId, "SomeMissingPermission"));

            await action.Should().ThrowAsync<Exception>();
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
    }
}