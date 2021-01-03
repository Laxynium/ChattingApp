using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Outbox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public static class Extensions
    {
        public static IServiceCollection AddUnitOfWork<TDbContext, TDomainEventsMapper>(this IServiceCollection services, bool outbox = false)
            where  TDbContext:DbContext
            where TDomainEventsMapper: class, IDomainEventMapper
        {
            services.AddScoped<UnitOfWork<TDbContext>>();
            services.AddScoped<DomainEventsAccessor<TDbContext>>();
            services.AddScoped<DomainEventPublisher<TDbContext>>();
            services.AddIntegrationEventPublisher<TDbContext>(outbox);
            services.AddSingleton<IDomainEventMapper, TDomainEventsMapper>();

            return services;
        }

        private static void AddIntegrationEventPublisher<TDbContext>(this IServiceCollection services, bool outbox)
            where TDbContext : DbContext
        {
            if (!outbox)
            {
                services.AddScoped<IIntegrationEventsPublisher<TDbContext>, InMemoryIntegrationEventsPublisher<TDbContext>>();
            }
            else
            {
                services.AddOutbox<TDbContext>();
                using (var scope = services.BuildServiceProvider().CreateScope())
                {
                    var options = scope.ServiceProvider.GetService<OutboxOptions>() ?? new OutboxOptions {Enabled = false};
                    if (options.Enabled)
                        services.AddTransient<IIntegrationEventsPublisher<TDbContext>, OutboxIntegrationEventsPublisher<TDbContext>>();
                    else
                        services.AddScoped<IIntegrationEventsPublisher<TDbContext>, InMemoryIntegrationEventsPublisher<TDbContext>>();
                }
            }
        }
    }
}