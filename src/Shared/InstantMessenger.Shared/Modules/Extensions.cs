using System;
using System.Collections.Generic;
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
            services.AddModuleRegistry(Assembly.GetCallingAssembly());
            services.AddSingleton<IModuleSubscriber, ModuleSubscriber>();
            services.AddTransient<IModuleClient, ModuleClient>();

            return services;
        }
        public static IModuleSubscriber UseModuleRequests(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<IModuleSubscriber>();

        private static void AddModuleRegistry(this IServiceCollection services, Assembly callingAssembly)
        {
            var eventTypes = callingAssembly
                .GetTypes()
                .Where(t => t.IsClass && typeof(IIntegrationEvent).IsAssignableFrom(t))
                .ToList();
            services.AddSingleton(sp => new ModuleIntegrationEventsProvider(eventTypes));
            
            services.AddSingleton<IModuleRegistry>(
                provider =>
                {
                    var logger = provider.GetService<ILogger<ModuleRegistry>>();
                    var allEventTypes =
                        provider.GetRequiredService<IEnumerable<ModuleIntegrationEventsProvider>>()
                            .SelectMany(x=>x.IntegrationEventTypes);
                    var registry = new ModuleRegistry(logger);

                    foreach (var eventType in allEventTypes)
                    {
                        registry.AddBroadcastAction(
                            eventType,
                            (sp, @event) =>
                            {
                                var dispatcher = sp.GetRequiredService<IIntegrationEventDispatcher>();
                                return (Task)dispatcher.GetType()
                                    .GetMethod(nameof(dispatcher.PublishAsync))
                                    .MakeGenericMethod(eventType)
                                    .Invoke(dispatcher, new[] { @event });
                            }
                        );
                    }

                    return registry;
                }
            );
        }
    }
}