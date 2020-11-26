using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Channel.AddChannel;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Domain.Entities;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class CreateChannelTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .Build();

            Func<Task> action = ()=> sut.SendAsync(new CreateChannelCommand(group.OwnerId, Guid.NewGuid(), Guid.NewGuid(), "channel1"));

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_user_is_missing_in_group() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .Build();

            Func<Task> action = ()=> sut.SendAsync(new CreateChannelCommand(Guid.NewGuid(), group.GroupId, Guid.NewGuid(), "channel1"));

            await action.Should().ThrowAsync<MemberNotFoundException>();
        });

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task Fails_when_channel_name_is_empty(string emptyName) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .Build();

            Func<Task> action = ()=> sut.SendAsync(new CreateChannelCommand(group.OwnerId, group.GroupId, Guid.NewGuid(), emptyName));

            await action.Should().ThrowAsync<InvalidChannelNameException>();
        });

        [Fact]
        public async Task Member_without_valid_permission_cannot_create_channel() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner()
                    .CreateRole("role1").Build()
                    .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = () => sut.SendAsync(new CreateChannelCommand(group.Member(1).UserId, group.GroupId, Guid.NewGuid(), "channel1"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Owner_can_create_channel() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .Build();
            var createChannel = new CreateChannelCommand(group.OwnerId, group.GroupId, Guid.NewGuid(), "channel1");

            await sut.SendAsync(createChannel);

            var channels = await sut.QueryAsync(new GetGroupChannelsQuery(group.OwnerId, group.GroupId));
            channels.Should().ContainEquivalentOf(new ChannelDto
            {
                GroupId = group.GroupId,
                ChannelId = createChannel.ChannelId,
                ChannelName = createChannel.ChannelName
            });
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("ManageChannels")]
        public async Task Member_with_valid_permissions_can_create_channel(string validPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner().CreateRole("role1").AddPermission(validPermission).Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();
            var createChannel = new CreateChannelCommand(group.OwnerId, group.GroupId, Guid.NewGuid(), "channel1");

            await sut.SendAsync(createChannel);

            var channels = await sut.QueryAsync(new GetGroupChannelsQuery(group.OwnerId, group.GroupId));
            channels.Should().ContainEquivalentOf(new ChannelDto
            {
                GroupId = group.GroupId,
                ChannelId = createChannel.ChannelId,
                ChannelName = createChannel.ChannelName
            });
        });
    }
}