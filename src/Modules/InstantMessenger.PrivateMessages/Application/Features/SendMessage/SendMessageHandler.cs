using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;
using NodaTime;

namespace InstantMessenger.PrivateMessages.Application.Features.SendMessage
{
    internal sealed class SendMessageHandler : ICommandHandler<SendMessageCommand>
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IClock _clock;

        public SendMessageHandler(
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository,
            IClock clock)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _clock = clock;
        }
        public async Task HandleAsync(SendMessageCommand command)
        {
            var conversation = await _conversationRepository.GetAsync(new ConversationId(command.ConversationId));

            var message = conversation.Send(MessageId.From(command.MessageId),new MessageBody(command.Text), new Participant(command.SenderId), _clock);

            await _messageRepository.AddAsync(message);
        }
    }
}