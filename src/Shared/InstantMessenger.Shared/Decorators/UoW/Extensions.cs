using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Decorators.UoW
{
    public static class Extensions
    {
        public static IServiceCollection AddUnitOfWork<TDbContext, TDomainEventsMapper>(this IServiceCollection services)
            where  TDbContext:DbContext
            where TDomainEventsMapper: class, IDomainEventMapper
        {
            services.AddScoped<UnitOfWork<TDbContext>>();
            services.AddScoped<DomainEventsAccessor<TDbContext>>();
            services.AddScoped<DomainEventPublisher<TDbContext>>();
            services.AddScoped<IntegrationEventsPublisher>();
            services.AddSingleton<IDomainEventMapper, TDomainEventsMapper>();

            return services;
        }
    }
}