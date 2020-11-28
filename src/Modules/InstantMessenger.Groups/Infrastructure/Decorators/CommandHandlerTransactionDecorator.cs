using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Decorators;
using Microsoft.Extensions.Logging;

namespace InstantMessenger.Groups.Infrastructure.Decorators
{
    public class CommandHandlerTransactionDecorator<TCommand> 
        : TransactionCommandHandlerDecorator<TCommand, GroupsContext>
        where TCommand:class, ICommand
    {
        public CommandHandlerTransactionDecorator(ICommandHandler<TCommand> innerHandler, ILogger<CommandHandlerTransactionDecorator<TCommand>> logger, GroupsContext context) 
            : base(innerHandler, logger, context)
        {
        }
    }
}