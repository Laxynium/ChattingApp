using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Domain.Events;
using InstantMessenger.PrivateMessages.Domain.Exceptions;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MessageBrokers;
using NodaTime;

namespace InstantMessenger.PrivateMessages.Api.Features.MarkMessageAsRead
{
    internal sealed class MarkMessageAsReadHandler : ICommandHandler<MarkMessageAsReadCommand>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;
        private readonly IMessageBroker _messageBroker;

        public MarkMessageAsReadHandler(IMessageRepository messageRepository, 
            IUnitOfWork unitOfWork,
            IClock clock,
            IMessageBroker messageBroker)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
            _messageBroker = messageBroker;
        }

        public async Task HandleAsync(MarkMessageAsReadCommand command)
        {
            var message = await _messageRepository.GetAsync(MessageId.From(command.MessageId)) ?? throw new MessageNotFoundException();

            message.MarkAsRead(new Participant(command.ParticipantId), _clock);

            await _unitOfWork.Commit();

            await _messageBroker.PublishAsync(new MessageMarkedAsReadEvent(message.Id.Value, message.ConversationId.Value, message.From,message.To,message.ReadAt.Value));
        }
    }
}