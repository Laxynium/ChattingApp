using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Infrastructure.Database;
using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Decorators.UoW;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.PrivateMessages.Infrastructure.Decorators
{
    [Decorator]
    public class TransactionCommandHandlerDecorator<TCommand>: ICommandHandler<TCommand>
        where TCommand:class, ICommand
    {
        private readonly ICommandHandler<TCommand> _innerHandler;
        private readonly UnitOfWork<PrivateMessagesContext> _unitOfWork;
        private readonly IntegrationEventsPublisher _integrationEventsPublisher;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> innerHandler, UnitOfWork<PrivateMessagesContext> unitOfWork, IntegrationEventsPublisher integrationEventsPublisher)
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