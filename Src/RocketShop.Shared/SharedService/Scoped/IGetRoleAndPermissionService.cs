using LanguageExt;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.Shared.SharedService.Singletion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.SharedService.Scoped
{
    public interface IGetRoleAndPermissionService
    {
        Task<Either<Exception, string[]>> GetMyRoles(string token);
        Task<Either<Exception, string[]>> GetMyPermissions(string token);
    }
    public class GetRoleAndPermissionService(Serilog.ILogger logger, 
        IUrlIndeiceServices urlIndeiceServices) : BaseServices("Get Role And Permission Service", logger) , 
        IGetRoleAndPermissionService
    {

        public async Task<Either<Exception, string[]>> GetMyRoles(string token) =>
            await InvokeServiceAsync(async () =>
            {
                var urls = (await urlIndeiceServices.GetUrls()).GetRight();
                using var client = new HttpClient(HttpClientHelper.CreateByPassSSLHandler());
                client.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", token);
                return await client.GetFromJsonAsync<string[]>($"{urls!.IdentityUrl}/RolePermission/MyRole");
            },
               retries : 3,
               isExponential:true);

        public async Task<Either<Exception, string[]>> GetMyPermissions(string token) =>
            await InvokeServiceAsync(async () =>
            {
                var urls = (await urlIndeiceServices.GetUrls()).GetRight();
                using var client = new HttpClient(HttpClientHelper.CreateByPassSSLHandler());
                client.DefaultRequestHeaders.Authorization= new AuthenticationHeaderValue("Bearer", token);
                return await client.GetFromJsonAsync<string[]>($"{urls!.IdentityUrl}/RolePermission/MyPermission");
            },
               retries : 3,
               isExponential:true);



    }

}
