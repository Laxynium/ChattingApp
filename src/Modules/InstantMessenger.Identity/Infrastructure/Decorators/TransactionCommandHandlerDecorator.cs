using System.Threading.Tasks;
using InstantMessenger.Identity.Infrastructure.Database;
using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Decorators.UoW;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Identity.Infrastructure.Decorators
{
    [Decorator]
    public class TransactionCommandHandlerDecorator<TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
    {
        private readonly ICommandHandler<TCommand> _innerHandler;
        private readonly UnitOfWork<IdentityContext> _unitOfWork;
        private readonly IIntegrationEventsPublisher _integrationEventsPublisher;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> innerHandler, UnitOfWork<IdentityContext> unitOfWork, IIntegrationEventsPublisher integrationEventsPublisher)
        {
            _innerHandler = innerHandler;
            _unitOfWork = unitOfWork;
            _integrationEventsPublisher = integrationEventsPublisher;
        }

        public async Task HandleAsync(TCommand command)
        {
            await _innerHandler.HandleAsync(command);

            await _unitOfWork.Commit();

            await _integrationEventsPublisher.PublishAsync();
        }
    }
}