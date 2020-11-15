using InstantMessenger.Groups.Api;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

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
                .AddScoped<IGroupRepository, GroupRepository>()
                .AddScoped<IUnitOfWork,UnitOfWork>()
                .AddSingleton<IClock>(x => SystemClock.Instance); ;
            return services;
        }
    }
}