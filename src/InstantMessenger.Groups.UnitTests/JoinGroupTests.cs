using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Application.Features.Invitations.GenerateInvitationCode;
using InstantMessenger.Groups.Application.Features.Invitations.JoinGroup;
using InstantMessenger.Groups.Application.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.UnitTests.Common;
using InstantMessenger.Shared.Modules;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
using NodaTime.Testing;
using Xunit;

namespace InstantMessenger.Groups.UnitTests
{
    public class JoinGroupTests : GroupsModuleUnitTestBase
    {
        private readonly FakeClock _clock;
        private readonly Dictionary<Guid, string> _userIdToNickname = new Dictionary<Guid, string>();
        public JoinGroupTests()
        {
            _clock = new FakeClock(Instant.FromDateTimeOffset(new DateTimeOffset(new DateTime(2020,01,01,12,00,00))));
            Configure(
                sc => sc
                    .Remove<IModuleClient>()
                    .AddSingleton<IModuleClient>(x => new FakeModuleClient(userIdToNickname: _userIdToNickname))
                    .Remove<IClock>()
                    .AddSingleton<IClock>(_clock)
            );
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        [InlineData("12abAB")]
        [InlineData("12abAB12")]
        [InlineData("12abAB123")]
        public async Task Fails_when_code_is_invalid(string invalidCode) => await Run(
        async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateInvitation(
                    new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Infinite),
                    new UsageCounterCommandItem(UsageCounterTypeCommandItem.Infinite)).Build()
                .Build();
            var newMemberUserId = Guid.NewGuid();
            _userIdToNickname.Add(newMemberUserId, "user1");

            Func<Task> action = async () => await sut.SendAsync(new JoinGroupCommand(newMemberUserId, invalidCode));

            await action.Should().ThrowAsync<InvalidInvitationCodeException>();
        });

        [Fact]
        public async Task Fails_when_code_is_expired() => await Run(
        async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateInvitation(
                    new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Bounded,TimeSpan.FromMinutes(1)),
                    new UsageCounterCommandItem(UsageCounterTypeCommandItem.Infinite)).Build()
                .Build();
            var newMemberUserId = Guid.NewGuid();
            _userIdToNickname.Add(newMemberUserId, "user1");
            _clock.AdvanceMinutes(2);

            Func<Task> action = async () => await sut.SendAsync(new JoinGroupCommand(newMemberUserId, group.Invitation(1).Code));

            await action.Should().ThrowAsync<InvalidInvitationException>();
        });

        [Fact]
        public async Task Fails_when_invitation_usage_counter_is_exceeded() => await Run(
        async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateInvitation(
                    new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Infinite),
                    new UsageCounterCommandItem(UsageCounterTypeCommandItem.Bounded,1)).Build()
                .Build();
            var (memberUserId1,memberUserId2) = (Guid.NewGuid(),Guid.NewGuid());
            _userIdToNickname.Add(memberUserId1, "user1");
            await sut.SendAsync(new JoinGroupCommand(memberUserId1, group.Invitation(1).Code));

            Func<Task> action = async () => await sut.SendAsync(new JoinGroupCommand(memberUserId2, group.Invitation(1).Code));

            await action.Should().ThrowAsync<InvalidInvitationException>();
        });

        [Fact]
        public async Task Fails_when_user_is_already_a_member() => await Run(
        async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateInvitation(
                    new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Infinite),
                    new UsageCounterCommandItem(UsageCounterTypeCommandItem.Infinite)).Build()
                .Build();
            var memberUserId1 = Guid.NewGuid();
            _userIdToNickname.Add(memberUserId1, "user1");
            await sut.SendAsync(new JoinGroupCommand(memberUserId1, group.Invitation(1).Code));

            Func<Task> action = async () => await sut.SendAsync(new JoinGroupCommand(memberUserId1, group.Invitation(1).Code));

            await action.Should().ThrowAsync<InvalidInvitationException>();
        });

        [Fact]
        public async Task User_can_join_group_with_valid_code() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateInvitation(
                    new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Infinite), 
                    new UsageCounterCommandItem(UsageCounterTypeCommandItem.Infinite)).Build()
                .Build();
            var newMemberUserId = Guid.NewGuid();
            _userIdToNickname.Add(newMemberUserId,"user1");

            await sut.SendAsync(new JoinGroupCommand(newMemberUserId, group.Invitation(1).Code));

            var members = await sut.QueryAsync(new GetMembersQuery(group.OwnerId,group.GroupId));
            var newMember = members.FirstOrDefault(x => x.UserId == newMemberUserId);
            newMember.Should().NotBeNull();
            newMember.CreatedAt.Should().NotBe(default);
            newMember.IsOwner.Should().BeFalse();
            newMember.Name.Should().Be("user1");
            newMember.UserId.Should().Be(newMemberUserId);
        });
    }
}