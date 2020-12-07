using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Channel.AddChannel;
using InstantMessenger.Groups.Api.Features.Channel.RemoveChannel;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Shared.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.Groups.Api
{
    [ApiController]
    [Authorize]
    [Route("api/groups/{groupId}/channels")]
    public class ChannelsController : ControllerBase
    {
        private readonly GroupsModuleFacade _facade;

        public ChannelsController(GroupsModuleFacade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public async Task<IActionResult> CreateChannel(CreateChannelApiRequest request)
        {
            await _facade.SendAsync(new CreateChannelCommand(User.GetUserId(), request.GroupId,request.ChannelId, request.ChannelName));
            return Ok();
        }

        [HttpDelete("{channelId}")]
        public async Task<IActionResult> DeleteChannel(Guid groupId, Guid channelId)
        {
            await _facade.SendAsync(new RemoveChannelCommand(User.GetUserId(), groupId, channelId));
            return Ok();
        }

        [HttpGet("{channelId}")]
        public async Task<IActionResult> GetChannel(Guid groupId, Guid channelId)
        {
            var result = await _facade.QueryAsync(new GetGroupChannelQuery(User.GetUserId(), groupId,channelId));
            return Ok(result);
        }       
        
        [HttpGet]
        public async Task<IActionResult> GetChannels(Guid groupId)
        {
            var result = await _facade.QueryAsync(new GetGroupChannelsQuery(User.GetUserId(), groupId));
            return Ok(result);
        }
    }
}