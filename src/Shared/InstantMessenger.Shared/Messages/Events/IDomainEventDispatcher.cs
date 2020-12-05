using System.Threading.Tasks;

namespace InstantMessenger.Shared.Messages.Events
{
    public interface IDomainEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IDomainEvent;
        Task PublishAsync(IDomainEvent @event);
    }
}