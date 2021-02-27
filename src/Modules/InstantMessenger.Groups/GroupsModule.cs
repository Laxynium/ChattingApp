using InstantMessenger.Groups.Application.Hubs;
using InstantMessenger.Groups.Application.IntegrationEvents;
using InstantMessenger.Groups.Domain.Messages;
using InstantMessenger.Groups.Domain.Repositories;
using InstantMessenger.Groups.Domain.Rules;
using InstantMessenger.Groups.Infrastructure;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Groups.Infrastructure.Database.Repositories;
using InstantMessenger.Groups.Infrastructure.Database.Rules;
using InstantMessenger.Groups.Infrastructure.Decorators;
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

namespace InstantMessenger.Groups
{
    internal sealed class GroupsModule: IModule
    {
        public string Name => "groups";
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
                .AddDbContext<GroupsContext>(
                    (provider, o) => o.UseSqlServer(
                        provider.GetConnectionString("InstantMessengerDb"),
                        x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Groups")
                    )
                )
                .AddUnitOfWork<GroupsModule, DomainEventsMapper>(c => c.UseEFCore<GroupsContext>())
                .AddTransient<GroupsModuleFacade>()
                .AddScoped<IGroupRepository, GroupRepository>()
                .AddScoped<IInvitationRepository, InvitationRepository>()
                .AddScoped<IUniqueInvitationCodeRule, UniqueInvitationCodeRule>()
                .AddScoped<IChannelRepository, ChannelRepository>()
                .AddScoped<IMessageRepository, MessageRepository>()
                .AddSingleton<IClock>(x => SystemClock.Instance);

            return services;
        }

        public IApplicationBuilder UseMiddleware(IApplicationBuilder app)
        {
            app.UseEndpoints(
                x => { x.MapHub<GroupsHub>("/api/groups/hub"); }
            );
            return app;
        }
    }
}