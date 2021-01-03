using System.Threading.Tasks;
using InstantMessenger.Friendships.Infrastructure.Database;
using InstantMessenger.Shared.Decorators;
using InstantMessenger.Shared.Decorators.UoW;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.Friendships.Infrastructure.Decorators
{
    [Decorator]
    public class CommandHandlerTransactionDecorator<TCommand>: ICommandHandler<TCommand>
        where TCommand:class, ICommand
    {
        private readonly ICommandHandler<TCommand> _innerHandler;
        private readonly UnitOfWork<FriendshipsContext> _unitOfWork;
        private readonly IIntegrationEventsPublisher<FriendshipsContext> _integrationEventsPublisher;

        public CommandHandlerTransactionDecorator(ICommandHandler<TCommand> innerHandler, UnitOfWork<FriendshipsContext> unitOfWork, IIntegrationEventsPublisher<FriendshipsContext> integrationEventsPublisher)
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