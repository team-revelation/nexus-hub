using System;
using System.Threading.Tasks;
using Authentication;
using Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Types.Users;
using Users.Commands;
using Users.Notifications;
using Users.Queries;

namespace Users.Controllers
{
    [Route("api/friends")]
    [Produces("application/json")]
    [ApiController]
    public class FriendController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserService _userService;
        
        public FriendController(IMediator mediator, IUserService userService)
        {
            _mediator = mediator;
            _userService = userService;
        }

        /// <summary>
        /// Get all the friends of a specific user.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpGet("{uuid:guid}")]
        public async Task<IActionResult> Get(Guid uuid)
        {
            var query = new GetFriendsQuery(uuid, false);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get a all the pending friends of a specific user.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpGet("pending/{uuid:guid}")]
        public async Task<IActionResult> Pending(Guid uuid)
        {
            var query = new GetFriendsQuery(uuid, true);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Get a friend of a specific user if they are their friend.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="friendUuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpGet("{uuid:guid}/{friendUuid:guid}")]
        public async Task<IActionResult> Get(Guid uuid, Guid friendUuid)
        {
            var user = await _mediator.Send(new GetUserQuery(uuid));
            var friend = await _mediator.Send(new GetUserQuery(friendUuid));

            return user.IsFriend(friend, FriendType.Friend) ? Accepted(new Friend(friend, FriendType.Friend)) : NotFound();
        }

        /// <summary>
        /// Send a friend request to another user.
        /// </summary>
        /// <param name="friendUuid"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPatch("requests/uuid/{friendUuid:guid}/send")]
        public async Task<IActionResult> RequestUuid(Guid friendUuid, [FromQuery] Guid uuid)
        {
            var result = await _mediator.Send(new RequestFriendCommand(friendUuid, uuid));
            result = await _mediator.Send(new FillUserCommand(result));
            
            await _mediator.Publish(new FriendRequestedNotification(friendUuid, result));
            await _mediator.Publish(new UserUpdatedNotification(result));
            return result is null ? Problem() : Ok(result);
        }

        /// <summary>
        /// Send a friend request to another user.
        /// </summary>
        /// <param name="friendEmail"></param>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPatch("requests/email/{friendEmail}/send")]
        public async Task<IActionResult> RequestEmail(string friendEmail, [FromQuery] Guid uuid)
        {
            var result = await _mediator.Send(new RequestFriendCommand(friendEmail, uuid));
            result = await _mediator.Send(new FillUserCommand(result));

            var friend = await _userService.Get(friendEmail);
            await _mediator.Publish(new FriendRequestedNotification(friend.Uuid, result));
            await _mediator.Publish(new UserUpdatedNotification(result));
            return result is null ? Problem() : Ok(result);
        }

        /// <summary>
        /// Accept an incoming friend request.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="friendUuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPatch("requests/{uuid:guid}/{friendUuid:guid}/accept")]
        public async Task<IActionResult> Accept(Guid uuid, Guid friendUuid)
        {
            var result = await _mediator.Send(new AcceptRequestCommand(friendUuid, uuid));
            result = await _mediator.Send(new FillUserCommand(result));
            
            await _mediator.Publish(new FriendAcceptedNotification(friendUuid, result));
            await _mediator.Publish(new UserUpdatedNotification(result));
            return result is null ? Problem() : Ok(result);
        }

        /// <summary>
        /// Decline an incoming friend request.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="friendUuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPatch("requests/{uuid:guid}/{friendUuid:guid}/decline")]
        public async Task<IActionResult> Decline(Guid uuid, Guid friendUuid)
        {
            var result = await _mediator.Send(new DeclineRequestCommand(friendUuid, uuid));
            result = await _mediator.Send(new FillUserCommand(result));
            
            await _mediator.Publish(new FriendDeclinedNotification(friendUuid, result));
            await _mediator.Publish(new UserUpdatedNotification(result));
            return result is null ? Problem() : Ok(result);
        }

        /// <summary>
        /// Remove an already existing friend.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="friendUuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpDelete("{uuid:guid}/{friendUuid:guid}")]
        public async Task<IActionResult> Remove(Guid uuid, Guid friendUuid)
        {
            var result = await _mediator.Send(new RemoveFriendCommand(friendUuid, uuid));
            result = await _mediator.Send(new FillUserCommand(result));

            await _mediator.Publish(new FriendRemovedNotification(friendUuid, result));
            await _mediator.Publish(new UserUpdatedNotification(result));
            return result is null ? Problem() : Ok(result);
        }
    }
}