using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RocketShop.Framework.Extension;
using System.Threading.Tasks;

namespace RocketShop.Warehouse.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MachineAuthorization(RequestDelegate next)
    {

        public async Task Invoke(HttpContext httpContext,ILogger<MachineAuthorization> logger)
        {
            if (httpContext.Request.Path
                .ToString()
                .Replace("/",string.Empty)
                .Replace("\\",string.Empty)
                .ToLower() == "query" && httpContext.Request.Method=="POST")
            {
                logger.LogInformation("An incoming request has been received.");
                var token = httpContext.Request.Headers.Authorization.ToString();
                if (token.IsNullOrEmpty() || !token.ToLower().StartsWith("bearer "))
                {
                    httpContext.Response.StatusCode = 401;
                    await httpContext.Response.WriteAsync("token is missing");
                    logger.LogError("This Request Is Missing Bearer Token. Request Terminated.");
                    return;
                }
                else
                    logger.LogInformation("Request Authenicated. Pass To next.");
            }
            await next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MachineAuthorizationExtensions
    {
        public static IApplicationBuilder UseMachineAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MachineAuthorization>();
        }
    }
}
