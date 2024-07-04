using LanguageExt;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RocketShop.Framework.ControllerFunction;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RocketShop.Identity.IdentityBaseServices
{
    public class IdentityControllerServices : Controller
    {
        readonly ILogger logger;
        public IdentityControllerServices(ILogger logger)
        {
            this.logger = logger;
        }

        public IActionResult AuthorizedViaHeaderServices<TReturn>(Func<JwtSecurityToken, TReturn> Operation)
        {
            if (HttpContext.Request.Headers.Authorization.IsNull())
                return BadRequest("Authorization Is Missing");
            try
            {
                var jwt = HttpContext.Request.Headers.Authorization;
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);
                var result = Operation(token);
                if (result is IActionResult)
                    return (result as IActionResult)!;
                return Ok(result);
            }
            catch (Exception x)
            {
                logger.LogError(x.Message, x);
                return StatusCode(500, x);
            }
        }
        public async Task<IActionResult> AuthorizedViaHeaderServicesAsync<TReturn>(Func<JwtSecurityToken, Task<TReturn>> Operation)
        {
            if (HttpContext.Request.Headers.Authorization.IsNull())
                return BadRequest("Authorization Is Missing");
            try
            {
                string jwt = HttpContext.Request.Headers.Authorization!;
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt!.Replace("Bearer ", string.Empty));
                var result = await Operation(token);
                if (result is IActionResult)
                    return (result as IActionResult)!;
                return Ok(result);
            }
            catch (Exception x)
            {
                logger.LogError(x.Message, x);
                return StatusCode(500, x);
            }
        }

        public IActionResult InvokeControllerService<TReturn>(Func<TReturn> Operation)
        {
            try
            {
                var result = Operation();
                if (result is IActionResult)
                    return (result as IActionResult)!;
                return Ok(result);
            }
            catch (Exception x)
            {
                logger.LogError(x.Message, x);
                return StatusCode(500, x);
            }
        }
        public async Task<IActionResult> InvokeControllerServiceAsync<TReturn>(Func<Task<TReturn>> Operation)
        {
            try
            {
                var result = await Operation();
                if (result is IActionResult)
                    return (result as IActionResult)!;
                return Ok(result);
            }
            catch (Exception x)
            {
                logger.LogError(x.Message, x);
                return StatusCode(500, x);
            }
        }


    }
}
