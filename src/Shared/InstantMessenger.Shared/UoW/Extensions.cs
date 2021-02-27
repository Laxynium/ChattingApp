using System;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Mvc;
using InstantMessenger.Shared.Outbox;
using InstantMessenger.Shared.Outbox.EFCore;
using InstantMessenger.Shared.UoW.EFCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.UoW
{
    public static class Extensions
    {
        public static IServiceCollection AddUnitOfWork<TModule, TDomainEventMapper>(this IServiceCollection services,
            Func<UnitOfWorkConfig, UnitOfWorkConfig> configure)
            where TModule : IModule
            where TDomainEventMapper : class, IDomainEventMapper
        {
            var config = configure(new UnitOfWorkConfig());
            services.AddScoped<IDomainEventAccessor<TModule>, EFCoreDomainEventAccessor<TModule>>(sp =>
            {
                var dbContext = (DbContext) sp.GetRequiredService(config.DbContextType);
                return new EFCoreDomainEventAccessor<TModule>(dbContext);
            });
            services.AddScoped<IDomainEventPublisher<TModule>, EFCoreDomainEventPublisher<TModule>>();
            services.AddSingleton<IDomainEventMapper, TDomainEventMapper>();
            services.AddScoped<IUnitOfWork<TModule>, EFCoreUnitOfWork<TModule>>(sp =>
            {
                var dbContext = (DbContext) sp.GetRequiredService(config.DbContextType);
                return new EFCoreUnitOfWork<TModule>(dbContext,
                    sp.GetRequiredService<IDomainEventPublisher<TModule>>());
            });
            services.AddIntegrationEventPublisher<TModule>(config.DbContextType);
            return services;
        }

        private static IServiceCollection AddIntegrationEventPublisher<TModule>(this IServiceCollection services,
            Type dbContextType)
            where TModule : IModule
        {
            var options = services.TryGetOptions<OutboxOptions>("outbox") ?? new OutboxOptions {Enabled = false};
            if (!options.Enabled)
            {
                services.AddScoped<IIntegrationEventsPublisher<TModule>, InMemoryIntegrationEventsPublisher<TModule>>();
            }
            else
            {
                services.AddOutbox<TModule>(dbContextType);
                services
                    .AddTransient<IIntegrationEventsPublisher<TModule>, OutboxIntegrationEventsPublisher<TModule>>();
            }

            return services;
        }

        private static IServiceCollection AddOutbox<TModule>(this IServiceCollection services, Type dbContextType)
            where TModule : IModule
        {
            var options = services.TryGetOptions<OutboxOptions>("outbox");
            if (options is { })
                services.AddSingleton(options);

            services.AddTransient<IMessageOutbox<TModule>, EFCoreMessageOutbox<TModule>>(sp =>
            {
                var dbContext = (DbContext) sp.GetRequiredService(dbContextType);
                return new EFCoreMessageOutbox<TModule>(dbContext);
            });

            services.AddTransient<IOutboxMessageAccessor<TModule>, EFCoreOutboxMessageAccessor<TModule>>(sp =>
            {
                var dbContext = (DbContext) sp.GetRequiredService(dbContextType);
                return new EFCoreOutboxMessageAccessor<TModule>(dbContext, sp.GetRequiredService<OutboxOptions>());
            });
            services.AddHostedService<OutboxProcessor<TModule>>();
            services.AddHostedService<CleanUpOutboxProcessor<TModule>>();

            return services;
        }

        public class UnitOfWorkConfig
        {
            public Type DbContextType { get; private set; }

            public UnitOfWorkConfig()
            {
            }

            public UnitOfWorkConfig UseEFCore<TDbContext>() where TDbContext : DbContext
            {
                DbContextType = typeof(TDbContext);
                return this;
            }
        }
    }
}