using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Messages.Events
{
    public static class Extensions
    {
        public static IServiceCollection AddDomainEventHandlers(this IServiceCollection services, params Type[] decorators)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            var registrator = new MessageHandlersRegistrator(typeof(IDomainEventHandler<>));
            return registrator.Register(services, callingAssembly, decorators);
        }

        public static IServiceCollection AddDomainEventDispatcher(this IServiceCollection services)
        {
            return services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
        }
    }
}