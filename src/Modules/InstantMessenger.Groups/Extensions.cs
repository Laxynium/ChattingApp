using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Events;
using InstantMessenger.Shared.Modules;
using InstantMessenger.Shared.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Groups
{
    public static class Extensions
    {
        public static IServiceCollection AddGroupsModule(this IServiceCollection services)
        {
            services.AddCommandHandlers()
                .AddCommandDispatcher()
                .AddQueryHandlers()
                .AddQueryDispatcher()
                .AddEventHandlers()
                .AddModuleRequests();
            return services;
        }
    }
}