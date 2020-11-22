using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Members.Add;
using InstantMessenger.Groups.Domain;
using InstantMessenger.Shared.Events;

namespace InstantMessenger.Groups.Api.Features.Invitations.JoinGroup
{
    internal sealed class InvitationUsedEventHandler : IEventHandler<InvitationUsedEvent>
    {
        private readonly GroupsModuleFacade _facade;
        

        public InvitationUsedEventHandler(GroupsModuleFacade facade)
        {
            _facade = facade;
        
        }

        public async Task HandleAsync(InvitationUsedEvent @event)
        {
            await _facade.SendAsync(new AddGroupMember(@event.GroupId, @event.UserId));
        }
    }
}