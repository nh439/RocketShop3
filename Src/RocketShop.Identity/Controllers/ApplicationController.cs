using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RocketShop.Framework.Extension;
using RocketShop.Identity.Helper;
using RocketShop.Identity.IdentityBaseServices;
using RocketShop.Shared.SharedService.Singletion;

namespace RocketShop.Identity.Controllers
{
    [Route("[controller]")]
    public class ApplicationController(
        ILogger<ApplicationController> logger,
        IUrlIndeiceServices urlIndeiceServices) : IdentityControllerServices(logger)
    {
        [Authorize]
        [Route("HR")]
        public async Task<IActionResult> HR() =>
            await InvokeControllerServiceAsync(async () =>
            {
                var token = TokenHelper.BuildToken(HttpContext);
                var urlResult = await urlIndeiceServices.GetUrls();
                var url = urlResult.GetRight()!;
                var hrUrl = $"{url.HRUrl}/Signed?id_token={token}";
                return Redirect(hrUrl);
            });
        [Authorize]
        [Route("WHAdmin")]
        public async Task<IActionResult> WarehouseAdmin() =>
            await InvokeControllerServiceAsync(async () =>
            {
                var token = TokenHelper.BuildToken(HttpContext);
                var urlResult = await urlIndeiceServices.GetUrls();
                var url = urlResult.GetRight()!;
                var WHAdminUrl = $"{url.WarehouseAdminUrl}/Signed?id_token={token}";
                return Redirect(WHAdminUrl);
            });

        
    }
}
