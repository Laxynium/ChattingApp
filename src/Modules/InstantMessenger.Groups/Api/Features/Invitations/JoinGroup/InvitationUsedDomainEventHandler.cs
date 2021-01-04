using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Members.Add;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Shared.Messages.Events;

namespace InstantMessenger.Groups.Api.Features.Invitations.JoinGroup
{
    internal sealed class InvitationUsedEventHandler : IDomainEventHandler<InvitationUsedDomainEvent>
    {
        private readonly GroupsModuleFacade _facade;

        public InvitationUsedEventHandler(GroupsModuleFacade facade)
        {
            _facade = facade;
        
        }

        public async Task HandleAsync(InvitationUsedDomainEvent @event)
        {
            await _facade.SendAsync(new AddGroupMember(@event.GroupId, @event.UserId));
        }
    }
}