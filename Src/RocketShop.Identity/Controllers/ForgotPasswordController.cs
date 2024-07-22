using LanguageExt;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RocketShop.Framework.Extension;
using RocketShop.Identity.IdentityBaseServices;
using RocketShop.Identity.Service;

namespace RocketShop.Identity.Controllers
{
    [Route("[controller]")]
    public class ForgotPasswordController(ILogger<ForgotPasswordController> logger,
       IPasswordServices passwordServices,
       IUserService userService) : IdentityControllerServices(logger)
    {
        [HttpGet]
        public IActionResult Index() =>
            InvokeControllerService(() => View());

        [HttpPost("")]
        public async Task<IActionResult> VerifyUser(string code, string email) =>
            await InvokeControllerServiceAsync(async () =>
            {
                var user = await userService.GetByEmpCode(code);
                if (user.IsLeft)
                {
                    ViewBag.e = user.GetLeft()!.Message;
                    return View("Index");
                }
                if (user.GetRight().IsNull() || (user.GetRight().Email != email))
                {
                    ViewBag.e = "User Not Found";
                    return View("Index");
                }
                return View("VerifiedUser");
            });
    }
}
