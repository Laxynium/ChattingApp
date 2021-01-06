using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Application.Features.Members.LeaveGroup;
using InstantMessenger.Groups.Application.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class LeaveGroupTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            Func<Task> action = async () => await sut.SendAsync(new LeaveGroupCommand(Guid.NewGuid(), Guid.NewGuid()));

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_members_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .Build();

            Func<Task> action = async () => await sut.SendAsync(new LeaveGroupCommand(Guid.NewGuid(), group.GroupId));

            await action.Should().ThrowAsync<MemberNotFoundException>();
        });

        [Fact]
        public async Task Owner_cannot_leave_group() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1").Build();

            Func<Task> action = async () => await sut.SendAsync(new LeaveGroupCommand(group.OwnerId, group.GroupId));

            await action.Should().ThrowAsync<OwnerCannotLeaveGroupException>();
        });

        [Fact]
        public async Task Member_can_leave_a_group() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateMember().Build()
                .Build().Build();

            await sut.SendAsync(new LeaveGroupCommand(group.Member(1).UserId, group.GroupId));

            var members = await sut.QueryAsync(new GetMembersQuery(group.OwnerId,group.GroupId, false, group.Member(1).UserId));
            members.Should().BeNullOrEmpty();
        });
    }
}