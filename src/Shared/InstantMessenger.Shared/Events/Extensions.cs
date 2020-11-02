using System;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Events
{
    public static class Extensions
    {
        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            services.Scan(
                s => s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );
            return services;
        }

        public static IServiceCollection AddEventDispatcher(this IServiceCollection services)
        {
            return services.AddSingleton<IEventDispatcher, EventDispatcher>();
        }
    }
}