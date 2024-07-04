using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RocketShop.Framework.ControllerFunction;
using RocketShop.Framework.Extension;
using RocketShop.Identity.IdentityBaseServices;
using RocketShop.Identity.Service;
using System.Security.Claims;
using System.Security.Principal;

namespace RocketShop.Identity.Controllers
{
    [Route("[controller]")]
    public class ProfileController : IdentityControllerServices
    {
        readonly IProfileServices profileServices;
        readonly ILogger<ProfileController> logger;
        public ProfileController(IProfileServices profileServices, ILogger<ProfileController> logger)
            :base(logger)
        {
            this.profileServices = profileServices;
            this.logger = logger;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index() =>
           await InvokeControllerServiceAsync(async () =>
           {
               var userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value;
               var userResult = await profileServices.GetProfile(userId);
               return View(userResult);
           });

        [HttpGet("me")]
        public async Task<IActionResult> GetProfileAPI() =>
            await AuthorizedViaHeaderServicesAsync(async token =>
            {
                var userId = token.Subject;
                var userResult = await profileServices.GetProfile(userId);
                if (userResult.IsLeft)
                    throw userResult.GetLeft()!;
                return userResult.GetRight();
            });
    }
}
