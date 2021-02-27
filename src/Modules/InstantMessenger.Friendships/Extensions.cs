using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Friendships
{
    public static class Extensions
    {
        public static IServiceCollection AddFriendshipsModule(this IServiceCollection services) =>
            new FriendshipsModule().ConfigureServices(services);

        public static IApplicationBuilder UseFriendshipsModule(this IApplicationBuilder app) =>
            new FriendshipsModule().UseMiddleware(app);
    }
}