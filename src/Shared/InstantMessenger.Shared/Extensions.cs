using InstantMessenger.Shared.MessageBrokers;
using InstantMessenger.Shared.Messages.Events;
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
                .AddDomainEventDispatcher()
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