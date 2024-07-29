using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RocketShop.Framework.Extension;
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
        public async Task<IActionResult> Index() =>
            await AuthorizedViaHeaderServicesAsync(async token =>
            {
                var subject = token.Subject;
                if (!subject.HasMessage())
                    subject = token.Claims.FirstOrDefault(x => x.Type == "nameid")!.Value;
               var result = await roleAndPermissionService.GetRoleByUserId(subject);
                if (result.IsLeft)
                    throw result.GetLeft()!;
                return result.GetRight()!.Select(s=>s.RoleName);
            }                
            );

        [HttpGet("MyPermission")]
        public async Task<IActionResult> MyPermission() =>
            await AuthorizedViaHeaderServicesAsync(async token =>
                {
                    var subject = token.Subject;
                    if (!subject.HasMessage())
                        subject = token.Claims.FirstOrDefault(x => x.Type == "nameid")!.Value;
                    var result = await roleAndPermissionService.GetAuthoriozedPermissionList(subject);
                    if (result.IsLeft)
                        throw result.GetLeft()!;
                    return result.GetRight();
                }
            );

        [HttpGet("permissioncheck")]
        public async Task<IActionResult> CheckMyPermission(string permissionName) =>
            await AuthorizedViaHeaderServicesAsync(async token =>
            await roleAndPermissionService.PermissionCheckByUserId(token.Subject,permissionName)
            );

    }
}
