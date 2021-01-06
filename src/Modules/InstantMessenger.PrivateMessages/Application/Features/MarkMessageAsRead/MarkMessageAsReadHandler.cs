using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Domain.Exceptions;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;
using NodaTime;

namespace InstantMessenger.PrivateMessages.Application.Features.MarkMessageAsRead
{
    internal sealed class MarkMessageAsReadHandler : ICommandHandler<MarkMessageAsReadCommand>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IClock _clock;

        public MarkMessageAsReadHandler(IMessageRepository messageRepository, 
            IClock clock)
        {
            _messageRepository = messageRepository;
            _clock = clock;
        }

        public async Task HandleAsync(MarkMessageAsReadCommand command)
        {
            var message = await _messageRepository.GetAsync(MessageId.From(command.MessageId)) ?? throw new MessageNotFoundException();

            message.MarkAsRead(new Participant(command.ParticipantId), _clock);
        }
    }
}