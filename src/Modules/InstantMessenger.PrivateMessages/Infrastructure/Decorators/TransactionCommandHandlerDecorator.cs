using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.UoW;

namespace InstantMessenger.PrivateMessages.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class
        TransactionCommandHandlerDecorator<TCommand> : TransactionCommandHandlerDecorator<PrivateMessagesModule,
            TCommand>
        where TCommand : class, ICommand
    {
        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> decoratedCommandHandler,
            IUnitOfWork<PrivateMessagesModule> unitOfWork, Shared.UoW.IIntegrationEventsPublisher<PrivateMessagesModule> integrationEventsPublisher) : base(
            decoratedCommandHandler, unitOfWork, integrationEventsPublisher)
        {
        }
    }
}