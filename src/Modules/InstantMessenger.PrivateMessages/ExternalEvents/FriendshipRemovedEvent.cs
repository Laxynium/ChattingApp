using System;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api.Features.RemoveConversation;
using InstantMessenger.Shared.Commands;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.PrivateMessages.ExternalEvents
{
    internal  sealed class FriendshipRemovedEvent : IEvent
    {
        public Guid FriendshipId { get; }
        public Guid FirstPerson { get; }
        public Guid SecondPerson { get; }
        public DateTimeOffset CreatedAt { get; }

        public FriendshipRemovedEvent(Guid friendshipId, Guid firstPerson, Guid secondPerson, DateTimeOffset createdAt)
        {
            FriendshipId = friendshipId;
            FirstPerson = firstPerson;
            SecondPerson = secondPerson;
            CreatedAt = createdAt;
        }
    }

    internal sealed class FriendshipRemovedEventHandler : IEventHandler<FriendshipRemovedEvent>
    {
        private readonly ICommandDispatcher _commandDispatcher;

        public FriendshipRemovedEventHandler(ICommandDispatcher commandDispatcher)
        {
            _commandDispatcher = commandDispatcher;
        }
        public async Task HandleAsync(FriendshipRemovedEvent @event)
        {
            await _commandDispatcher.SendAsync(new RemoveConversationCommand(@event.FriendshipId));
        }
    }

}