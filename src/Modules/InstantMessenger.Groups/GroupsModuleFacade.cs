using System.Threading.Tasks;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Messages.Queries;

namespace InstantMessenger.Groups
{
    public class GroupsModuleFacade
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public GroupsModuleFacade(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }
        public async Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            await _commandDispatcher.SendAsync(command);
        }

        public async Task<TResult> QueryAsync<TResult>(IQuery<TResult> query)
        {
            return await _queryDispatcher.QueryAsync(query);
        }
    }
}