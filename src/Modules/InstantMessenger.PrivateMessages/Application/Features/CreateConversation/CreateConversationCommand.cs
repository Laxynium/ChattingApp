using System;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Domain;
using InstantMessenger.PrivateMessages.Domain.Entities;
using InstantMessenger.PrivateMessages.Domain.ValueObjects;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.PrivateMessages.Application.Features.CreateConversation
{
    public class CreateConversationCommand : ICommand
    {
        public Guid ConversationId { get; }
        public Guid FirstParticipant { get; }
        public Guid SecondParticipant { get; }

        public CreateConversationCommand(Guid conversationId, Guid firstParticipant, Guid secondParticipant)
        {
            ConversationId = conversationId;
            FirstParticipant = firstParticipant;
            SecondParticipant = secondParticipant;
        }
    }

    internal sealed class CreateConversationCommandHandler : ICommandHandler<CreateConversationCommand>
    {
        private readonly IConversationRepository _conversationRepository;

        public CreateConversationCommandHandler(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }
        public async Task HandleAsync(CreateConversationCommand command)
        {

            if (await _conversationRepository.ExistsAsync(new Participant(command.FirstParticipant), new Participant(command.SecondParticipant)))
            {
                return;
            }
            var conversation = Conversation.Create(new ConversationId(command.ConversationId), new Participant(command.FirstParticipant), new Participant(command.SecondParticipant));

            await _conversationRepository.AddAsync(conversation);
        }
    }
}