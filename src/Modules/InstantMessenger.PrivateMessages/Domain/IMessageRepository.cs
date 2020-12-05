using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain.Entities;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;

namespace InstantMessenger.PrivateMessages.Domain
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
        Task<Message> GetAsync(MessageId id);
    }
}