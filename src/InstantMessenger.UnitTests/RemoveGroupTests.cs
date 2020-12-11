using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Group.Delete;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class RemoveGroupTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(
            async sut =>
            {
                var group = await GroupBuilder.For(sut).CreateGroup("group1")
                    .Build();
                var command = new RemoveGroupCommand(group.OwnerId, Guid.NewGuid());

                Func<Task> action = () => sut.SendAsync(command);

                await action.Should().ThrowAsync<GroupNotFoundException>();
            }
        );        
        
        [Fact]
        public async Task Fails_when_user_is_missing() => await Run(
            async sut =>
            {
                var group = await GroupBuilder.For(sut).CreateGroup("group1")
                    .Build();
                var command = new RemoveGroupCommand(Guid.NewGuid(), group.GroupId);

                Func<Task> action = () => sut.SendAsync(command);

                await action.Should().ThrowAsync<MemberNotFoundException>();
            }
        );       
        
        [Fact]
        public async Task Fails_when_user_is_not_owner_despite_of_permissions() => await Run(
            async sut =>
            {
                var group = await GroupBuilder.For(sut).CreateGroup("group1")
                    .AsOwner()
                    .CreateRole("role1")
                    .AddPermission("Administrator")
                    .AddPermission("ManageGroup")
                    .AddPermission("ManageRoles")
                    .AddPermission("ManageChannels").Build()
                    .CreateMember().AssignRole(1).Build()
                    .Build().Build();
                var command = new RemoveGroupCommand(group.Member(1).UserId, group.GroupId);

                Func<Task> action = () => sut.SendAsync(command);

                await action.Should().ThrowAsync<InsufficientPermissionsException>();
            }
        );

        [Fact]
        public async Task Removes_group_when_user_is_owner() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .Build();
            var command = new RemoveGroupCommand(group.OwnerId, group.GroupId);

            await sut.SendAsync(command);

            //then group is removed
            var groups = await sut.QueryAsync(
                new GetGroupsQuery(command.UserId, command.GroupId));
            groups.Should().BeEmpty();

            //then all members are removed
            var members = await sut.QueryAsync(new GetMembersQuery(group.OwnerId,group.GroupId));
            members.Should().BeEmpty();
            //then all channels are removed
            var channels = await sut.QueryAsync(new GetGroupChannelsQuery(group.OwnerId, group.GroupId));
            channels.Should().BeEmpty();
            //then all roles are removed
            var roles = await sut.QueryAsync(new GetRolesQuery(group.OwnerId, group.GroupId));
            roles.Should().BeEmpty();
        });
    }
}