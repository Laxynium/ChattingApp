using System.Threading.Tasks;
using InstantMessenger.Shared.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace InstantMessenger.Shared.Decorators
{
    public abstract class TransactionCommandHandlerDecorator<TCommand, TDbContext> : ICommandHandler<TCommand> 
        where TCommand:class,ICommand
        where TDbContext: DbContext
    {
        private readonly ICommandHandler<TCommand> _innerHandler;
        private readonly ILogger _logger;
        private readonly TDbContext _context;

        protected TransactionCommandHandlerDecorator(ICommandHandler<TCommand> innerHandler, ILogger logger, TDbContext context)
        {
            _innerHandler = innerHandler;
            _logger = logger;
            _context = context;
        }
        public async Task HandleAsync(TCommand command)
        {
            _logger.LogInformation($"Beginning transaction for command[{command}]...");

            await _innerHandler.HandleAsync(command);

            _logger.LogInformation($"Trying to save transaction of command[{command}]...");
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Transaction saved of command[{command}]");
        }
    }
}