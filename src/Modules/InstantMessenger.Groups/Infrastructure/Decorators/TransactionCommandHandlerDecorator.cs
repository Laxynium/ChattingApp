using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.UoW;

namespace InstantMessenger.Groups.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class
        TransactionCommandHandlerDecorator<TCommand> : TransactionCommandHandlerDecorator<GroupsModule, TCommand>
        where TCommand : class, ICommand
    {
        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> decoratedCommandHandler,
            IUnitOfWork<GroupsModule> unitOfWork,
            IIntegrationEventsPublisher<GroupsModule> integrationEventsPublisher) : base(
            decoratedCommandHandler, unitOfWork, integrationEventsPublisher)
        {
        }
    }
}