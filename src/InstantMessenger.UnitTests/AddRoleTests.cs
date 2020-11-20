using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class AddRoleTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var command = new AddRoleCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "role1");

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task Fails_when_role_name_is_invalid(string roleName) => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1").AsOwner().CreateRole("role1").Build().Build().Build();

            var command = new AddRoleCommand(group.OwnerId, group.GroupId, group.Role(1).RoleId, roleName);

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<InvalidRoleNameException>();
        });

        [Fact]
        public async Task Fails_when_member_do_not_have_correct_permission() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1").AsOwner()
                .CreateMember().Build().Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new AddRoleCommand(group.Member(1).UserId, group.GroupId, Guid.NewGuid(), "role1"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Owner_can_add_new_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1").Build();

            await sut.SendAsync(new AddRoleCommand(group.OwnerId, group.GroupId, Guid.NewGuid(), "role1"));

            var roles = await sut.QueryAsync(new GetRolesQuery(group.OwnerId, group.GroupId));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
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
        public async Task Member_with_administrator_permission_can_add_new_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                    .CreateRole("role1").AddPermission("Administrator").Build()
                    .CreateMember().AssignRole(1).Build()
                .Build().Build();
            var role2Id = Guid.NewGuid();

            await sut.SendAsync(new AddRoleCommand(group.Member(1).UserId, group.GroupId, role2Id, "role2"));

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
                    x.RoleId.Should().Be(role2Id);
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

        [Fact]
        public async Task Member_with_manage_roles_permission_can_add_new_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageRoles").Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();
            var role2Id = Guid.NewGuid();

            await sut.SendAsync(new AddRoleCommand(group.Member(1).UserId, group.GroupId, role2Id, "role2"));

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
                    x.RoleId.Should().Be(role2Id);
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

        [Fact]
        public async Task Added_role_has_the_lowest_priority_after_the_everyone_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                    .CreateRole("role1").Build()
                    .CreateRole("role2").Build()
                .Build().Build();
            var addRole3 = new AddRoleCommand(group.OwnerId, group.GroupId, Guid.NewGuid(), "role3");

            await sut.SendAsync(addRole3);

            var roles = await sut.QueryAsync(new GetRolesQuery(group.OwnerId, group.GroupId));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(group.Role(1).RoleId);
                    x.Priority.Should().Be(2);
                    x.Name.Should().Be(group.Role(1).Name);
                },
                x =>
                {
                    x.RoleId.Should().Be(group.Role(2).RoleId);
                    x.Priority.Should().Be(1);
                    x.Name.Should().Be(group.Role(2).Name);
                },
                x =>
                {
                    x.RoleId.Should().Be(addRole3.RoleId);
                    x.Priority.Should().Be(0);
                    x.Name.Should().Be(addRole3.Name);
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