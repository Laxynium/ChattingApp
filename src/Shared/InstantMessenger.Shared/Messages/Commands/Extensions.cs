using System;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Messages.Commands
{
    public static class Extensions
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services, params Type[]decorators)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            var registrator = new MessageHandlersRegistrator(typeof(ICommandHandler<>));
            return registrator.Register(services, callingAssembly, decorators);

        }
        public static IServiceCollection AddCommandDispatcher(this IServiceCollection services) 
            => services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
    }
}