using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Domain.Exceptions;
using InstantMessenger.Shared.Commands;
using NodaTime;

namespace InstantMessenger.PrivateMessages.Api.Features.MarkMessageAsRead
{
    internal sealed class MarkMessageAsReadHandler : ICommandHandler<MarkMessageAsReadCommand>
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IClock _clock;

        public MarkMessageAsReadHandler(IMessageRepository messageRepository, 
            IUnitOfWork unitOfWork,
            IClock clock)
        {
            _messageRepository = messageRepository;
            _unitOfWork = unitOfWork;
            _clock = clock;
        }

        public async Task HandleAsync(MarkMessageAsReadCommand command)
        {
            var message = await _messageRepository.GetAsync(MessageId.From(command.MessageId)) ?? throw new MessageNotFoundException();

            message.MarkAsRead(new Participant(command.ParticipantId), _clock);

            await _unitOfWork.Commit();
        }
    }
}