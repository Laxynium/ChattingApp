using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Members.RemoveRole;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.UnitTests.Common;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class RemoveRoleFromMemberTests : GroupsModuleUnitTestBase
    {
        [Fact]
        public async Task Fails_when_group_is_missing() => await Run(async sut =>
        {
            var command = new RemoveRoleFromMemberCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid());

            Func<Task> action = async ()=>await sut.SendAsync(command);

            await action.Should().ThrowAsync<GroupNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_member_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1").AsOwner().CreateRole("role1").Build().Build().Build();

            Func<Task> action = async ()=>await sut.SendAsync(new RemoveRoleFromMemberCommand(group.OwnerId, group.GroupId, Guid.NewGuid(), group.Role(1).RoleId));

            await action.Should().ThrowAsync<MemberNotFoundException>();
        });

        [Fact]
        public async Task Fails_when_role_is_missing() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1").AsOwner().CreateMember().Build().Build().Build();

            Func<Task> action = async ()=>await sut.SendAsync(new RemoveRoleFromMemberCommand(group.OwnerId, group.GroupId, group.Member(1).UserId, Guid.NewGuid()));

            await action.Should().ThrowAsync<RoleNotFoundException>();
        });

        [Fact]
        public async Task Owner_can_remove_role_from_member() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            await sut.SendAsync(new RemoveRoleFromMemberCommand(group.OwnerId, group.GroupId, group.Member(1).UserId, group.Role(1).RoleId));

            var roles = await sut.QueryAsync(new GetMemberRolesQuery(group.OwnerId, group.GroupId, group.Member(1).UserId));
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
        public async Task Member_with_administrator_permission_cannot_remove_roles_with_higher_priority_than_his_highest_one_from_member() => await Run(
            async sut =>
            {
                var group = await GroupBuilder.For(sut)
                    .CreateGroup("group1")
                    .AsOwner()
                    .CreateRole("role1").Build()
                    .CreateRole("role2").Build()
                    .CreateRole("role3").AddPermission("Administrator").Build()
                    .CreateMember().AssignRole(2).AssignRole(3).Build()
                    .CreateMember().AssignRole(1).Build()
                    .Build().Build();

                Func<Task> action = async () => await sut.SendAsync(new RemoveRoleFromMemberCommand(group.Member(1).UserId, group.GroupId, group.Member(2).UserId, group.Role(1).RoleId));
            
                await action.Should().ThrowAsync<InsufficientPermissionsException>();
            });

        [Fact]
        public async Task Member_with_administrator_permission_cannot_remove_role_with_same_priority_as_his_highest_one_from_member() => await Run(
            async sut =>
            {
                var group = await GroupBuilder.For(sut)
                    .CreateGroup("group1")
                    .AsOwner()
                    .CreateRole("role1").Build()
                    .CreateRole("role2").AddPermission("Administrator").Build()
                    .CreateRole("role3").Build()
                    .CreateMember().AssignRole(2).AssignRole(3).Build()
                    .CreateMember().AssignRole(2).Build()
                    .Build().Build();

                Func<Task> action = async () => await sut.SendAsync(new RemoveRoleFromMemberCommand(group.Member(1).UserId, group.GroupId, group.Member(2).UserId, group.Role(2).RoleId));
            
                await action.Should().ThrowAsync<InsufficientPermissionsException>();
            });

        [Fact]
        public async Task Member_with_administrator_permission_cannot_remove_from_himself_role_with_highest_priority() => await Run(
            async sut =>
            {
                var group = await GroupBuilder.For(sut)
                    .CreateGroup("group1")
                    .AsOwner()
                    .CreateRole("role1").Build()
                    .CreateRole("role2").AddPermission("Administrator").Build()
                    .CreateRole("role3").Build()
                    .CreateMember().AssignRole(2).AssignRole(3).Build()
                    .Build().Build();

                Func<Task> action = async () => await sut.SendAsync(new RemoveRoleFromMemberCommand(group.Member(1).UserId, group.GroupId, group.Member(1).UserId, group.Role(2).RoleId));
            
                await action.Should().ThrowAsync<InsufficientPermissionsException>();
            });

        [Fact]
        public async Task Member_with_administrator_permission_can_remove_role_from_member() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("Administrator").Build()
                .CreateRole("role2").Build()
                .CreateMember().AssignRole(1).Build()
                .CreateMember().AssignRole(2).Build()
                .Build().Build();

            await sut.SendAsync(new RemoveRoleFromMemberCommand(group.Member(1).UserId, group.GroupId, group.Member(2).UserId, group.Role(2).RoleId));

            var roles = await sut.QueryAsync(new GetMemberRolesQuery(group.Member(1).UserId, group.GroupId, group.Member(2).UserId));
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
        public async Task Member_with_manage_roles_cannot_remove_role_with_higher_priority_than_his_highest_one_from_member() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateRole("role2").Build()
                .CreateRole("role3").AddPermission("ManageRoles").Build()
                .CreateMember().AssignRole(2).AssignRole(3).Build()
                .CreateMember().AssignRole(1).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemoveRoleFromMemberCommand(group.Member(1).UserId, group.GroupId, group.Member(2).UserId, group.Role(1).RoleId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });

        [Fact]
        public async Task Member_with_manage_roles_cannot_assign_roles_with_same_priority_as_his_highest_one() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").Build()
                .CreateRole("role2").AddPermission("ManageRoles").Build()
                .CreateRole("role3").Build()
                .CreateMember().AssignRole(2).AssignRole(3).Build()
                .CreateMember().AssignRole(2).Build()
                .Build().Build();

            Func<Task> action = async () => await sut.SendAsync(new RemoveRoleFromMemberCommand(group.Member(1).UserId, group.GroupId, group.Member(2).UserId, group.Role(2).RoleId));

            await action.Should().ThrowAsync<InsufficientPermissionsException>();
        });
        [Fact]
        public async Task Member_with_manage_roles_permission_can_assign_role_to_member() => await Run(async sut =>
        {
            var group = await GroupBuilder.For(sut)
                .CreateGroup("group1")
                .AsOwner()
                .CreateRole("role1").AddPermission("ManageRoles").Build()
                .CreateRole("role2").Build()
                .CreateMember().AssignRole(1).Build()
                .CreateMember().AssignRole(2).Build()
                .Build().Build();

            await sut.SendAsync(new RemoveRoleFromMemberCommand(group.Member(1).UserId, group.GroupId, group.Member(2).UserId, group.Role(2).RoleId));

            var roles = await sut.QueryAsync(new GetMemberRolesQuery(group.Member(1).UserId, group.GroupId, group.Member(2).UserId));
            roles.Should().SatisfyRespectively(
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