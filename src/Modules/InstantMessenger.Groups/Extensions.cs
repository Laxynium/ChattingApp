using InstantMessenger.Groups.Application.Hubs;
using InstantMessenger.Groups.Application.IntegrationEvents;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Groups.Domain.Messages;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.Rules;
using InstantMessenger.Groups.Infrastructure;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Groups.Infrastructure.Decorators;
using InstantMessenger.Shared.Decorators.UoW;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.Messages.Queries;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Outbox;
using Microsoft.AspNetCore.Builder;
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
                .AddCommandHandlers(typeof(TransactionCommandHandlerDecorator<>))
                .AddCommandDispatcher()
                .AddDomainEventHandlers(typeof(PublishDomainEventsEventHandlerDecorator<>))
                .AddDomainEventDispatcher()
                .AddQueryHandlers()
                .AddQueryDispatcher()
                .AddIntegrationEventHandlers()
                .AddIntegrationEventDispatcher()
                .AddModuleRequests()
                .AddExceptionMapper<ExceptionMapper>()
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
                .AddDbContext<GroupsViewContext>(
                    o =>
                    {
                        using var provider = services.BuildServiceProvider();
                        using var scope = provider.CreateScope();
                        var connectionString = scope.ServiceProvider.GetService<IConfiguration>()
                            .GetConnectionString("InstantMessengerDb");
                        o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                        o.UseSqlServer(
                            connectionString,
                            x => x.MigrationsHistoryTable("__EFMigrationsHistory_Views", "Groups")
                        );
                    })
                .AddUnitOfWork<GroupsContext, DomainEventsMapper>(outbox:true)
                .AddTransient<GroupsModuleFacade>()
                .AddScoped<IGroupRepository, GroupRepository>()
                .AddScoped<IInvitationRepository, InvitationRepository>()
                .AddScoped<IUniqueInvitationCodeRule, UniqueInvitationCodeRule>()
                .AddScoped<IChannelRepository, ChannelRepository>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .AddSingleton<IClock>(x => SystemClock.Instance);

            return services;
        }

        public static IApplicationBuilder UseGroupsModule(this IApplicationBuilder app)
        {
            app.UseEndpoints(
                x => { x.MapHub<GroupsHub>("/api/groups/hub"); }
            );
            return app;
        }
    }
}