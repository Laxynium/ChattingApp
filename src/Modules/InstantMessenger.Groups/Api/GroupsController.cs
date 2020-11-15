using System;
using System.Linq;
using System.Threading.Tasks;
using InstantMessenger.Groups.Api.Features.Group.Create;
using InstantMessenger.Groups.Api.Queries;
using InstantMessenger.Shared.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.Groups.Api
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class GroupsController : ControllerBase
    {
        private readonly GroupsModuleFacade _facade;

        public GroupsController(GroupsModuleFacade facade)
        {
            _facade = facade;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]CreateGroupApiRequest request)
        {
            await _facade.SendAsync(new CreateGroupCommand(User.GetUserId(),request.GroupId, request.OwnerId, request.GroupName));
            return Ok();
        }

        [HttpGet("{groupId}")]
        public async Task<IActionResult> GetGroup([FromRoute]Guid groupId)
        {
            var result = await _facade.QueryAsync(new GetGroupsQuery(groupId));
            return Ok(result.FirstOrDefault());
        }

        [HttpGet("{groupId}/owner")]
        public async Task<IActionResult> GetOwner([FromRoute]Guid groupId)
        {
            var result = await _facade.QueryAsync(new GetMembersQuery(groupId,true));
            return Ok(result.FirstOrDefault());
        }
    }
}