using System;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api.Features.CreateConversation;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.PrivateMessages.ExternalIntegrationEvents
{
    public class FriendshipCreatedIntegrationEvent : IIntegrationEvent
    {
        public Guid FriendshipId { get; }
        public Guid FirstPerson { get; }
        public Guid SecondPerson { get; }

        public FriendshipCreatedIntegrationEvent(Guid friendshipId, Guid firstPerson, Guid secondPerson)
        {
            FriendshipId = friendshipId;
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
        }
    }

    internal sealed class FriendshipCreatedHandler : IIntegrationEventHandler<FriendshipCreatedIntegrationEvent>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public FriendshipCreatedHandler(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        public async Task HandleAsync(FriendshipCreatedIntegrationEvent @event)
        {
            await _commandDispatcher.SendAsync(new CreateConversationCommand(@event.FriendshipId, @event.FirstPerson, @event.SecondPerson));
        }
    }
}