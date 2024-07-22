using Microsoft.AspNetCore.Mvc;
using RocketShop.Framework.Extension;
using RocketShop.Identity.IdentityBaseServices;
using RocketShop.Identity.Service;

namespace RocketShop.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(ILogger<UserController> logger,
            IUserService userService) 
        : IdentityControllerServices(logger)
    {
        [HttpGet]
        [ResponseCache(VaryByHeader = "User-Agent", Duration = 10)]
        public async Task<IActionResult> Index(string? search, int? page, int? per) =>
        await AuthorizedViaHeaderServicesAsync(async token =>
        {
            var result = search.HasMessage() ?
            await userService.GetUserList(search!, page, per ?? 20) :
            await userService.GetUserList(page, per ?? 20);
            if (result.IsLeft)
                throw result.GetLeft()!;
            return Ok(result.GetRight());
        });
        
    }
}
