using System.Threading.Tasks;

namespace InstantMessenger.Shared.Events
{
    public interface IEventHandler<in TEvent> where TEvent : class, IEvent
    {
        Task HandleAsync(TEvent @event);
    }
}