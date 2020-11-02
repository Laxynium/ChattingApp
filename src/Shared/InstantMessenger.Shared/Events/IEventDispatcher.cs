using System.Threading.Tasks;

namespace InstantMessenger.Shared.Events
{
    public interface IEventDispatcher
    {
        Task Publish<TEvent>(TEvent @event) where TEvent : class, IEvent;
    }
}