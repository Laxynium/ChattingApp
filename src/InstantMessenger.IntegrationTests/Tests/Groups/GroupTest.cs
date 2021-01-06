using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Application.Features.Group.Create;
using InstantMessenger.Groups.Application.Features.Invitations.GenerateInvitationCode;
using InstantMessenger.Groups.Application.Features.Members.AssignRole;
using InstantMessenger.Groups.Application.Features.Roles.AddPermissionToRole;
using InstantMessenger.Groups.Application.Features.Roles.AddRole;
using InstantMessenger.Groups.Application.ResponseDtos;
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
            var user2 = await _fixture.LoginAsUser("user2@test.com","user2");
            var sut = _fixture.GetClient<IGroupsApi>();
            var createGroup = new CreateGroupApiRequest(Guid.NewGuid(), "test");

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
            owner.UserId.Should().Be(user.UserId);
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

            var generateInvitation = new GenerateInvitationApiRequest(
                createGroup.GroupId,
                Guid.NewGuid(),
                new ExpirationTimeCommandItem(ExpirationTimeTypeCommandItem.Infinite),
                new UsageCounterCommandItem(UsageCounterTypeCommandItem.Infinite)
            );
            await sut.GenerateInvitation(
                user.BearerToken(),
                generateInvitation.GroupId,
                generateInvitation
            );

            var invitation = await sut.GetInvitation(user.BearerToken(), generateInvitation.GroupId, generateInvitation.InvitationId);
            invitation.Should().NotBeNull();
            invitation.InvitationId.Should().Be(generateInvitation.InvitationId);
            invitation.GroupId.Should().Be(generateInvitation.GroupId);
            invitation.Code.Should().MatchRegex("[a-zA-Z0-9]{8,8}");
            invitation.ExpirationTime.Should().NotBeNull();
            invitation.ExpirationTime.Period.Should().BeNull();
            invitation.ExpirationTime.Start.Should().NotBe(default);
            invitation.ExpirationTime.Type.Should().Be(ExpirationTimeTypeDto.Infinite);
            invitation.UsageCounter.Should().NotBeNull();
            invitation.UsageCounter.Value.Should().BeNull();
            invitation.UsageCounter.Type.Should().Be(UsageCounterTypeDto.Infinite);

            await sut.JoinGroup(user2.BearerToken(), invitation.Code);

            var member = await sut.GetMember(user.BearerToken(), group.GroupId, user2.UserId());
            member.Should().NotBeNull();
            member.CreatedAt.Should().NotBe(default);
            member.IsOwner.Should().BeFalse();
            member.Name.Should().Be("user2");
            member.UserId.Should().Be(user2.UserId());

            await sut.AssignRole(
                user.BearerToken(),
                group.GroupId,
                user2.UserId(),
                new AssignRoleToMemberApiRequest(group.GroupId, user2.UserId(), createRole.RoleId)
            );

            var user2Roles = await sut.GetMemberRoles(user.BearerToken(), group.GroupId, user2.UserId());
            user2Roles.Should().SatisfyRespectively(
                x =>
                {
                    x.Name.Should().Be("role1");
                    x.Priority.Should().Be(0);
                    x.RoleId.Should().Be(createRole.RoleId);
                },
                x =>
                {
                    x.Name.Should().Be("@everyone");
                    x.Priority.Should().Be(-1);
                    x.RoleId.Should().NotBeEmpty();
                }
            );

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