using System.Threading.Tasks;

namespace InstantMessenger.Shared.Messages.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        Task HandleAsync(TCommand command);
    }
}