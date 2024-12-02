using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using RocketShop.Framework.Extension;
using RocketShop.Warehouse.Services;
using System.Text.Json;
using System.Threading.Tasks;

namespace RocketShop.Warehouse.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MachineAuthorization(RequestDelegate next,
        IAuthenicationService authenicationService,
        IHttpContextAccessor accessor)
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

                var results = await authenicationService.UseToken(token.Trim().Replace("bearer ", string.Empty)
                    .Replace("Bearer ", string.Empty));
                if (results.IsLeft)
                {
                    httpContext.Response.StatusCode = 500;
                    var exc = results.GetLeft()!;
                    logger.LogCritical(exc, exc.Message);
                    return;
                }
                var allowedObj = results.GetRight()!;
                var readableObject = allowedObj.Where(x=>x.Read).Select(x=>x.ObjectName);
                var writeObject = allowedObj.Where(x=>x.Write).Select(x=>x.ObjectName);
                var keys = accessor.HttpContext!.Session.Keys;
                if(keys.Where(x=>x == "read" || x == "write").HasData())
                    accessor.HttpContext!.Session.Clear();
                accessor.HttpContext!.Session.SetString("read", JsonSerializer.Serialize(readableObject));
                accessor.HttpContext!.Session.SetString("write", JsonSerializer.Serialize(writeObject));
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
