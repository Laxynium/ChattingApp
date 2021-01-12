using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Application.Features.Roles.RemoveRole;
using InstantMessenger.Groups.Application.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.UnitTests.Common;
using Xunit;

namespace InstantMessenger.Groups.UnitTests
{
    public class RemoveRoleTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_do_not_exists() => await Run(async sut =>
        {
            var command = new RemoveRoleCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_role_do_not_exists() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1").Build();
            var command = new RemoveRoleCommand(group.OwnerId, group.GroupId, Guid.NewGuid());

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<RoleNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_member_do_not_have_correct_permission() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateMember().Build()
                .CreateRole("role1").Build().Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemoveRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
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

        [Fact]
        public async Task Role_is_also_removed_from_members() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateRole("role1").Build()
                .CreateMember().AssignRole(1).Build()
                .CreateMember().AssignRole(1).Build()
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

            var member1Roles = await sut.QueryAsync(new GetMemberRolesQuery(group.OwnerId, group.GroupId, group.Member(1).UserId));
            var member2Roles = await sut.QueryAsync(new GetMemberRolesQuery(group.OwnerId, group.GroupId, group.Member(1).UserId));
            member1Roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );
            member2Roles.Should().SatisfyRespectively(
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

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
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

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
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

        [Fact]
        public async Task Everyone_role_cannot_be_removed() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .Build();
            var everyoneRole = (await sut.QueryAsync(new GetRolesQuery(group.OwnerId, group.GroupId))).First();
            

            Func<Task> action = ()=>sut.SendAsync(new RemoveRoleCommand(group.OwnerId, group.GroupId, everyoneRole.RoleId));

            await action.Should().ThrowAsync<EveryoneRoleCannotBeRemovedException>();
        });
    }
}