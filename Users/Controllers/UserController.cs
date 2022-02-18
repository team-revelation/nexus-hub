using System;
using System.Threading.Tasks;
using Authentication;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Types.Users;
using Users.Commands;
using Users.Notifications;
using Users.Queries;

namespace Users.Controllers
{
    [Route("api/users")]
    [Produces("application/json")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get every user in the database.
        /// </summary>
        /// <returns></returns>
        [KeyAuth]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetUsersQuery());
            result = await _mediator.Send(new FillUsersCommand(result));
            return Ok(result);
        }

        /// <summary>
        /// Get a user with a specific uuid.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpGet("{uuid:guid}")]
        public async Task<IActionResult> Get(Guid uuid)
        {
            var result = await _mediator.Send(new GetUserQuery(uuid));
            result = await _mediator.Send(new FillUserCommand(result));
            return Ok(result);
        }

        /// <summary>
        /// Get a user with a specific email address.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpGet("email/{userEmail}")]
        public async Task<IActionResult> Get(string userEmail)
        {
            var result = await _mediator.Send(new GetUserQuery(userEmail));
            result = await _mediator.Send(new FillUserCommand(result));
            return Ok(result);
        }

        /// <summary>
        /// Add a new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] User user)
        {
            var result = await _mediator.Send(new CreateUserCommand(user));
            return Ok(result);
        }

        /// <summary>
        /// Update an already existing user.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("{uuid:guid}")]
        public async Task<IActionResult> Put(Guid uuid, [FromBody] User user)
        {
            var result = await _mediator.Send(new UpdateUserCommand(user, uuid));
            result = await _mediator.Send(new FillUserCommand(result));
            
            await _mediator.Publish(new FriendUpdatedNotification(result));
            await _mediator.Publish(new UserUpdatedNotification(result));
            
            return Ok(result);
        }

        /// <summary>
        /// Delete an already existing user.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpDelete("{uuid:guid}")]
        public async Task<IActionResult> Delete(Guid uuid)
        {
            await _mediator.Send(new RemoveUserCommand(uuid));
            return Ok();
        }

        /// <summary>
        /// Clear all users.
        /// </summary>
        /// <returns></returns>
        [KeyAuth]
        [HttpDelete]
        public async Task<IActionResult> Clear()
        {
            await _mediator.Send(new ClearUsersCommand());
            return Ok();
        }
    }
}