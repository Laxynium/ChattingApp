using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.Shared.Commands;
using NodaTime;

namespace InstantMessenger.PrivateMessages.Api.Features.SendMessage
{
    internal sealed class SendMessageHandler : ICommandHandler<SendMessageCommand>
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public SendMessageHandler(
            IConversationRepository conversationRepository,
            IMessageRepository messageRepository,
            IUnitOfWork unitOfWork,
            IClock clock)
        {
            _conversationRepository = conversationRepository;
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }
        public async Task HandleAsync(SendMessageCommand command)
        {
            var conversation = await _conversationRepository.GetAsync(new ConversationId(command.ConversationId));

            var message = conversation.Send(new MessageBody(command.Text), new Participant(command.SenderId), _clock);

            await _messageRepository.AddAsync(message);
            await _unitOfWork.Commit();
        }
    }
}