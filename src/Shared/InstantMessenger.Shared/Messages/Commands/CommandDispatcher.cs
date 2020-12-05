using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace InstantMessenger.Shared.Messages.Commands
{
    internal sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public CommandDispatcher(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        public async Task SendAsync<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            using var scope = _scopeFactory.CreateScope();
            var handler = scope.ServiceProvider.GetService<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command);
        }
    }
}