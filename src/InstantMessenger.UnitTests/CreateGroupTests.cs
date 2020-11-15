using System;
using System.Threading.Tasks;
using FluentAssertions;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Features.Group.Create.ExternalQueries;
using InstantMessenger.Groups.Api.Queries;
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
            .AddSingleton<IModuleClient>(x => new FakeModuleClient())
        );

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("   ")]
        public async Task Fails_when_group_name_is_invalid(string groupName) => await Run(async sut =>
        {
            var command = new CreateGroupCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), groupName);

            Func<Task> action = async () => await sut.SendAsync(command);

            await action.Should().ThrowAsync<Exception>();
        });

        [Fact]
        public async Task Creates_group_when_name_is_correct() => await Run(async sut =>
        {
            var command = new CreateGroupCommand(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), "group_name");

            await sut.SendAsync(command);

            var groupDtos = await sut.QueryAsync(new GetGroupsQuery(command.GroupId));
            groupDtos.Should().SatisfyRespectively(
                x =>
                {
                    x.GroupId.Should().Be(command.GroupId);
                    x.Name.Should().Be(command.GroupName);
                    x.CreatedAt.Should().NotBe(default);
                }
            );
            var members = await sut.QueryAsync(new GetMembersQuery(command.GroupId));
            members.Should().SatisfyRespectively(
                x =>
                {
                    x.UserId.Should().Be(command.UserId);
                    x.MemberId.Should().Be(command.OwnerId);
                    x.Name.Should().Be("test_user");
                    x.CreatedAt.Should().NotBe(default);
                    x.IsOwner.Should().BeTrue();
                });
        });
    }

    internal sealed class FakeModuleClient : IModuleClient
    {
        public Task<TResult> GetAsync<TResult>(string path, object moduleRequest) where TResult : class
        {
            var result = moduleRequest switch
            {
                MeQuery q => new UserDto(q.UserId, "test_user")
            };
            return Task.FromResult((TResult) System.Convert.ChangeType(result, typeof(TResult)));
        }

        public Task PublishAsync(object moduleBroadcast)
        {
            return Task.CompletedTask;
        }
    }
}
