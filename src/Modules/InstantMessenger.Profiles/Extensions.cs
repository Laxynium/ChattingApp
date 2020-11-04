using InstantMessenger.Profiles.Api;
using InstantMessenger.Profiles.Domain;
using InstantMessenger.Profiles.Infrastructure.Database;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Profiles
{
    public static class Extensions
    {
        public static IServiceCollection AddProfilesModule(this IServiceCollection services)
        {
            services.AddCommandHandlers()
                .AddCommandDispatcher()
                .AddQueryHandlers()
                .AddQueryDispatcher()
                .AddEventHandlers()
                .AddModuleRequests()
                .AddDbContext<ProfilesContext>(
                    o =>
                    {
                        using var provider = services.BuildServiceProvider();
                        using var scope = provider.CreateScope();
                        var connectionString = scope.ServiceProvider.GetService<IConfiguration>().GetConnectionString("InstantMessengerDb");
                        o.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Profiles"));
                    }
                )
                .AddScoped<IProfileRepository, ProfileRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}