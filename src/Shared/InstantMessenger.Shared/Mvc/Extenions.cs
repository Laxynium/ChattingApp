using System;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Mvc
{
    public static class Extensions
    {
        public static TModel TryGetOptions<TModel>(this IConfiguration configuration, string sectionName)
            where TModel : class,new()
        {
            var section = configuration.GetSection(sectionName);
            if(section is null)
                return null;
            var model = new TModel();
            section.Bind(model);
            return model;
        }

        public static TModel TryGetOptions<TModel>(this IServiceCollection services, string sectionName)
            where TModel : class, new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            return configuration.TryGetOptions<TModel>(sectionName);
        }

        public static TModel GetOptions<TModel>(this IConfiguration configuration, string sectionName)
            where TModel : new()
        {
            var model = new TModel();
            configuration.GetSection(sectionName).Bind(model);
            return model;
        }

        public static TModel GetOptions<TModel>(this IServiceCollection services, string settingsSectionName)
            where TModel : new()
        {
            using var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            return configuration.GetOptions<TModel>(settingsSectionName);
        }

        public static Guid GetUserId(this ClaimsPrincipal user) =>  user.Identity.Name is null ? Guid.Empty : Guid.Parse(user.Identity.Name);

        public static string GetConnectionString(this IServiceProvider provider, string name)
        {
            var connectionString = provider.GetService<IConfiguration>()
                .GetConnectionString(name);
            return connectionString;
        }
    }
}