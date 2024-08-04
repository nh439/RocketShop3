using LanguageExt;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.SharedService.Singletion
{
    public interface IGetRoleAndPermissionService
    {
        Task<Either<Exception, string[]>> GetMyRoles(string token);
        Task<Either<Exception, string[]>> GetMyPermissions(string token);
        Either<Exception, string> BuildToken(HttpContext httpContext);
    }
    public class GetRoleAndPermissionService(Serilog.ILogger logger,
        IUrlIndeiceServices urlIndeiceServices) : BaseServices("Get Role And Permission Service", logger),
        IGetRoleAndPermissionService
    {

        public async Task<Either<Exception, string[]>> GetMyRoles(string token) =>
            await InvokeServiceAsync(async () =>
            {
                var urls = (await urlIndeiceServices.GetUrls()).GetRight();
                using var client = new HttpClient(HttpClientHelper.CreateByPassSSLHandler());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await client.GetFromJsonAsync<string[]>($"{urls!.IdentityUrl}/RolePermission/MyRole");
            },
               retries: 3,
               isExponential: true);

        public async Task<Either<Exception, string[]>> GetMyPermissions(string token) =>
            await InvokeServiceAsync(async () =>
            {
                var urls = (await urlIndeiceServices.GetUrls()).GetRight();
                using var client = new HttpClient(HttpClientHelper.CreateByPassSSLHandler());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return await client.GetFromJsonAsync<string[]>($"{urls!.IdentityUrl}/RolePermission/MyPermission");
            },
               retries: 3,
               isExponential: true);

        public Either<Exception, string> BuildToken(HttpContext httpContext) =>
            InvokeService(() =>
        {
            var claims = httpContext.User.Claims;
            string secretKey = "828db15b-65f0-4492-a1eb-a89afb182e30"; // Replace with a strong, secure key
            SymmetricSecurityKey signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var jwtHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials,
                Expires = DateTime.Now.AddHours(10)
            };

            var securityToken = jwtHandler.CreateToken(tokenDescriptor);
            return jwtHandler.WriteToken(securityToken);

        });


    }

}
