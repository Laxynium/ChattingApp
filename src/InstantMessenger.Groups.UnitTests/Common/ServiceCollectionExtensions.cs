using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Groups.UnitTests.Common
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Replace<TService, TImplementation>(
            this IServiceCollection services,
            ServiceLifetime lifetime)
            where TService : class
            where TImplementation : class, TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            var descriptorToAdd = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);

            services.Add(descriptorToAdd);

            return services;
        }
        public static IServiceCollection Remove<TService>(
            this IServiceCollection services)
            where TService : class
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));

            services.Remove(descriptorToRemove);

            return services;
        }

        public static IServiceCollection RemoveDbContext<TDbContext>(
            this IServiceCollection services)
            where TDbContext : DbContext
        {
            services.Remove<TDbContext>()
                .Remove<DbContextOptions>()
                .Remove<DbContextOptions<TDbContext>>();
            return services;
        }


    }
}