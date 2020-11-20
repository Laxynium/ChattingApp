using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class AddPermissionToRoleTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var (userId, groupId, roleId) = (Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(userId, groupId, roleId, "Administrator"));

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_role_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1").Build();

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(group.OwnerId, group.GroupId, Guid.NewGuid(), "Administrator"));

            await action.Should().ThrowAsync<RoleNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_permission_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1").AsOwner().CreateRole("role1").Build().Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(group.OwnerId, group.GroupId, group.Role(1).RoleId, "SomeMissingPermission"));

            await action.Should().ThrowAsync<PermissionNotFoundException>();
        });

        [Fact]
        public async Task Member_without_correct_permissions_cannot_add_permission_to_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                    .AsOwner()
                        .CreateRole("role1").Build()
                        .CreateRole("role2").Build()
                        .CreateMember().AssignRole(1).Build()
                    .Build()
                .Build();


            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(2).RoleId, "Administrator"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Owner_can_add_permission_to_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateRole("role1").Build().Build()
                .Build();

            await sut.SendAsync(new AddPermissionToRoleCommand(group.OwnerId, group.GroupId, group.Role(1).RoleId,"Administrator"));

            var permissions = await sut.QueryAsync(new GetRolePermissionsQuery(group.GroupId, group.Role(1).RoleId));
            permissions.Should().SatisfyRespectively(
                x =>
                {
                    x.Name.Should().Be("Administrator");
                    x.Code.Should().Be(0x1);
                }
            );
        });

        [Fact]
        public async Task Member_with_administrator_permission_cannot_add_permission_to_role_with_higher_priority() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                    .CreateRole("role1").Build()
                    .CreateRole("role2").AddPermission("Administrator").Build()
                    .CreateMember().AssignRole(2).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId, "ManageRoles"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_administrator_permission_cannot_add_permission_to_role_with_same_priority() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                    .CreateRole("role1").AddPermission("Administrator").Build()
                    .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId, "ManageRoles"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_administrator_permission_can_add_permission_to_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                    .CreateRole("role1").AddPermission("Administrator").Build()
                    .CreateRole("role2").Build()
                    .CreateMember().AssignRole(1).Build()
                .Build().Build();

            await sut.SendAsync(new AddPermissionToRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(2).RoleId,"ManageRoles"));

            var permissions = await sut.QueryAsync(new GetRolePermissionsQuery(group.GroupId, group.Role(2).RoleId));
            permissions.Should().SatisfyRespectively(
                x =>
                {
                    x.Name.Should().Be("ManageRoles");
                    x.Code.Should().Be(0x4);
                }
            );
        });


        [Fact]
        public async Task Member_with_manage_roles_permission_cannot_add_permission_to_role_with_higher_priority() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateRole("role1").Build()
                .CreateRole("Role2").AddPermission("ManageRoles").Build()
                .CreateMember().AssignRole(2).Build().Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId, "ManageRoles"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_manage_roles_permission_cannot_add_permission_to_role_with_same_priority() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner().CreateRole("role1").AddPermission("ManageRoles").Build()
                .CreateMember().AssignRole(1).Build().Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(1).RoleId, "ManageRoles"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_manage_roles_permission_cannot_add_permission_to_role_which_he_do_not_possess() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                    .CreateRole("role1").AddPermission("ManageRoles").Build()
                    .CreateRole("role2").Build()
                .CreateMember().AssignRole(1).Build().Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new AddPermissionToRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(2).RoleId, "ManageGroup"));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_manage_roles_permission_can_add_permission_to_role() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut).CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageRoles").Build()
                .CreateRole("role2").Build()
                .CreateMember().AssignRole(1).Build().Build().Build();

            await sut.SendAsync(new AddPermissionToRoleCommand(group.Member(1).UserId, group.GroupId, group.Role(2).RoleId, "ManageRoles"));

            var permissions = await sut.QueryAsync(new GetRolePermissionsQuery(group.GroupId, group.Role(2).RoleId));
            permissions.Should().SatisfyRespectively(
                x =>
                {
                    x.Name.Should().Be("ManageRoles");
                    x.Code.Should().Be(0x4);
                }
            );
        });
    }
}