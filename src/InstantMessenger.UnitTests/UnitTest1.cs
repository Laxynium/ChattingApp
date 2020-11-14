using System;
using System.Threading.Tasks;
using InstantMessenger.Groups;
using InstantMessenger.Groups.Api.Features.GroupCreation;
using InstantMessenger.Groups.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace InstantMessenger.UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var services = new ServiceCollection();
            services.AddGroupsModule()
                .RemoveDbContext<GroupsContext>()
                .Replace<IGroupRepository, FakeRepository>(ServiceLifetime.Scoped);

            using var provider = services.BuildServiceProvider();
            using var scope = provider.CreateScope();

            var sut = scope.ServiceProvider.GetRequiredService<GroupsModuleFacade>();

            await sut.SendAsync(new CreateGroupCommand(Guid.NewGuid(), Guid.NewGuid(), "test"));
        }
    }
}
