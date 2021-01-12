using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Application.Features.Channel.DenyPermissionForRole;
using InstantMessenger.Groups.Application.Queries;
using InstantMessenger.Groups.Application.ResponseDtos;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.UnitTests.Common;
using Xunit;

namespace InstantMessenger.Groups.UnitTests.PermissionOverrides
{
    public class DenyPermissionForRoleTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build()
                .Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(new DenyPermissionForRoleCommand(@group.OwnerId, Guid.NewGuid(), @group.Channel(1).ChannelId, @group.Role(1).RoleId, "SendMessages"));

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_member_is_missing_in_given_group() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build()
                .Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(new DenyPermissionForRoleCommand(Guid.NewGuid(), @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, "SendMessages"));

            await action.Should().ThrowAsync<MemberNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_channel_is_missing_in_given_group() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build()
                .Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(new DenyPermissionForRoleCommand(@group.OwnerId, @group.GroupId, Guid.NewGuid(), @group.Role(1).RoleId, "SendMessages"));

            await action.Should().ThrowAsync<ChannelNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_role_is_missing_in_given_group() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build()
                .Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(new DenyPermissionForRoleCommand(@group.OwnerId, @group.GroupId, @group.Channel(1).ChannelId, Guid.NewGuid(), "SendMessages"));

            await action.Should().ThrowAsync<RoleNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_permission_is_invalid() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build()
                .Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(new DenyPermissionForRoleCommand(@group.OwnerId, @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, "InvalidPermission"));

            await action.Should().ThrowAsync<PermissionNotFoundException>();
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("ManageGroup")]
        [InlineData("Kick")]
        //[InlineData("Ban")]
        [InlineData("ManageInvitations")]
        //[InlineData("ChangeNickname")]
        //[InlineData("ManageNicknames")]
        public async Task Fails_when_permission_cannot_be_overriden(string deniedPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build()
                .Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(new DenyPermissionForRoleCommand(@group.OwnerId, @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, deniedPermission));

            await action.Should().ThrowAsync<InvalidPermissionOverride>();
        });

        [Theory]
        [InlineData("ManageRoles")]
        [InlineData("ManageChannels")]
        [InlineData("ReadMessages")]
        [InlineData("SendMessages")]
        //[InlineData("AttachFiles")]
        //[InlineData("EmbedLinks")]
        //[InlineData("MentionRoles")]
        //[InlineData("AddReactions")]
        public async Task success_when_permission_can_be_overriden(string allowedPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build()
                .Build()
                .Build();

            await sut.SendAsync(new DenyPermissionForRoleCommand(@group.OwnerId, @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, allowedPermission));

            var overrides = await sut.QueryAsync(
                new GetChannelRolePermissionOverridesQuery(@group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId)
            );
            overrides.Should().ContainEquivalentOf(
                new PermissionOverrideDto
                {
                    Type = OverrideTypeDto.Deny,
                    Permission = allowedPermission
                }
            );
        });

        [Fact]
        public async Task Fails_when_member_has_no_valid_permission() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build()
                .CreateMember().AssignRole(1).Build()
                .Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(new DenyPermissionForRoleCommand(@group.Member(1).UserId, @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, "ReadMessages"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Success_when_member_is_owner() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build()
                .CreateMember().AssignRole(1).Build()
                .Build()
                .Build();

            await sut.SendAsync(new DenyPermissionForRoleCommand(@group.OwnerId, @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, "ReadMessages"));

            var overrides = await sut.QueryAsync(
                new GetChannelRolePermissionOverridesQuery(@group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId)
            );
            overrides.Should().ContainEquivalentOf(
                new PermissionOverrideDto
                {
                    Type = OverrideTypeDto.Deny,
                    Permission = "ReadMessages"
                }
            );
        });

        [Fact]
        public async Task Success_when_member_has_administrator_permission() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("Administrator").Build()
                .CreateChannel("channel1").Build()
                .CreateMember().AssignRole(1).Build()
                .Build()
                .Build();

            await sut.SendAsync(new DenyPermissionForRoleCommand(@group.Member(1).UserId, @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, "ReadMessages"));

            var overrides = await sut.QueryAsync(
                new GetChannelRolePermissionOverridesQuery(@group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId)
            );
            overrides.Should().ContainEquivalentOf(
                new PermissionOverrideDto
                {
                    Type = OverrideTypeDto.Deny,
                    Permission = "ReadMessages"
                }
            );
        });

        [Fact]
        public async Task Fails_when_member_with_manage_roles_permission_tries_to_override_permission_which_he_do_not_possess() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageRoles").Build()
                .CreateChannel("channel1").Build()
                .CreateMember().AssignRole(1).Build()
                .Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(new DenyPermissionForRoleCommand(@group.Member(1).UserId, @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, "SendMessages"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Fails_when_member_with_manage_roles_permission_tries_to_override_manage_roles_permissions() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageRoles").Build()
                .CreateChannel("channel1").Build()
                .CreateMember().AssignRole(1).Build()
                .Build()
                .Build();

            Func<Task> action = () => sut.SendAsync(new DenyPermissionForRoleCommand(@group.Member(1).UserId, @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, "ManageRoles"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_without_manage_roles_permission_but_with_allow_manage_roles_override_on_role_in_given_channel_can_change_all_permissions_in_channel() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").AllowPermissionForRole(1, "ManageRoles").Build()
                .CreateMember().AssignRole(1).Build()
                .Build()
                .Build();

            await sut.SendAsync(new DenyPermissionForRoleCommand(@group.Member(1).UserId, @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, "SendMessages"));

            var overrides = await sut.QueryAsync(
                new GetChannelRolePermissionOverridesQuery(@group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId)
            );
            overrides.Should().ContainEquivalentOf(
                new PermissionOverrideDto
                {
                    Type = OverrideTypeDto.Deny,
                    Permission = "SendMessages"
                }
            );
        });

        [Fact]
        public async Task Success_when_member_has_manage_roles_permission() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageRoles").AddPermission("SendMessages").Build()
                .CreateChannel("channel1").Build()
                .CreateMember().AssignRole(1).Build()
                .Build()
                .Build();

            await sut.SendAsync(new DenyPermissionForRoleCommand(@group.Member(1).UserId, @group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId, "SendMessages"));

            var overrides = await sut.QueryAsync(
                new GetChannelRolePermissionOverridesQuery(@group.GroupId, @group.Channel(1).ChannelId, @group.Role(1).RoleId)
            );
            overrides.Should().ContainEquivalentOf(
                new PermissionOverrideDto
                {
                    Type = OverrideTypeDto.Deny,
                    Permission = "SendMessages"
                }
            );
        });
    }
}