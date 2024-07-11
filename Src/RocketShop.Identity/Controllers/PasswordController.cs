using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RocketShop.Identity.IdentityBaseServices;

namespace RocketShop.Identity.Controllers
{
    [Route("[controller]")]
    public class PasswordController : IdentityControllerServices
    { 
        public PasswordController(ILogger<PasswordController> logger) 
        :base(logger){ 
        
        }
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
