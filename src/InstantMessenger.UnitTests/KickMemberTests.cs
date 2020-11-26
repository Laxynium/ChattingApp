using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Members.Kick;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class KickMemberTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            Func<Task> action = async () => await sut.SendAsync(new KickMemberCommand(Guid.NewGuid(), Guid.NewGuid(),Guid.NewGuid()));

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_executing_member_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateMember().Build().Build()
                .Build();

            Func<Task> action = async () => await sut.SendAsync(new KickMemberCommand(Guid.NewGuid(), group.GroupId, group.Member(1).UserId));

            await action.Should().ThrowAsync<MemberNotFoundException>();
        });
        [Fact]

        public async Task Fails_when_kicked_member_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .Build();

            Func<Task> action = async () => await sut.SendAsync(new KickMemberCommand(group.OwnerId, group.GroupId, Guid.NewGuid()));

            await action.Should().ThrowAsync<MemberNotFoundException>();
        });

        [Fact]
        public async Task Member_without_correct_permissions_cannot_kick_member() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateMember().AssignRole(1).Build()
                .CreateMember().Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new KickMemberCommand(group.Member(1).UserId, group.GroupId, group.Member(2).UserId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Owner_cannot_kick_himself() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .Build();

            Func<Task> action = async () => await sut.SendAsync(new KickMemberCommand(group.OwnerId, group.GroupId, group.OwnerId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });


        [Fact]
        public async Task Owner_can_kick_member() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateMember().Build().Build()
                .Build();

            await sut.SendAsync(new KickMemberCommand(group.OwnerId, group.GroupId, group.Member(1).UserId));

            var members = await sut.QueryAsync(new GetMembersQuery(group.GroupId, false, group.Member(1).UserId));
            members.Should().BeNullOrEmpty();
        });


        [Theory]
        [InlineData("Administrator")]
        [InlineData("Kick")]
        public async Task Member_with_correct_permissions_cannot_kick_owner(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission(correctPermission).Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new KickMemberCommand(group.Member(1).UserId, group.GroupId, group.OwnerId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("Kick")]
        public async Task Member_with_correct_permissions_cannot_kick_members_with_roles_higher_in_hierarchy(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateRole("role2").AddPermission(correctPermission).Build()
                .CreateMember().AssignRole(1).Build()
                .CreateMember().AssignRole(2).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new KickMemberCommand(group.Member(2).UserId, group.GroupId, group.Member(1).UserId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("Kick")]
        public async Task Member_with_correct_permissions_cannot_kick_members_on_the_same_level_in_hierarchy(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission(correctPermission).Build()
                .CreateMember().AssignRole(1).Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new KickMemberCommand(group.Member(2).UserId, group.GroupId, group.Member(1).UserId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("Kick")]
        public async Task Member_with_correct_permissions_cannot_kick_himself(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission(correctPermission).Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new KickMemberCommand(group.Member(1).UserId, group.GroupId, group.Member(1).UserId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });


        [Theory]
        [InlineData("Administrator")]
        [InlineData("Kick")]
        public async Task Member_with_correct_permissions_can_kick_member(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission(correctPermission).Build()
                .CreateMember().AssignRole(1).Build()
                .CreateMember().Build()
                .Build().Build();

            await sut.SendAsync(new KickMemberCommand(group.Member(1).UserId, group.GroupId,group.Member(2).UserId));

            var members = await sut.QueryAsync(new GetMembersQuery(group.GroupId, false, group.Member(2).UserId));
            members.Should().BeNullOrEmpty();
        });
    }
}