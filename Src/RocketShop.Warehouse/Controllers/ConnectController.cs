using Microsoft.AspNetCore.Mvc;

namespace RocketShop.Warehouse.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConnectController : ControllerBase
    {
        [HttpPost("token")]
        [Consumes("application/x-www-form-urlencoded")]
        public async Task<IActionResult> CallToken(
            [FromForm]string clientId,
            [FromForm] string? clientSecret,
            [FromForm] string application)
        {
            return Ok(200);
        }
    }
}
