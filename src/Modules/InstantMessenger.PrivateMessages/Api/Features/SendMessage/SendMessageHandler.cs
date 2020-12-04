using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Domain.Events;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MessageBrokers;
using NodaTime;

namespace InstantMessenger.PrivateMessages.Api.Features.SendMessage
{
    internal sealed class SendMessageHandler : ICommandHandler<SendMessageCommand>
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;
        private readonly IMessageBroker _broker;

        public SendMessageHandler(
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository,
            IUnitOfWork unitOfWork,
            IClock clock,
            IMessageBroker broker)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
            _broker = broker;
        }
        public async Task HandleAsync(SendMessageCommand command)
        {
            var conversation = await _conversationRepository.GetAsync(new ConversationId(command.ConversationId));

            var message = conversation.Send(MessageId.From(command.MessageId),new MessageBody(command.Text), new Participant(command.SenderId), _clock);

            await _messageRepository.AddAsync(message);
            await _unitOfWork.Commit();

            await _broker.PublishAsync(
                new MessageCreatedEvent(
                    message.Id.Value,
                    message.ConversationId.Value,
                    message.From.Id,
                    message.To.Id,
                    message.Body.TextContent,
                    message.CreatedAt
                )
            );
        }
    }
}