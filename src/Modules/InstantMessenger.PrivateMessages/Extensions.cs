using InstantMessenger.PrivateMessages.Application.Hubs;
using InstantMessenger.PrivateMessages.Application.IntegrationEvents;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Infrastructure;
using InstantMessenger.PrivateMessages.Infrastructure.Database;
using InstantMessenger.PrivateMessages.Infrastructure.Decorators;
using InstantMessenger.Shared.Decorators.UoW;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Messages.Events;
using InstantMessenger.Shared.Messages.Queries;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Mvc;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.PrivateMessages
{
    public static class Extensions
    {
        public static IServiceCollection AddPrivateMessagesModule(this IServiceCollection services)
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
                .AddDbContext<PrivateMessagesContext>(
                    (provider, o) =>
                    {
                        o.UseSqlServer(
                            provider.GetConnectionString("InstantMessengerDb"),
                            x => x.MigrationsHistoryTable("__EFMigrationsHistory", "PrivateMessages")
                        );
                    }
                )
                .AddUnitOfWork<PrivateMessagesContext, DomainEventMapper>(outbox: true)
                .AddScoped<IConversationRepository, ConversationRepository>()
                .AddScoped<IMessageRepository, MessageRepository>();
            return services;
        }

        public static IApplicationBuilder UsePrivateMessagesModule(this IApplicationBuilder app)
        {
            app.UseEndpoints(
                x => x.MapHub<PrivateMessagesHub>("/api/privateMessages/hub")
            );
            app.UseModuleRegistry();
            return app;
        }
    }
}