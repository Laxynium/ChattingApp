using System.Threading.Tasks;

namespace InstantMessenger.Shared.Commands
{
    public interface ICommandDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}