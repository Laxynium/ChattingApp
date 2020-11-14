using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Commands
{
    public static class Extensions
    {
        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            return services.Scan(
                s =>
                    s.FromAssemblies(callingAssembly)
                        .AddClasses(c => c.AssignableTo(typeof(ICommandHandler<>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
            );
        }

        public static IServiceCollection AddCommandDispatcher(this IServiceCollection services) 
            => services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
    }
}