using InstantMessenger.Friendships.Api;
using InstantMessenger.Friendships.Domain;
using InstantMessenger.Friendships.Infrastructure.Database;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace InstantMessenger.Friendships
{
    public static class Extensions
    {
        public static IServiceCollection AddFriendshipsModule(this IServiceCollection services)
        {
            services.AddCommandHandlers()
                .AddCommandDispatcher()
                .AddQueryHandlers()
                .AddQueryDispatcher()
                .AddEventHandlers()
                .AddModuleRequests()
                .AddDbContext<FriendshipsContext>(x =>
                {
                    using var provider = services.BuildServiceProvider();
                    using var scope = provider.CreateScope();
                    var connectionString = scope.ServiceProvider.GetService<IConfiguration>().GetConnectionString("InstantMessengerDb");
                    x.UseSqlServer(connectionString, x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Friendships"));
                })
                .AddScoped<IPersonRepository, PersonRepository>()
                .AddScoped<IInvitationRepository, InvitationRepository>()
                .AddScoped<IFriendshipRepository, FriendshipRepository>()
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddSingleton<IClock>(x=>SystemClock.Instance);
            return services;
        }
    }
}