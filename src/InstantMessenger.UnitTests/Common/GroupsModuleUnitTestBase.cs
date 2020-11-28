using System;
using InstantMessenger.Groups;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace InstantMessenger.UnitTests.Common
{
    public class GroupsModuleUnitTestBase : UnitTestBase<GroupsModuleFacade>
    {
        public GroupsModuleUnitTestBase()
        {
            Configure(
                services =>
                {
                    
                    var dbName = Guid.NewGuid().ToString();
                    services.AddGroupsModule()
                        .RemoveDbContext<GroupsContext>()
                        .AddDbContext<GroupsContext>(o => o.UseInMemoryDatabase(dbName))
                        .AddSingleton(typeof(ILogger<>),typeof(NullLogger<>));
                }
            );
            Configure(sc => sc
                .Remove<IModuleClient>()
                .AddSingleton<IModuleClient>(x => new FakeModuleClient())
            );
        }
    }
}