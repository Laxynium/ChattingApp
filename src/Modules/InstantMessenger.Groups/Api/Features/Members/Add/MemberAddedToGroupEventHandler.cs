using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Hubs;
using InstantMessenger.Groups.Api.IntegrationEvents;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Shared.IntegrationEvents;
using Microsoft.AspNetCore.SignalR;

namespace InstantMessenger.Groups.Api.Features.Members.Add
{
    //public class MemberAddedToGroupEventHandler : IIntegrationEventHandler<MemberAddedToGroupEvent>
    //{
    //    private readonly IHubContext<GroupsHub, IGroupsHubContract> _hubContext;

    //    public MemberAddedToGroupEventHandler(IHubContext<GroupsHub, IGroupsHubContract> hubContext)
    //    {
    //        _hubContext = hubContext;
    //    }
    //    public async Task HandleAsync(MemberAddedToGroupEvent @event)
    //    {
    //        await _hubContext.Clients.Users().OnMemberAddedToGroup(
    //            new MemberDto
    //            {
    //                UserId = @event.
    //            }
    //        );
    //    }
    //}
}