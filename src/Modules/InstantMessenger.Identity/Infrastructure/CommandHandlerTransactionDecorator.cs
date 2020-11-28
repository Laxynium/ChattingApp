using InstantMessenger.Identity.Infrastructure.Database;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Decorators;
using Microsoft.Extensions.Logging;

namespace InstantMessenger.Identity.Infrastructure
{
    public class CommandHandlerTransactionDecorator<TCommand> : TransactionCommandHandlerDecorator<TCommand,IdentityContext>
    where TCommand : class, ICommand
    {
        public CommandHandlerTransactionDecorator(ICommandHandler<TCommand> innerHandler, ILogger<CommandHandlerTransactionDecorator<TCommand>> logger, IdentityContext context) : base(innerHandler, logger, context)
        {
        }
    }
}