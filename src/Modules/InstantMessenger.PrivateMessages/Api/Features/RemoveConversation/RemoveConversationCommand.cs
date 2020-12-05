using System;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Domain.Events;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.MessageBrokers;

namespace InstantMessenger.PrivateMessages.Api.Features.RemoveConversation
{
    public class RemoveConversationCommand : ICommand
    {
        public Guid ConversationId { get; }

        public RemoveConversationCommand(Guid conversationId)
        {
            ConversationId = conversationId;
        }
    }

    internal sealed class RemoveConversationCommandHandler : ICommandHandler<RemoveConversationCommand>
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMessageBroker _messageBroker;

        public RemoveConversationCommandHandler(IConversationRepository conversationRepository, IUnitOfWork unitOfWork, IMessageBroker messageBroker)
        {
            _conversationRepository = conversationRepository;
            _unitOfWork = unitOfWork;
            _messageBroker = messageBroker;
        }
        public async Task HandleAsync(RemoveConversationCommand command)
        {
            var conversation = await _conversationRepository.GetAsync(new ConversationId(command.ConversationId));
            if (conversation is null)
                return;

            await _conversationRepository.RemoveAsync(conversation);
            await _unitOfWork.Commit();
            await _messageBroker.PublishAsync(
                new ConversationRemovedEvent(conversation.Id, conversation.FirstParticipant, conversation.SecondParticipant)
            );
        }
    }
}