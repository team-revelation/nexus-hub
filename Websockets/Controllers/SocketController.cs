using System.Threading.Tasks;
using Authentication;
using Microsoft.AspNetCore.Mvc;
using Websockets.Services;

namespace Websockets.Controllers
{
    [ApiController]
    [Route("api")]
    public class SocketController : ControllerBase
    {
        private readonly ISocketService _socketService;

        public SocketController(ISocketService socketService)
        {
            _socketService = socketService;
        }

        /// <summary>
        /// Initialize a websocket connection.
        /// </summary>
        /// <returns></returns>
        [TokenAuth]
        [HttpGet("ws")]
        public async Task<ActionResult> Listen()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await _socketService.Listen(webSocket, TokenAuthAttribute.CurrentEmail);
            }
            else return BadRequest("This is not a websocket request.");

            return Ok();
        }
    }
}