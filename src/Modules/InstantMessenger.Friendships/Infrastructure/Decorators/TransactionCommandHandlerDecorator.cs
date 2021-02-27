using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Outbox;
using InstantMessenger.Shared.UoW;

namespace InstantMessenger.Friendships.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class
        CommandHandlerTransactionDecorator<TCommand> : TransactionCommandHandlerDecorator<FriendshipsModule, TCommand>
        where TCommand : class, ICommand
    {
        public CommandHandlerTransactionDecorator(ICommandHandler<TCommand> decoratedCommandHandler,
            IUnitOfWork<FriendshipsModule> unitOfWork,
            IIntegrationEventsPublisher<FriendshipsModule> integrationEventsPublisher) : base(
            decoratedCommandHandler, unitOfWork, integrationEventsPublisher)
        {
        }
    }
}