using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Shared.Events;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InstantMessenger.Shared.Modules
{
    public static class Extensions
    {
        private const string AppName = "Bootstrap";
        public static IServiceCollection AddModuleRequest(this IServiceCollection services)
        {
            services.AddModuleRegistry();
            services.AddSingleton<IModuleSubscriber, ModuleSubscriber>();
            services.AddTransient<IModuleClient, ModuleClient>();

            return services;
        }
        public static IModuleSubscriber UseModuleRequests(this IApplicationBuilder app)
            => app.ApplicationServices.GetRequiredService<IModuleSubscriber>();
        private static void AddModuleRegistry(this IServiceCollection services)
        {
            var eventTypes = AppDomain.CurrentDomain
                .GetAssemblies()
                .Where(a => a.FullName.Contains(AppName))
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsClass && typeof(IEvent).IsAssignableFrom(t))
                .ToList();
            services.AddSingleton<IModuleRegistry>(
                provider =>
                {
                    var logger = provider.GetService<ILogger<IModuleRegistry>>();
                    var registry = new ModuleRegistry(logger);

                    foreach (var eventType in eventTypes)
                    {
                        registry.AddBroadcastAction(
                            eventType,
                            (sp, @event) =>
                            {
                                var dispatcher = sp.GetService<IEventDispatcher>();
                                return (Task) dispatcher.GetType()
                                    .GetMethod(nameof(dispatcher.Publish))
                                    .MakeGenericMethod(eventType)
                                    .Invoke(dispatcher, new[] {@event});
                            }
                        );
                    }

                    return registry;
                }
            );
        }
    }
}