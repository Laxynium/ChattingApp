using System.Threading.Tasks;
using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Modules;

namespace InstantMessenger.Shared.UoW
{
    [Decorator]
    public class TransactionCommandHandlerDecorator<TModule, TCommand> : ICommandHandler<TCommand>
        where TCommand : class, ICommand
        where TModule : IModule
    {
        private readonly ICommandHandler<TCommand> _decoratedCommandHandler;
        private readonly IUnitOfWork<TModule> _unitOfWork;
        private readonly IIntegrationEventsPublisher<TModule> _integrationEventsPublisher;

        public TransactionCommandHandlerDecorator(ICommandHandler<TCommand> decoratedCommandHandler,
            IUnitOfWork<TModule> unitOfWork, IIntegrationEventsPublisher<TModule> integrationEventsPublisher)
        {
            _decoratedCommandHandler = decoratedCommandHandler;
            _unitOfWork = unitOfWork;
            _integrationEventsPublisher = integrationEventsPublisher;
        }

        public async Task HandleAsync(TCommand command)
        {
            await _decoratedCommandHandler.HandleAsync(command);
            await _unitOfWork.Commit();
            await _integrationEventsPublisher.PublishAsync();
        }
    }
}