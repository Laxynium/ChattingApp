using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Identity
{
    public static class Extensions
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services) =>
            new IdentityModule().ConfigureServices(services);

        public static IApplicationBuilder UseIdentityModule(this IApplicationBuilder app) =>
            new IdentityModule().UseMiddleware(app);
    }
}