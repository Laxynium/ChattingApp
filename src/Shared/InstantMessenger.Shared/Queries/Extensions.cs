using System;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Queries
{
    public static class Extensions
    {
        public static IServiceCollection AddQueryHandlers(this IServiceCollection services) 
            => services.Scan(s =>
                s.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                    .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                    .AsImplementedInterfaces()
                    .WithTransientLifetime()
            );

        public static IServiceCollection AddQueryDispatcher(this IServiceCollection services)
            => services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
    }
}