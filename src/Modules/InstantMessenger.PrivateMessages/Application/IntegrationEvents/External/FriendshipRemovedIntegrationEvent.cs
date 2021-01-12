using System;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Application.Features.RemoveConversation;
using InstantMessenger.Shared.IntegrationEvents;
using InstantMessenger.Shared.Messages.Commands;

namespace InstantMessenger.PrivateMessages.Application.IntegrationEvents.External
{
    internal  sealed class FriendshipRemovedIntegrationEvent : IIntegrationEvent
    {
        public Guid FriendshipId { get; }
        public Guid FirstPerson { get; }
        public Guid SecondPerson { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipRemovedIntegrationEvent(Guid friendshipId, Guid firstPerson, Guid secondPerson, DateTimeOffset createdAt)
        {
            FriendshipId = friendshipId;
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
            CreatedAt = createdAt;
        }
    }

    internal sealed class FriendshipRemovedEventHandler : IIntegrationEventHandler<FriendshipRemovedIntegrationEvent>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public FriendshipRemovedEventHandler(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        public async Task HandleAsync(FriendshipRemovedIntegrationEvent @event)
        {
            await _commandDispatcher.SendAsync(new RemoveConversationCommand(@event.FriendshipId));
        }
    }
}