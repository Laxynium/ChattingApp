using InstantMessenger.Shared.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Outbox
{
    internal static class Extensions
    {
        public static IServiceCollection AddOutbox<TDbContext>(this IServiceCollection serviceCollection) where TDbContext : DbContext
        {
            var options = serviceCollection.TryGetOptions<OutboxOptions>("outbox");
            if(options is {})
                serviceCollection.AddSingleton(options);
            serviceCollection.AddTransient<MessageOutbox<TDbContext>>();
            serviceCollection.AddHostedService<OutboxProcessor<TDbContext>>();
            return serviceCollection;
        }
    }
}