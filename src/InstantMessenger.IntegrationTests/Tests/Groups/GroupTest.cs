using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Members.RemoveRole;
using InstantMessenger.Groups.Api.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Api.Features.Roles.AddRole;
using InstantMessenger.IntegrationTests.Api;
using InstantMessenger.IntegrationTests.Common;
using Xunit;

namespace InstantMessenger.IntegrationTests.Tests.Groups
{
    [Collection("Server collection")]
    public class GroupTest : IClassFixture<ServerFixture>
    {
        private readonly ServerFixture _fixture;

        public GroupTest(ServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateGroup()
        {
            var user = await _fixture.LoginAsUser("user@test.com","user");
            var sut = _fixture.GetClient<IGroupsApi>();
            var createGroup = new CreateGroupApiRequest(Guid.NewGuid(), Guid.NewGuid(), "test");

            var result = await sut.CreateGroup(user.BearerToken(), createGroup);

            result.IsSuccessStatusCode.Should().BeTrue();
            var group = await sut.GetGroup(user.BearerToken(), createGroup.GroupId);
            group.GroupId.Should().Be(createGroup.GroupId);
            group.Name.Should().Be(createGroup.GroupName);
            group.CreatedAt.Should().NotBe(default);
            var owner = await sut.GetOwner(user.BearerToken(), group.GroupId);
            owner.Should().NotBeNull();
            owner.CreatedAt.Should().NotBe(default);
            owner.IsOwner.Should().BeTrue();
            owner.UserId.Should().Be(Guid.Parse(user.Subject));
            owner.MemberId.Should().NotBeEmpty();
            owner.Name.Should().Be("user");


            var createRole = new AddRoleApiRequest(createGroup.GroupId, Guid.NewGuid(), "role1");
            result = await sut.CreateRole(user.BearerToken(), createRole.GroupId, createRole);
            result.EnsureSuccessStatusCode();

            var addPermission = new AddPermissionToRoleApiRequest(createGroup.GroupId, createRole.RoleId, "Administrator");
            result = await sut.AddPermission(user.BearerToken(), addPermission.GroupId, addPermission.RoleId, addPermission);
            result.EnsureSuccessStatusCode();

            var assignRole = new AssignRoleToMemberApiRequest(createGroup.GroupId, user.UserId(), createRole.RoleId);
            result = await sut.AssignRole(user.BearerToken(), assignRole.GroupId, assignRole.MemberUserId, assignRole);
            result.EnsureSuccessStatusCode();


            var roles = await sut.GetRoles(user.BearerToken(), createRole.GroupId);
            roles.Should().NotBeNull();
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(createRole.RoleId);
                    x.Priority.Should().Be(0);
                    x.Name.Should().Be(createRole.Name);
                },
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );

            var ownerRoles = await sut.GetMemberRoles(user.BearerToken(), createRole.GroupId, user.UserId());
            ownerRoles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(createRole.RoleId);
                    x.Priority.Should().Be(0);
                    x.Name.Should().Be(createRole.Name);
                },
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );

            var permissions = await sut.GetRolePermissions(user.BearerToken(), createRole.GroupId, createRole.RoleId);
            permissions.Should().SatisfyRespectively(
                x =>
                {
                    x.Code.Should().Be(0x1);
                    x.Name.Should().Be("Administrator");
                }
            );

            result = await sut.RemovePermission(user.BearerToken(), createRole.GroupId, createRole.RoleId, "Administrator");
            result.EnsureSuccessStatusCode();

            result = await sut.RemoveRoleFromMember(user.BearerToken(), createRole.GroupId, user.UserId(), createRole.RoleId);
            result.EnsureSuccessStatusCode();

            roles = await sut.GetRoles(user.BearerToken(), createRole.GroupId);
            roles.Should().NotBeNull();
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().Be(createRole.RoleId);
                    x.Priority.Should().Be(0);
                    x.Name.Should().Be(createRole.Name);
                },
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );

            ownerRoles = await sut.GetMemberRoles(user.BearerToken(), createRole.GroupId, user.UserId());
            ownerRoles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );
            permissions = await sut.GetRolePermissions(user.BearerToken(), createRole.GroupId, createRole.RoleId);
            permissions.Should().BeNullOrEmpty();

            result = await sut.RemoveRole(user.BearerToken(), createRole.GroupId, createRole.RoleId);
            result.EnsureSuccessStatusCode();

            roles = await sut.GetRoles(user.BearerToken(), createRole.GroupId);
            roles.Should().NotBeNull();
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Priority.Should().Be(-1);
                    x.Name.Should().Be("@everyone");
                }
            );
        }
    }
}