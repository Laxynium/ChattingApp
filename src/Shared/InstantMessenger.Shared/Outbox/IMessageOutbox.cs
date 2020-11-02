using System.Threading.Tasks;

namespace InstantMessenger.Shared.Outbox
{
    public interface IMessageOutbox
    {
        Task AddAsync<T>(T message);
    }
}