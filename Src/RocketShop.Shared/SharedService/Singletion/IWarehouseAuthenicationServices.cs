using DocumentFormat.OpenXml.Validation;
using LanguageExt;
using LanguageExt.Pipes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.Shared.GlobalConfiguration;
using RocketShop.Shared.Helper;
using RocketShop.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.SharedService.Singletion
{
    public interface IWarehouseAuthenicationServices
    {
        Task<Either<Exception, string>> CreateNewToken();
        Task<Either<Exception, WarehouseTokenIntrospectionResult>> Introspection(string token);
        Task<Either<Exception, bool>> Revocation(string token);
    }
    public class WarehouseAuthenicationServices(IConfiguration configuration,
        IUrlIndeiceServices urlIndeiceServices,
        ILogger<WarehouseAuthenicationServices> logger) : BaseServices<WarehouseAuthenicationServices>("Warehouse Authenication Services", logger), IWarehouseAuthenicationServices
    {
        ConfigurationCenter? urlConfig;
        WHClientConfiguration? clientConfig = configuration.GetWarehouseCredential();
        public async Task<Either<Exception, string>> CreateNewToken() =>
            await InvokeServiceAsync(async () =>
            {
                if (urlConfig.IsNull())
                {
                    var urlConfigResult = await urlIndeiceServices.GetUrls();
                    if (urlConfigResult.IsLeft)
                        throw urlConfigResult.GetLeft()!;
                    urlConfig = urlConfigResult.GetRight();
                }
                var url = $"{urlConfig!.WarehouseAPIUrl}/connect/token";
                Dictionary<string, string> requestParam = new Dictionary<string, string>();
                requestParam.Add("client_id", clientConfig!.ClientId);
                requestParam.Add("application", clientConfig!.ClientId);
                if (clientConfig.ClientSecret.HasMessage())
                    requestParam.Add("client_secret", clientConfig!.ClientSecret!);
                using var client = new HttpClient(HttpClientHelper.CreateByPassSSLHandler());
                var content = new FormUrlEncodedContent(requestParam);
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadAsStringAsync();
            },
                retries: 3,
                isExponential: true);

        public async Task<Either<Exception, WarehouseTokenIntrospectionResult>> Introspection(string token) =>
            await InvokeServiceAsync(async () =>
            {
                if (urlConfig.IsNull())
                {
                    var urlConfigResult = await urlIndeiceServices.GetUrls();
                    if (urlConfigResult.IsLeft)
                        throw urlConfigResult.GetLeft()!;
                    urlConfig = urlConfigResult.GetRight();
                }
                var url = $"{urlConfig!.WarehouseAPIUrl}/connect/introspection";
                Dictionary<string, string> requestParam = new Dictionary<string, string>();
                requestParam.Add("token", token);
                var content = new FormUrlEncodedContent(requestParam);
                using var client = new HttpClient(HttpClientHelper.CreateByPassSSLHandler());
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
                return await response.Content.ReadFromJsonAsync<WarehouseTokenIntrospectionResult>();
            },
                retries: 3,
                isExponential: true);

         public async Task<Either<Exception, bool>> Revocation(string token) =>
            await InvokeServiceAsync(async () =>
            {
                if (urlConfig.IsNull())
                {
                    var urlConfigResult = await urlIndeiceServices.GetUrls();
                    if (urlConfigResult.IsLeft)
                        throw urlConfigResult.GetLeft()!;
                    urlConfig = urlConfigResult.GetRight();
                }
                var url = $"{urlConfig!.WarehouseAPIUrl}/connect/revocation";
                Dictionary<string, string> requestParam = new Dictionary<string, string>();
                requestParam.Add("token", token);
                var content = new FormUrlEncodedContent(requestParam);
                using var client = new HttpClient(HttpClientHelper.CreateByPassSSLHandler());
                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                    throw new HttpRequestException(await response.Content.ReadAsStringAsync());
                return  response.IsSuccessStatusCode;
            },
                retries: 3,
                isExponential: true);


    }
}
