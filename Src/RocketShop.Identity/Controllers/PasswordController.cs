using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RocketShop.Identity.IdentityBaseServices;
using RocketShop.Identity.Service;
using System.Security.Claims;

namespace RocketShop.Identity.Controllers
{
    [Route("[controller]")]
    public class PasswordController : IdentityControllerServices
    {
        readonly PasswordServices _passwordServices;
        public PasswordController(ILogger<PasswordController> logger,PasswordServices passwordServices) 
        :base(logger){ 
        _passwordServices = passwordServices;
        }
        [Authorize]
        public IActionResult Index() =>
            InvokeControllerService(() =>
            {
                ViewBag.e = HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)!.Value;
                return View();
            });

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(string oldPassword, string newPassword, string retypePassword) =>
            await InvokeControllerServiceAsync(async () =>
            {
                var result = await _passwordServices.ChangePassword(
                    HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)!.Value,
                    oldPassword,
                    newPassword);
                return result;
            });

    }
}
