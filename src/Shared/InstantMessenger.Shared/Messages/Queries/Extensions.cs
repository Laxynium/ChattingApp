using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Messages.Queries
{
    public static class Extensions
    {
        public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
        {
            var callingAssembly = Assembly.GetCallingAssembly();
            return services.Scan(
                s =>
                    s.FromAssemblies(callingAssembly)
                        .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                        .AsImplementedInterfaces()
                        .WithTransientLifetime()
            );
        }

        public static IServiceCollection AddQueryDispatcher(this IServiceCollection services)
            => services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
    }
}