using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RocketShop.Framework.Extension;
using RocketShop.Identity.IdentityBaseServices;
using RocketShop.Identity.Service;
using System.Security.Claims;
using System.Security.Principal;

namespace RocketShop.Identity.Controllers
{
    [Route("[controller]")]
    public class ProfileController(IProfileServices profileServices, ILogger<ProfileController> logger) : 
        IdentityControllerServices(logger)
    {

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index() =>
           await InvokeControllerServiceAsync(async () => View(await profileServices.GetProfile(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value)));

        [HttpGet("me")]
        public async Task<IActionResult> GetProfileAPI() =>
            await AuthorizedViaHeaderServicesAsync(async token =>
            {
                var userResult = await profileServices.GetProfile(token.Subject);
                if (userResult.IsLeft)
                    throw userResult.GetLeft()!;
                return userResult.GetRight();
            });

        [HttpPost("clearprovider")]
        [Authorize]
        public async Task<IActionResult> ClearProvider() =>
            await InvokeControllerServiceAsync(async () => {
               var result = await profileServices.ClearProviderKey(HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value);
                if (result.IsLeft)
                    throw result.GetLeft()!;
                return Redirect("/Profile");
                });


    }
}
