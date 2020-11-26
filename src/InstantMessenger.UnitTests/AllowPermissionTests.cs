using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Channel.AllowPermissionForRole;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class AllowPermissionTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            Func<Task> action = () => sut.SendAsync(
                new AllowPermissionForRoleCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "ReadMessages")
            );

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_channel_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateRole("role1").Build().Build()
                .Build();
            Func<Task> action = () => sut.SendAsync(
                new AllowPermissionForRoleCommand(group.OwnerId, group.GroupId, Guid.NewGuid(), group.Role(1).RoleId, "ReadMessages")
            );

            await action.Should().ThrowAsync<ChannelNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_channel_role_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build().Build()
                .Build();
            Func<Task> action = () => sut.SendAsync(
                new AllowPermissionForRoleCommand(group.OwnerId, group.GroupId, group.Channel(1).ChannelId, Guid.NewGuid(), "ReadMessages")
            );

            await action.Should().ThrowAsync<RoleNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_member_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build().Build()
                .Build();
            Func<Task> action = () => sut.SendAsync(
                new AllowPermissionForRoleCommand(Guid.NewGuid(), group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId, "ReadMessages")
            );

            await action.Should().ThrowAsync<MemberNotFoundException>();
        });

        [Fact]
        public async Task Member_without_correct_permissions_cannot_allow_permission() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateMember().Build()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build().Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(
                new AllowPermissionForRoleCommand(group.Member(1).UserId, group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId, "ReadMessages")
            );

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Owner_can_allow_permission() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateMember().Build()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build().Build()
                .Build();

            await sut.SendAsync(
                new AllowPermissionForRoleCommand(group.OwnerId, group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId, "ReadMessages")
            );

            var permissions = await sut.QueryAsync(new GetChannelRolePermissionOverridesQuery(group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId));
            permissions.Should().ContainEquivalentOf(
                new RolePermissionOverrideDto
                {
                    Type = OverrideTypeDto.Allow,
                    Permission = "ReadMessages"
                }
            );
        });

        [Fact]
        public async Task Member_with_administrator_permission_can_allow_permission_for_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("Administrator").Build()
                .CreateMember().AssignRole(1).Build()
                .CreateChannel("channel1").Build().Build()
                .Build();

            await sut.SendAsync(
                new AllowPermissionForRoleCommand(group.Member(1).UserId, group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId, "ReadMessages")
            );

            var permissions = await sut.QueryAsync(new GetChannelRolePermissionOverridesQuery(group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId));
            permissions.Should().ContainEquivalentOf(
                new RolePermissionOverrideDto
                {
                    Type = OverrideTypeDto.Allow,
                    Permission = "ReadMessages"
                }
            );
        });

        [Fact]
        public async Task Member_with_manage_channels_permission_cannot_allow_permission_for_role_when_his_manage_channel_permission_is_denied_within_that_channel() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageChannels").Build()
                .CreateMember().AssignRole(1).Build()
                .CreateChannel("channel1").DenyPermissionForRole(1, "ManageChannels").Build().Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(
                new AllowPermissionForRoleCommand(group.Member(1).UserId, group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId, "ReadMessages")
            );

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Role_specific_permission_override_is_applied_correctly() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageChannels").Build()
                .CreateRole("role2").AddPermission("ManageChannels").Build()
                .CreateMember().AssignRole(1).Build()
                .CreateMember().AssignRole(2).Build()
                .CreateChannel("channel1").DenyPermissionForRole(1, "ManageChannels").Build().Build()
                .Build();

            await sut.SendAsync(
                new AllowPermissionForRoleCommand(group.Member(2).UserId, group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId, "ReadMessages")
            );


            var permissions = await sut.QueryAsync(new GetChannelRolePermissionOverridesQuery(group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId));
            permissions.Should().ContainEquivalentOf(
                new RolePermissionOverrideDto
                {
                    Type = OverrideTypeDto.Allow,
                    Permission = "ReadMessages"
                }
            );
        });

        [Fact]
        public async Task Member_with_manage_channels_permission_can_allow_permission_for_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageChannels").Build()
                .CreateMember().AssignRole(1).Build()
                .CreateChannel("channel1").Build().Build()
                .Build();

            await sut.SendAsync(
                new AllowPermissionForRoleCommand(group.Member(1).UserId, group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId, "ReadMessages")
            );

            var permissions = await sut.QueryAsync(new GetChannelRolePermissionOverridesQuery(group.GroupId, group.Channel(1).ChannelId, group.Role(1).RoleId));
            permissions.Should().ContainEquivalentOf(
                new RolePermissionOverrideDto
                {
                    Type = OverrideTypeDto.Allow,
                    Permission = "ReadMessages"
                }
            );
        });

    }
}