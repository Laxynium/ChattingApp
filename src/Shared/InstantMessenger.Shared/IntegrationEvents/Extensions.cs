using System.Reflection;
using InstantMessenger.Shared.Decorators;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.IntegrationEvents
{
    public static class Extensions
    {
        public static IServiceCollection AddIntegrationEventHandlers(this IServiceCollection services)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            services.Scan(
                s => s.FromAssemblies(callingAssembly)
                    .AddClasses(c => c.AssignableTo(typeof(IIntegrationEventHandler<>)).Where(t => !t.IsDefined(typeof(DecoratorAttribute), true)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );

            return services;
        }

        public static IServiceCollection AddIntegrationEventDispatcher(this IServiceCollection services)
        {
            return services.AddScoped<IIntegrationEventDispatcher, IntegrationEventDispatcher>();
        }
    }
}