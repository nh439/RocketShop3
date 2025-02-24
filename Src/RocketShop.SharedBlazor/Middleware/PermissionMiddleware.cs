﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RocketShop.Framework.Extension;
using RocketShop.Shared.SharedService.Singletion;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RocketShop.Warehouse.Admin.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class PermissionMiddleware(RequestDelegate next, IGetRoleAndPermissionService getRoleAndPermissionService)
    {
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.User.Identity.IsNotNull() && httpContext.User.Identity!.IsAuthenticated)
            {

                var permissions = httpContext.User.Claims.Where(x => x.Type == "Permission").ToList();
                var role = httpContext.User.Claims.Where(x => x.Type == "Role").ToList();
                if (!permissions.HasData() || !role.HasData())
                {
                    var token = getRoleAndPermissionService.BuildToken(httpContext).GetRight();
                    if (!permissions.HasData())
                    {
                        var newPermissions = await getRoleAndPermissionService.GetMyPermissions(token!);
                        var newClaims = newPermissions.GetRight()!.Select(s => new Claim("Permission", s));
                        httpContext.User.AddIdentity(new ClaimsIdentity(newClaims));
                    }
                    if (!role.HasData())
                    {
                        var newPermissions = await getRoleAndPermissionService.GetMyRoles(token!);
                        var newClaims = newPermissions.GetRight()!.Select(s => new Claim("Role", s));
                        httpContext.User.AddIdentity(new ClaimsIdentity(newClaims));
                    }

                }
            }
            await next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class PermissionMiddlewareExtensions
    {
        public static IApplicationBuilder UsePermissionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<PermissionMiddleware>();
        }
    }
}
