using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.PrivateMessages
{
    public static class Extensions
    {
        public static IServiceCollection AddPrivateMessagesModule(this IServiceCollection services) =>
            new PrivateMessagesModule().ConfigureServices(services);

        public static IApplicationBuilder UsePrivateMessagesModule(this IApplicationBuilder app) =>
            new PrivateMessagesModule().UseMiddleware(app);
    }
}