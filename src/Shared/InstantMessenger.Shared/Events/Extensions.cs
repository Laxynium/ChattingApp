using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Events
{
    public static class Extensions
    {
        public static IServiceCollection AddEventHandlers(this IServiceCollection services)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            return services.Scan(
                s => s.FromAssemblies(callingAssembly)
                    .AddClasses(c => c.AssignableTo(typeof(IEventHandler<>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );
        }

        public static IServiceCollection AddEventDispatcher(this IServiceCollection services)
        {
            return services.AddSingleton<IEventDispatcher, EventDispatcher>();
        }
    }
}