using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Members.Add;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Shared.Modules;
using InstantMessenger.UnitTests.Common;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class AddRoleTests : GroupsModuleUnitTestBase
    {
        public AddRoleTests() => Configure(sc => sc
            .Remove<IModuleClient>()
            .AddSingleton<IModuleClient>(x => new FakeModuleClient("test_user"))
        );

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task Fails_when_role_name_is_invalid(string roleName) => await Run(async sut =>
        {
            var command = new AddRoleCommand(Guid.NewGuid(), Guid.NewGuid(), roleName);

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Fails_when_member_do_not_have_correct_permission() => await Run(async sut =>
        {
            var (userId, groupId,newMemberUserId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddGroupMember(userId, newMemberUserId, groupId, "memberName"));

            Func<Task> action = async () => await sut.SendAsync(new AddRoleCommand(newMemberUserId, groupId, "roleName"));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Group_owner_can_add_member() => await Run(async sut =>
        {
            var (userId, groupId,newMemberUserId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));

            await sut.SendAsync(new AddGroupMember(userId, newMemberUserId, groupId, "memberName"));

        });

        [Fact]
        public async Task Member_with_administrator_permission_can_add_member() => await Run(async sut =>
        {
            var (userId, groupId,newMemberUserId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddGroupMember(userId, newMemberUserId, groupId, "memberName"));


        });

        [Fact]
        public async Task Member_with_manage_members_permission_can_add_member() => await Run(async sut =>
        {
            var (userId, groupId,newMemberUserId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());
            await sut.SendAsync(new CreateGroupCommand(userId, groupId, "groupName"));
            await sut.SendAsync(new AddGroupMember(userId, newMemberUserId, groupId, "memberName"));


        });
    }
}