using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Extension
{
    public static class HttpContextAccessorExtension
    {
        public static string? GetCurrentUserId(this IHttpContextAccessor accessor)=>
            accessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.NameIdentifier)!.Value;

        public static string? GetCurrentEmail(this IHttpContextAccessor accessor) =>
            accessor.HttpContext.User.FindFirst(x => x.Type == ClaimTypes.Email)!.Value;

        public static string? GetClientIpAddress(this IHttpContextAccessor accessor) =>
            accessor.HttpContext.GetClientIpAddress();

        public static string? GetClientIpAddress(this HttpContext  httpContext)
        {
            
            if (httpContext == null)
            {
                return null;
            }

            // Check if the IP is forwarded from a proxy or load balancer
            var forwardedIp = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (!string.IsNullOrEmpty(forwardedIp))
            {
                return forwardedIp.Split(',').FirstOrDefault()?.Trim();
            }

            // Fallback to the remote IP address
            return httpContext.Connection.RemoteIpAddress?.ToString();
        }
    }
}
