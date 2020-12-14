using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Channel.AddChannel;
using InstantMessenger.Groups.Api.Features.Channel.RemoveChannel;
using InstantMessenger.Groups.Api.Features.Channel.UpdateRolePermissionOverride;
using InstantMessenger.Groups.Api.Features.Messages.SendMessage;
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

        [HttpPost("{channelId}/messages")]
        public async Task<IActionResult> SendMessage(SendMessageApiRequest request)
        {
            await _facade.SendAsync(new SendMessageCommand(User.GetUserId(), request.GroupId, request.ChannelId, request.MessageId, request.Content));

            var message = await _facade.QueryAsync(new GetMessageQuery(User.GetUserId(), request.GroupId,
                request.ChannelId, request.MessageId));

            return Ok(message);
        }

        [HttpPut("{channelId}/permission-overrides/role")]
        public async Task<IActionResult> UpdateRolePermissionOverrides(UpdateRolePermissionOverrideApiRequest request)
        {
            await _facade.SendAsync(
                new UpdateRolePermissionOverrideCommand(
                    User.GetUserId(),
                    request.GroupId,
                    request.ChannelId,
                    request.RoleId,
                    request.Overrides
                )
            );
            return Ok();
        }

        [HttpGet("{channelId}/permission-overrides/role/{roleId}")]
        public async Task<IActionResult> GetRolePermissionOverrides(Guid groupId, Guid channelId, Guid roleId)
        {
            var result = await _facade.QueryAsync(new GetChannelRolePermissionOverridesQuery(groupId, channelId, roleId));
            return Ok(result);
        }

        [HttpGet("{channelId}/permission-overrides/role")]
        public async Task<IActionResult> GetAvailablePermissionOverrides(Guid groupId, Guid channelId)
        {
            var result = await _facade.QueryAsync(new GetAvailablePermissionOverridesQuery());
            return Ok(result);
        }


        [HttpGet("{channelId}/messages")]
        public async Task<IActionResult> GetMessages(Guid groupId, Guid channelId)
        {
            var result = await _facade.QueryAsync(new GetMessagesQuery(User.GetUserId(), groupId, channelId));
            return Ok(result);
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