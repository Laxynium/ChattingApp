using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.PrivateMessages.ExternalEvents
{
    internal sealed class FriendshipCreatedHandler : IEventHandler<FriendshipCreatedEvent>
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FriendshipCreatedHandler(IConversationRepository conversationRepository, IUnitOfWork unitOfWork)
        {
            _conversationRepository = conversationRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task HandleAsync(FriendshipCreatedEvent @event)
        {
            var (firstParticipant, secondParticipant) =
                (new Participant(@event.FirstPerson), new Participant(@event.SecondPerson));

            if (await _conversationRepository.ExistsAsync(firstParticipant, secondParticipant))
            {
                return;
            }
            var conversation = Conversation.Create(firstParticipant, secondParticipant);

            await _conversationRepository.AddAsync(conversation);
            await _unitOfWork.Commit();
        }
    }
}