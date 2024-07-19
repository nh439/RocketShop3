using Microsoft.AspNetCore.Mvc;

namespace RocketShop.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
