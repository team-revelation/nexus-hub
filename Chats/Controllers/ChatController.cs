using System;
using System.Threading.Tasks;
using Authentication;
using Chats.Commands;
using Chats.Commands.Chats;
using Chats.Commands.Messages;
using Chats.Notifications;
using Chats.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Types.Chats;

namespace Chats.Controllers
{
    [Produces("application/json")]
    [Route("api/chats")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public ChatController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get every chat in the database.
        /// </summary>
        /// <returns></returns>
        [KeyAuth]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var res = await _mediator.Send(new GetChatsQuery());
            res = await _mediator.Send(new FillChatsCommand(res));
            
            return Ok(res);
        }

        /// <summary>
        /// Get all chats that have a user as member.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpGet("users/{uuid:guid}")]
        public async Task<IActionResult> GetWithUser(Guid uuid)
        {
            var res = await _mediator.Send(new GetChatQuery(uuid));
            res = await _mediator.Send(new FillChatCommand(res));
            
            return Ok(res);
        }

        /// <summary>
        /// Get a chat with a specific uuid.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpGet("{uuid:guid}")]
        public async Task<IActionResult> Get(Guid uuid)
        {
            var res = await _mediator.Send(new GetChatQuery(uuid));
            res = await _mediator.Send(new FillChatCommand(res));
            
            return Ok(res);
        }

        /// <summary>
        /// Join an already existing chat.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPatch("{uuid:guid}/join")]
        public async Task<IActionResult> Join(Guid uuid, [FromBody] JoinRequest request)
        {
            var res = await _mediator.Send(new JoinChatCommand(request.UserUuids, uuid));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// Leave a chat.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="userUuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPatch("users/{userUuid:guid}/{uuid:guid}/leave")]
        public async Task<IActionResult> Leave(Guid uuid, Guid userUuid)
        {
            var res = await _mediator.Send(new LeaveChatCommand(uuid, userUuid));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new LeaveChatNotification(res, userUuid));
            return Accepted(res);
        }

        /// <summary>
        /// Send a message to everyone in a chat.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("{uuid:guid}/send")]
        public async Task<IActionResult> Send(Guid uuid, [FromBody] Message message)
        {
            var res = await _mediator.Send(new SendMessageCommand(uuid, message));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// Vote for a poll that is attached to a message.
        /// </summary>
        /// <param name="userUuid"></param>
        /// <param name="chatUuid"></param>
        /// <param name="messageUuid"></param>
        /// <param name="pollUuid"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("users/{userUuid:guid}/{chatUuid:guid}/{messageUuid:guid}/{pollUuid:guid}/vote/{index:int}")]
        public async Task<IActionResult> Vote(Guid userUuid, Guid chatUuid, Guid messageUuid, Guid pollUuid, int index)
        {
            var res = await _mediator.Send(new VoteForOptionCommand(userUuid, chatUuid, messageUuid, pollUuid, index));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// Edit an already existing message.
        /// </summary>
        /// <param name="chatUuid"></param>
        /// <param name="messageUuid"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("{chatUuid:guid}/{messageUuid:guid}/edit")]
        public async Task<IActionResult> Edit(Guid chatUuid, Guid messageUuid, [FromBody] Message message)
        {
            var res = await _mediator.Send(new EditMessageCommand(messageUuid, chatUuid, message));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// Edit an already existing poll.
        /// </summary>
        /// <param name="chatUuid"></param>
        /// <param name="messageUuid"></param>
        /// <param name="pollUuid"></param>
        /// <param name="poll"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("{chatUuid:guid}/{messageUuid:guid}/polls/{pollUuid:guid}/edit")]
        public async Task<IActionResult> Edit(Guid chatUuid, Guid messageUuid, Guid pollUuid, [FromBody] Poll poll)
        {
            var res = await _mediator.Send(new EditPollCommand(pollUuid, messageUuid, chatUuid, poll));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// Edit an already existing checklist.
        /// </summary>
        /// <param name="chatUuid"></param>
        /// <param name="messageUuid"></param>
        /// <param name="checklistUuid"></param>
        /// <param name="checklist"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("{chatUuid:guid}/{messageUuid:guid}/checklists/{checklistUuid:guid}/edit")]
        public async Task<IActionResult> Edit(Guid chatUuid, Guid messageUuid, Guid checklistUuid, [FromBody] Checklist checklist)
        {
            var res = await _mediator.Send(new EditChecklistCommand(checklistUuid, messageUuid, chatUuid, checklist));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// React to an already existing message.
        /// </summary>
        /// <param name="userUuid"></param>
        /// <param name="chatUuid"></param>
        /// <param name="messageUuid"></param>
        /// <param name="emoji"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("users/{userUuid:guid}/{chatUuid:guid}/{messageUuid:guid}/react")]
        public async Task<IActionResult> React(Guid userUuid, Guid chatUuid, Guid messageUuid, [FromBody] Emoticon emoji)
        {
            var res = await _mediator.Send(new ReactToMessageCommand(userUuid, messageUuid, chatUuid, emoji));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// Cancel sending the message.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="messageUuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpDelete("{uuid:guid}/{messageUuid:guid}")]
        public async Task<IActionResult> Cancel(Guid uuid, Guid messageUuid)
        {
            var res = await _mediator.Send(new RemoveMessageCommand(uuid, messageUuid));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// Read all messages in a chat.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="userUuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("users/{userUuid:guid}/{uuid:guid}/read")]
        public async Task<IActionResult> Read(Guid uuid, Guid userUuid)
        {
            var res = await _mediator.Send(new ReadChatCommand(uuid, userUuid));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// Mark message in a chat as unread.
        /// </summary>
        /// <param name="chatUuid"></param>
        /// <param name="messageUuid"></param>
        /// <param name="userUuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("users/{userUuid:guid}/{chatUuid:guid}/{messageUuid:guid}/unread")]
        public async Task<IActionResult> Unread(Guid chatUuid, Guid messageUuid, Guid userUuid)
        {
            var res = await _mediator.Send(new UnreadMessageCommand(messageUuid, userUuid, chatUuid));
            res = await _mediator.Send(new FillChatCommand(res));

            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// Create a new chat.
        /// </summary>
        /// <param name="chat"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateChatRequest chat)
        {
            var res = await _mediator.Send(new CreateChatCommand(chat.Chat, chat.MemberUuids));
            await _mediator.Publish(new NewChatNotification(res));
            return res is null ? Problem() : Accepted(res);
        }

        /// <summary>
        /// Update an already existing chat.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="chat"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("{uuid:guid}")]
        public async Task<IActionResult> Put(Guid uuid, [FromBody] Chat chat)
        {
            var res = await _mediator.Send(new UpdateChatCommand(uuid, chat));
            res = await _mediator.Send(new FillChatCommand(res));
            
            await _mediator.Publish(new UpdateChatNotification(res));
            return Accepted(res);
        }

        /// <summary>
        /// Delete an already existing chat.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpDelete("{uuid:guid}")]
        public async Task<IActionResult> Delete(Guid uuid)
        {
            var chat = await _mediator.Send(new GetChatQuery(uuid));
            await _mediator.Publish(new RemoveChatNotification(chat));
            await _mediator.Send(new RemoveChatCommand(uuid));
            
            return Ok();
        }

        /// <summary>
        /// Clear all chats.
        /// </summary>
        /// <returns></returns>
        [KeyAuth]
        [HttpDelete]
        public async Task<IActionResult> Clear()
        {
            await _mediator.Send(new ClearChatsNotification());
            await _mediator.Send(new ClearChatsCommand());
            return Ok();
        }
    }
}