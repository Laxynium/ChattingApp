using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InstantMessenger.Shared.Modules
{
    public static class Extensions
    {
        private const string AppName = "InstantMessenger";

        public static IServiceCollection AddExceptionMapper<TMapper>(this IServiceCollection services) where TMapper : class,IExceptionMapper
        {
            services.AddSingleton<IExceptionMapper, TMapper>();
            return services;
        }

        public static IServiceCollection AddModuleRequests(this IServiceCollection services)
        {
            services.AddModuleRegistry();
            services.AddSingleton<IModuleSubscriber, ModuleSubscriber>();
            services.AddTransient<IModuleClient, ModuleClient>();

            return services;
        }
        public static IModuleSubscriber UseModuleRequests(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<IModuleSubscriber>();
        public static IApplicationBuilder UseModuleRegistry(this IApplicationBuilder app)
        {
            var moduleRegistry =app.ApplicationServices.GetRequiredService<IModuleRegistry>();
            var eventTypes = Assembly.GetCallingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && typeof(IIntegrationEvent).IsAssignableFrom(t))
                .ToList();
            foreach (var eventType in eventTypes)
            {
                moduleRegistry.AddBroadcastAction(
                    eventType,
                    (sp, @event) =>
                    {
                        var dispatcher = sp.GetService<IIntegrationEventDispatcher>();
                        return (Task)dispatcher.GetType()
                            .GetMethod(nameof(dispatcher.PublishAsync))
                            .MakeGenericMethod(eventType)
                            .Invoke(dispatcher, new[] { @event });
                    }
                );
            }
            return app;
        }
        private static void AddModuleRegistry(this IServiceCollection services)
        {
            services.AddSingleton<IModuleRegistry>(
                provider =>
                {
                    var logger = provider.GetRequiredService<ILogger<IModuleRegistry>>();
                    var registry = new ModuleRegistry(logger);
                    return registry;
                }
            );
        }
    }
}