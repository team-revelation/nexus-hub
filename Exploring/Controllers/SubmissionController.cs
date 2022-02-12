using System;
using System.Threading.Tasks;
using Authentication;
using Exploring.Commands;
using Exploring.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Types.Exploring;

namespace Exploring.Controllers
{
    [Produces("application/json")]
    [Route("api/submissions")]
    [ApiController]
    public class SubmissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public SubmissionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        ///     Get every submission in the database.
        /// </summary>
        /// <returns></returns>
        [TokenAuth]
        [HttpGet]
        public async Task<IActionResult> All()
        {
            var res = await _mediator.Send(new GetSubmissionsQuery());
            return Ok(res);
        }

        /// <summary>
        ///     Get submission with uuid.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpGet("{uuid:guid}")]
        public async Task<IActionResult> Get(Guid uuid)
        {
            var res = await _mediator.Send(new GetSubmissionQuery(uuid));
            return Ok(res);
        }

        /// <summary>
        ///     Create a new submission.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateSubmissionRequest request)
        {
            var res = await _mediator.Send(new CreateSubmissionCommand(request.User, request.Position));
            return res is null ? Problem() : Accepted(res);
        }

        /// <summary>
        ///     Update an already existing chat.
        /// </summary>
        /// <param name="uuid"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpPut("{uuid:guid}")]
        public async Task<IActionResult> Put(Guid uuid, [FromBody] UpdateSubmissionRequest request)
        {
            var res = await _mediator.Send(new UpdateSubmissionCommand(request.User, request.Position));
            return Accepted(res);
        }

        /// <summary>
        ///     Delete an already existing chat.
        /// </summary>
        /// <param name="uuid"></param>
        /// <returns></returns>
        [TokenAuth]
        [HttpDelete("{uuid:guid}")]
        public async Task<IActionResult> Delete(Guid uuid)
        {
            await _mediator.Send(new RemoveSubmissionCommand(uuid));
            return Ok();
        }

        /// <summary>
        ///     Clear all chats.
        /// </summary>
        /// <returns></returns>
        [KeyAuth]
        [HttpDelete]
        public async Task<IActionResult> Clear()
        {
            await _mediator.Send(new ClearSubmissionsCommand());
            return Ok();
        }
    }
}