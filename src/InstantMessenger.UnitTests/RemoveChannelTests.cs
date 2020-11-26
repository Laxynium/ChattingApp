using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Channel.RemoveChannel;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class RemoveChannelTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner().CreateChannel("channel1").Build()
                .Build().Build();

            Func<Task> action = () => sut.SendAsync(
                new RemoveChannelCommand(group.OwnerId, Guid.NewGuid(), group.Channel(1).ChannelId));

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_user_is_missing_in_group() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner().CreateChannel("channel1").Build()
                .Build().Build();

            Func<Task> action = () => sut.SendAsync(
                new RemoveChannelCommand(Guid.NewGuid(), group.GroupId, group.Channel(1).ChannelId));

            await action.Should().ThrowAsync<MemberNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_channel_is_missing_in_group() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner()
                .CreateChannel("channel1").Build()
                .Build().Build();

            Func<Task> action = () => sut.SendAsync(
                new RemoveChannelCommand(group.OwnerId, group.GroupId, Guid.NewGuid()));

            await action.Should().ThrowAsync<ChannelNotFoundException>();
        });

        [Fact]
        public async Task Member_without_valid_permission_cannot_remove_channel() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateChannel("channel1").Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = () => sut.SendAsync(
                new RemoveChannelCommand(group.Member(1).UserId, group.GroupId, group.Channel(1).ChannelId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Owner_can_create_channel() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner()
                .CreateChannel("channel1").Build()
                .Build().Build();

            await sut.SendAsync(new RemoveChannelCommand(group.OwnerId, group.GroupId, group.Channel(1).ChannelId));

            var channels = await sut.QueryAsync(new GetGroupChannelsQuery(group.OwnerId, group.GroupId));
            channels.Should().BeNullOrEmpty();
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("ManageChannels")]
        public async Task Member_with_valid_permissions_can_create_channel(string validPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission(validPermission).Build()
                .CreateMember().AssignRole(1).Build()
                .CreateChannel("channel1").Build()
                .Build().Build();

            await sut.SendAsync(new RemoveChannelCommand(group.Member(1).UserId, group.GroupId, group.Channel(1).ChannelId));

            var channels = await sut.QueryAsync(new GetGroupChannelsQuery(group.OwnerId, group.GroupId));
            channels.Should().BeNullOrEmpty();
        });
    }
}