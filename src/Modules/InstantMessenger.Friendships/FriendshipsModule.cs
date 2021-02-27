using InstantMessenger.Friendships.Application.Hubs;
using InstantMessenger.Friendships.Application.IntegrationEvents;
using InstantMessenger.Friendships.Domain.Repositories;
using InstantMessenger.Friendships.Domain.Rules;
using InstantMessenger.Friendships.Infrastructure;
using InstantMessenger.Friendships.Infrastructure.Database;
using InstantMessenger.Friendships.Infrastructure.Database.Repositories;
using InstantMessenger.Friendships.Infrastructure.Database.Rules;
using InstantMessenger.Friendships.Infrastructure.Decorators;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.Messages.Queries;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Mvc;
using InstantMessenger.Shared.UoW;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;

namespace InstantMessenger.Friendships
{
    internal class FriendshipsModule : IModule
    {
        public string Name => "friendships";
        
        public IServiceCollection ConfigureServices(IServiceCollection services)
        {
            services
                .AddCommandHandlers(typeof(CommandHandlerTransactionDecorator<>))
                .AddCommandDispatcher()
                .AddDomainEventHandlers(typeof(PublishDomainEventsEventHandlerDecorator<>))
                .AddDomainEventDispatcher()
                .AddQueryHandlers()
                .AddQueryDispatcher()
                .AddIntegrationEventHandlers()
                .AddIntegrationEventDispatcher()
                .AddModuleRequests()
                .AddExceptionMapper<ExceptionMapper>()
                .AddDbContext<FriendshipsContext>((provider, builder) => builder.UseSqlServer(
                    provider.GetConnectionString("InstantMessengerDb"),
                    x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Friendships")))
                .AddUnitOfWork<FriendshipsModule, DomainEventsMapper>(c => c.UseEFCore<FriendshipsContext>())
                .AddSingleton<FriendshipsModuleFacade>()
                .AddScoped<IInvitationRepository, InvitationRepository>()
                .AddScoped<IFriendshipRepository, FriendshipRepository>()
                .AddScoped<IUniquePendingInvitationRule, UniquePendingInvitationRule>()
                .AddSingleton<IClock>(x => SystemClock.Instance);

            return services;
        }

        public IApplicationBuilder UseMiddleware(IApplicationBuilder app)
        {
            app.UseEndpoints(
                x => x.MapHub<FriendshipsHub>("/api/friendships/hub"));
            return app;
        }
    }
}