using System.Threading.Tasks;

namespace InstantMessenger.Shared.Messages.Events
{
    public interface IDomainEventHandler<in TEvent> where TEvent : class, IDomainEvent
    {
        Task HandleAsync(TEvent @event);
    }
}