using InstantMessenger.Groups.Api.Features.GroupCreation;
using InstantMessenger.Groups.Infrastructure;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Groups
{
    public static class Extensions
    {
        public static IServiceCollection AddGroupsModule(this IServiceCollection services)
        {
            services
                .AddCommandHandlers()
                .AddCommandDispatcher()
                .AddQueryHandlers()
                .AddQueryDispatcher()
                .AddEventHandlers()
                .AddModuleRequests()
                .AddDbContext<GroupsContext>(
                    o =>
                    {
                        using var provider = services.BuildServiceProvider();
                        using var scope = provider.CreateScope();
                        var connectionString = scope.ServiceProvider.GetService<IConfiguration>()
                            .GetConnectionString("InstantMessengerDb");
                        o.UseSqlServer(
                            connectionString,
                            x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Groups")
                        );
                    }
                )
                .AddTransient<GroupsModuleFacade>()
                .AddScoped<IGroupRepository, GroupRepository>();
            return services;
        }
    }
}