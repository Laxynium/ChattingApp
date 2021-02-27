using System.Threading.Tasks;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Shared.Outbox
{
    internal interface IMessageOutbox<TModule> where TModule : IModule
    {
        Task AddAsync<T>(T message);
    }
}