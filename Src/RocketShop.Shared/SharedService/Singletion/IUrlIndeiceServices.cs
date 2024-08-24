using LanguageExt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.Shared.GlobalConfiguration;
using RocketShop.Shared.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.SharedService.Singletion
{
    public interface IUrlIndeiceServices
    {
        Task<Either<Exception, ConfigurationCenter>> GetUrls();
    }
    public class UrlIndeiceServices(IConfiguration configuration, ILogger<UrlIndeiceServices> logger) : 
        BaseServices<UrlIndeiceServices>("Url Indeice Services", logger), IUrlIndeiceServices
    {
        ConfigurationCenter? center;
        public async Task<Either<Exception, ConfigurationCenter>> GetUrls() =>
            await InvokeServiceAsync(async () =>
            {
                if (center.IsNotNull()) return center!;
                var url = $"{configuration.GetDomainCenterServiceUrl()}/Urls";
                using var client = new HttpClient(HttpClientHelper.CreateByPassSSLHandler());
                var item = await client.GetFromJsonAsync<ConfigurationCenter>(url);
                center = item;
                return center!;
            }, retries: 5,
                isExponential: true);
    }
}
