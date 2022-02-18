using Microsoft.AspNetCore.Mvc;

namespace Users.Controllers
{
    [Route("")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Health check for the api.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}