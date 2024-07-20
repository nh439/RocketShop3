using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RocketShop.Identity.IdentityBaseServices;
using RocketShop.Identity.Service;

namespace RocketShop.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RolePermissionController(IRoleAndPermissionService roleAndPermissionService,ILogger<RolePermissionController> logger) 
        : IdentityControllerServices(logger)
    {
        [HttpGet("MyRole")]
        [Authorize]
        public async Task<IActionResult> Index() =>
            await AuthorizedViaHeaderServicesAsync(async token =>
                 await roleAndPermissionService.GetRoleByUserId(token.Subject)
            );
        [HttpGet("permissioncheck")]
        [Authorize]
        public async Task<IActionResult> CheckMyPermission(string permissionName) =>
            await AuthorizedViaHeaderServicesAsync(async token =>
            await roleAndPermissionService.PermissionCheckByUserId(token.Subject,permissionName)
            );

    }
}
