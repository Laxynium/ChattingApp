using System.Threading.Tasks;

namespace InstantMessenger.Shared.Messages.Commands
{
    public interface ICommandDispatcher
    {
        Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand;
    }
}