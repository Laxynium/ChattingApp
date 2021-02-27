using InstantMessenger.PrivateMessages.Application.Hubs;
using InstantMessenger.PrivateMessages.Application.IntegrationEvents;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Infrastructure;
using InstantMessenger.PrivateMessages.Infrastructure.Database;
using InstantMessenger.PrivateMessages.Infrastructure.Decorators;
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

namespace InstantMessenger.PrivateMessages
{
    internal sealed class PrivateMessagesModule : IModule
    {
        public string Name => "private-messages";
        public IServiceCollection ConfigureServices(IServiceCollection services)
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
                .AddUnitOfWork<PrivateMessagesModule, DomainEventMapper>(c => c.UseEFCore<PrivateMessagesContext>())
                .AddScoped<IConversationRepository, ConversationRepository>()
                .AddScoped<IMessageRepository, MessageRepository>();
            return services;

        }

        public IApplicationBuilder UseMiddleware(IApplicationBuilder app)
        {
            app.UseEndpoints(
                x => x.MapHub<PrivateMessagesHub>("/api/privateMessages/hub")
            );
            return app;
        }
    }
}