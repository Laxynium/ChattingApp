using System;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Domain.Events;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.Shared.MessageBrokers;
using InstantMessenger.Shared.Messages.Commands;

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

        public RemoveConversationCommandHandler(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }
        public async Task HandleAsync(RemoveConversationCommand command)
        {
            var conversation = await _conversationRepository.GetAsync(new ConversationId(command.ConversationId));
            if (conversation is null)
                return;

            conversation.Remove();

            await _conversationRepository.RemoveAsync(conversation);
        }
    }
}