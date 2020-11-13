using System.Threading.Tasks;

namespace InstantMessenger.PrivateMessages.Domain
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
        Task<Message> GetAsync(MessageId id);
    }
}