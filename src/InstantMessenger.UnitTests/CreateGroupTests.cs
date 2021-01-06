using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Application.Features.Group.Create;
using InstantMessenger.Groups.Application.Queries;
using InstantMessenger.Groups.Domain.Exceptions;
using InstantMessenger.Shared.Modules;
using InstantMessenger.UnitTests.Common;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class CreateGroupTests : GroupsModuleUnitTestBase
    {
        public CreateGroupTests() => Configure(sc => sc
            .Remove<IModuleClient>()
            .AddSingleton<IModuleClient>(x => new FakeModuleClient("test_user"))
        );

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task Fails_when_group_name_is_invalid(string groupName) => await Run(async sut =>
        {
            var command = new CreateGroupCommand(Guid.NewGuid(), Guid.NewGuid(), groupName);

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<InvalidGroupNameException>();
        });

        [Fact]
        public async Task Creates_group_when_name_is_correct() => await Run(async sut =>
        {
            var command = new CreateGroupCommand(Guid.NewGuid(), Guid.NewGuid(), "group_name");

            //when
            await sut.SendAsync(command);

            //then group with correct name is created
            var groups = await sut.QueryAsync(
                new GetGroupsQuery(command.UserId,command.GroupId));
            groups.Should().SatisfyRespectively(
                x =>
                {
                    x.GroupId.Should().Be(command.GroupId);
                    x.Name.Should().Be(command.GroupName);
                    x.CreatedAt.Should().NotBe(default);
                }
            );
            //then everyone role is created
            var roles = await sut.QueryAsync(new GetRolesQuery(command.UserId, command.GroupId));
            roles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Name.Should().Be("@everyone");
                    x.Priority.Should().Be(-1);
                }
            );
            //then owner member is created
            var members = await sut.QueryAsync(new GetMembersQuery(command.UserId,command.GroupId));
            members.Should().SatisfyRespectively(
                x =>
                {
                    x.UserId.Should().Be(command.UserId);
                    x.MemberId.Should().NotBeEmpty();
                    x.IsOwner.Should().BeTrue();
                    x.Name.Should().Be("test_user");
                    x.CreatedAt.Should().NotBe(default);
                });
            //then owner has role @everyone
            var ownerRoles = await sut.QueryAsync(new GetMemberRolesQuery(command.UserId, command.GroupId, command.UserId));
            ownerRoles.Should().SatisfyRespectively(
                x =>
                {
                    x.RoleId.Should().NotBeEmpty();
                    x.Name.Should().Be("@everyone");
                    x.Priority.Should().Be(-1);
                });
        });
    }
}
