using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Outbox
{
    public class Extensions
    {
        public IServiceCollection AddOutbox<TDbContext>(IServiceCollection serviceCollection) where TDbContext : DbContext
        {
            serviceCollection.AddTransient<IMessageOutbox, MessageOutbox<TDbContext>>();
            serviceCollection.AddHostedService<OutboxProcessor<TDbContext>>();
            return serviceCollection;
        }
    }
}