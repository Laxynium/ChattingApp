using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Roles.RemoveRole;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class RemoveRoleTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_role_do_not_exists() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1").Build();
            var command = new RemoveRoleCommand(group.OwnerId, group.GroupId, Guid.NewGuid());

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Fails_when_member_do_not_have_correct_permission() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateMember().Build()
                .CreateRole("role1").Build().Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemoveRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId));

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Owner_can_remove_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateRole("role1").Build()
                .Build().Build();

            await sut.SendAsync(new RemoveRoleCommand(group.OwnerId, group.GroupId, group.Role(1).RoleId));

            var roles = await sut.QueryAsync(new GetRolesQuery(group.OwnerId, group.GroupId));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("ManageRoles")]
        public async Task Member_with_correct_permission_cannot_remove_role_with_higher_priority_than_his_highest_one(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateRole("role2").AddPermission(correctPermission).Build()
                .CreateMember().AssignRole(2).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemoveRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId));

            await action.Should().ThrowAsync<Exception>();
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("ManageRoles")]
        public async Task Member_with_correct_permission_cannot_remove_role_with_same_priority_as_his_highest_one(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission(correctPermission).Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemoveRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId));

            await action.Should().ThrowAsync<Exception>();
        });


        [Theory]
        [InlineData("Administrator")]
        [InlineData("ManageRoles")]
        public async Task Member_with_administrator_permission_can_remove_role(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission(correctPermission).Build()
                .CreateRole("role2").Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            await sut.SendAsync(new RemoveRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(2).RoleId));

            var roles = await sut.QueryAsync(new GetRolesQuery(group.OwnerId, group.GroupId));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(group.Role(1).RoleId);
                    x.Priority.Should().Be(1);
                    x.Name.Should().Be("role1");
                },
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );
        });
    }
}