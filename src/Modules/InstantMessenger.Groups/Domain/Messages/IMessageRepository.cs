using System.Threading.Tasks;
using InstantMessenger.Groups.Domain.Messages.Entities;

namespace InstantMessenger.Groups.Domain.Messages
{
    public interface IMessageRepository
    {
        Task AddAsync(Message message);
    }
}