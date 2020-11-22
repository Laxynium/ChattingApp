using System.Threading.Tasks;

namespace InstantMessenger.Shared.Events
{
    public interface IEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
    }
}