using System;
using System.Threading.Tasks;
using InstantMessenger.PrivateMessages.Api.Features.MarkMessageAsRead;
using InstantMessenger.PrivateMessages.Api.Features.SendMessage;
using InstantMessenger.PrivateMessages.Api.Queries;
using InstantMessenger.Shared.Messages.Commands;
using InstantMessenger.Shared.Messages.Queries;
using InstantMessenger.Shared.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InstantMessenger.PrivateMessages.Api
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PrivateMessagesController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;

        public PrivateMessagesController(ICommandDispatcher commandDispatcher,IQueryDispatcher queryDispatcher)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet("conversations/latest")]
        public async Task<IActionResult> GetLatestConversations(int number=10)
        {
            var result = await _queryDispatcher.QueryAsync(new GetLatestConversationsQuery(User.GetUserId(), number));
            return Ok(result);
        }

        [HttpGet("conversations")]
        public async Task<IActionResult> GetConversations()
        {
            var result = await _queryDispatcher.QueryAsync(new GetConversationsQuery(User.GetUserId()));
            return Ok(result);
        }

        [HttpGet("conversations/{conversationId}")]
        public async Task<IActionResult> GetConversations(Guid conversationId)
        {
            var result = await _queryDispatcher.QueryAsync(new GetConversationQuery(conversationId));
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageApiRequest request)
        {
            var command = new SendMessageCommand(Guid.NewGuid(), request.ConversationId, User.GetUserId(), request.Text);
            await _commandDispatcher.SendAsync(command);

            var result = await _queryDispatcher.QueryAsync(new GetMessageQuery(command.MessageId));
            return Ok(result);
        }

        [HttpPost("mark-as-read")]
        public async Task<IActionResult> MarkAsRead(MarkMessageAsReadApiRequest request)
        {
            await _commandDispatcher.SendAsync(new MarkMessageAsReadCommand(request.MessageId, User.GetUserId()));
            return Ok();
        }

        [HttpGet("{conversationId}")]
        public async Task<IActionResult> GetMessages(Guid conversationId)
        {
            var result = await _queryDispatcher.QueryAsync(new GetMessagesQuery(User.GetUserId(), conversationId));
            return Ok(result);
        }
    }
}