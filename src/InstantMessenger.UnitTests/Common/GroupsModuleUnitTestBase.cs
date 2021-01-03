using System;
using System.Collections.Generic;
using InstantMessenger.Groups;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.MessageBrokers;
using InstantMessenger.Shared.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace InstantMessenger.UnitTests.Common
{
    public class GroupsModuleUnitTestBase : UnitTestBase<GroupsModuleFacade>
    {
        public GroupsModuleUnitTestBase()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string> { ["outbox.enabled"] = "false" })
                .Build();
            Configure(
                services =>
                {

                    var dbName = Guid.NewGuid().ToString();
                    services
                        .AddSingleton(configuration)
                        .AddGroupsModule()
                        .RemoveDbContext<GroupsContext>()
                        .AddDbContext<GroupsContext>(o => o.UseInMemoryDatabase(dbName))
                        .Replace<IMessageBroker, FakeMessageBroker>(ServiceLifetime.Transient)
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