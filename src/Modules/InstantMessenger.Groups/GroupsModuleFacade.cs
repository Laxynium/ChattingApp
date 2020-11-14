using System.Threading.Tasks;
using InstantMessenger.Shared.Commands;

namespace InstantMessenger.Groups
{
    public class GroupsModuleFacade
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public GroupsModuleFacade(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        public async Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            await _commandDispatcher.SendAsync(command);
        }
    }
}