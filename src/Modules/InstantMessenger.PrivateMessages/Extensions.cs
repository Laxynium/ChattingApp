using InstantMessenger.PrivateMessages.Api.Hubs;
using InstantMessenger.PrivateMessages.Api.IntegrationEvents;
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
                    o =>
                    {
                        using var provider = services.BuildServiceProvider();
                        using var scope = provider.CreateScope();
                        var connectionString = scope.ServiceProvider.GetService<IConfiguration>()
                            .GetConnectionString("InstantMessengerDb");
                        o.UseSqlServer(
                            connectionString,
                            x => x.MigrationsHistoryTable("__EFMigrationsHistory", "PrivateMessages")
                        );
                    }
                )
                .AddUnitOfWork<PrivateMessagesContext,DomainEventMapper>()
                .AddScoped<IConversationRepository, ConversationRepository>()
                .AddScoped<IMessageRepository, MessageRepository>();
                
            services.AddSignalR(
                x => { x.EnableDetailedErrors = true; }
            ).AddNewtonsoftJsonProtocol();
            return services;
        }

        public static IApplicationBuilder UsePrivateMessagesModule(this IApplicationBuilder app)
        {
            app.UseEndpoints(
                x => x.MapHub<PrivateMessagesHub>("/api/privateMessages/hub")
            );
            return app;
        }
    }
}