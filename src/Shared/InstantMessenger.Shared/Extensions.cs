using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.MessageBrokers;
using InstantMessenger.Shared.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared
{
    public static class Extensions
    {
        public static IServiceCollection AddSharedModule(this IServiceCollection services)
        {
            services
                .AddEventDispatcher()
                .AddModuleRequests();

            services.AddTransient<IMessageBroker, MessageBroker>();

            return services;
        }

        public static IApplicationBuilder UseSharedModule(this IApplicationBuilder app)
        {
            return app;
        }
    }
}