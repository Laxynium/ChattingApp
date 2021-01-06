using System;
using System.Threading.Tasks;
using InstantMessenger.Groups.Application.Features.Channel.AddChannel;
using InstantMessenger.Groups.Application.Features.Channel.EditChannel;
using InstantMessenger.Groups.Application.Features.Channel.RemoveChannel;
using InstantMessenger.Groups.Application.Features.Channel.UpdateMemberPermissionOverride;
using InstantMessenger.Groups.Application.Features.Channel.UpdateRolePermissionOverride;
using InstantMessenger.Groups.Application.Features.Messages.SendMessage;
using InstantMessenger.Groups.Application.Queries;
using InstantMessenger.Shared.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.Groups.Application.Controllers
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
        
        [HttpPut("{channelId}")]
        public async Task<IActionResult> EditChannel(RenameChannelApiRequest request)
        {
            await _facade.SendAsync(new RenameChannelCommand(User.GetUserId(), request.GroupId, request.ChannelId,request.Name));
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

        [HttpPut("{channelId}/permission-overrides/member")]
        public async Task<IActionResult> UpdateMemberPermissionOverrides(UpdateMemberPermissionOverrideApiRequest request)
        {
            await _facade.SendAsync(
                new UpdateMemberPermissionOverrideCommand(
                    User.GetUserId(),
                    request.GroupId,
                    request.ChannelId,
                    request.MemberUserId,
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

        [HttpGet("{channelId}/permission-overrides/member/{memberUserId}")]
        public async Task<IActionResult> GetMemberPermissionOverrides(Guid groupId, Guid channelId, Guid memberUserId)
        {
            var result = await _facade.QueryAsync(new GetChannelMemberPermissionOverridesQuery(groupId, channelId, memberUserId));
            return Ok(result);
        }

        [HttpGet("{channelId}/permission-overrides")]
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