using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Groups
{
    public static class Extensions
    {
        public static IServiceCollection AddGroupsModule(this IServiceCollection services) =>
            new GroupsModule().ConfigureServices(services);

        public static IApplicationBuilder UseGroupsModule(this IApplicationBuilder app) =>
            new GroupsModule().UseMiddleware(app);
    }
}