using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.UoW;

namespace InstantMessenger.Identity.Infrastructure.Decorators
{
    [Decorator]
    internal sealed class
        TransactionCommandHandlerDecorator<TCommand> : TransactionCommandHandlerDecorator<IdentityModule, TCommand>
        where TCommand : class, ICommand
    {
        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> decoratedCommandHandler,
            IUnitOfWork<IdentityModule> unitOfWork, Shared.UoW.IIntegrationEventsPublisher<IdentityModule> integrationEventsPublisher) : base(
            decoratedCommandHandler, unitOfWork, integrationEventsPublisher)
        {
        }
    }
}