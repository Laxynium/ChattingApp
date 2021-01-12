using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Application.Features.Roles.MoveUpRoleInHierarchy;
using InstantMessenger.Groups.Application.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Groups.UnitTests.Common;
using Xunit;

namespace InstantMessenger.Groups.UnitTests
{
    public class MoveUpRolesInHierarchyTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var command = new MoveUpRoleInHierarchyCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_role_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1").AsOwner().Build().Build();

            var command = new MoveUpRoleInHierarchyCommand(group.OwnerId, group.GroupId, Guid.NewGuid());

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<RoleNotFoundException>();
        });

        [Fact]
        public async Task Everyone_role_cannot_be_moved_up() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().Build().Build();
            var everyoneRole = (await sut.QueryAsync(new GetRolesQuery(group.OwnerId, group.GroupId))).First();

            await sut.SendAsync(new MoveUpRoleInHierarchyCommand(group.OwnerId, group.GroupId, everyoneRole.RoleId));

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
        public async Task The_highest_user_defined_role_cannot_be_moved_up_higher() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build().Build().Build();

            await sut.SendAsync(new MoveUpRoleInHierarchyCommand(group.OwnerId, group.GroupId, group.Role(1).RoleId));

            var roles = await sut.QueryAsync(new GetRolesQuery(group.OwnerId, group.GroupId));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(group.Role(1).RoleId);
                    x.Priority.Should().Be(0);
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
        public async Task Member_without_correct_permissions_cannot_move_up_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateRole("role2").Build()
                .CreateMember().Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new MoveUpRoleInHierarchyCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Owner_can_move_up_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateRole("role2").Build()
                .CreateRole("role3").Build()
                .Build().Build();

            await sut.SendAsync(new MoveUpRoleInHierarchyCommand(group.OwnerId, group.GroupId, group.Role(2).RoleId));

            var roles = await sut.QueryAsync(new GetRolesQuery(group.OwnerId, group.GroupId));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(group.Role(2).RoleId);
                    x.Priority.Should().Be(2);
                    x.Name.Should().Be("role2");
                },
                x =>
                {
                    x.RoleId.Should().Be(group.Role(1).RoleId);
                    x.Priority.Should().Be(1);
                    x.Name.Should().Be("role1");
                },
                x =>
                {
                    x.RoleId.Should().Be(group.Role(3).RoleId);
                    x.Priority.Should().Be(0);
                    x.Name.Should().Be("role3");
                },
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
        public async Task Member_with_correct_permission_cannot_move_up_role_with_higher_priority_than_his_highest_one(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateRole("role2").Build()
                .CreateRole("role3").AddPermission(correctPermission).Build()
                .CreateMember().AssignRole(3).Build()
                .Build().Build();

            Func<Task> action = async()=> await sut.SendAsync(new MoveUpRoleInHierarchyCommand(group.Member(1).UserId, group.GroupId, group.Role(2).RoleId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("ManageRoles")]
        public async Task Member_with_correct_permission_cannot_move_up_role_with_same_priority_as_his_highest_one(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateRole("role2").AddPermission(correctPermission).Build()
                .CreateMember().AssignRole(2).Build()
                .Build().Build();

            Func<Task> action = async()=> await sut.SendAsync(new MoveUpRoleInHierarchyCommand(group.Member(1).UserId, group.GroupId, group.Role(2).RoleId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Theory]
        [InlineData("Administrator")]
        [InlineData("ManageRoles")]
        public async Task Member_with_correct_permission_can_move_up_role(string correctPermission) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission(correctPermission).Build()
                .CreateRole("role2").Build()
                .CreateRole("role3").Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            await sut.SendAsync(new MoveUpRoleInHierarchyCommand(group.Member(1).UserId, group.GroupId, group.Role(3).RoleId));

            var roles = await sut.QueryAsync(new GetRolesQuery(group.OwnerId, group.GroupId));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(group.Role(1).RoleId);
                    x.Priority.Should().Be(2);
                    x.Name.Should().Be("role1");
                },
                x =>
                {
                    x.RoleId.Should().Be(group.Role(3).RoleId);
                    x.Priority.Should().Be(1);
                    x.Name.Should().Be("role3");
                },
                x =>
                {
                    x.RoleId.Should().Be(group.Role(2).RoleId);
                    x.Priority.Should().Be(0);
                    x.Name.Should().Be("role2");
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