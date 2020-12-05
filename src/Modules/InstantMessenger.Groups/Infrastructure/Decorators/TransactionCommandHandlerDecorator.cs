using System.Threading.Tasks;
using InstantMessenger.Groups.Infrastructure.Database;
using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Decorators.UoW;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Groups.Infrastructure.Decorators
{
    [Decorator]
    public class TransactionCommandHandlerDecorator<TCommand>: ICommandHandler<TCommand>
        where TCommand:class, ICommand
    {
        private readonly ICommandHandler<TCommand> _innerHandler;
        private readonly UnitOfWork<GroupsContext> _unitOfWork;
        private readonly IntegrationEventsPublisher _integrationEventsPublisher;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> innerHandler, UnitOfWork<GroupsContext> unitOfWork, IntegrationEventsPublisher integrationEventsPublisher)
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