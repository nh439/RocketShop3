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
    public interface IRocketShopDivisionServices
    {
        Task<Either<Exception,List< string>>> GetDivisions();
    }
    public class RocketShopDivisionServices(ILogger<RocketShopDivisionServices> logger,
        IConfiguration configuration) :
        BaseServices<RocketShopDivisionServices>("Rocket Shop Division Services", logger), IRocketShopDivisionServices
    {
        List< string> divisions = new();
        public async Task<Either<Exception, List<string>>> GetDivisions() =>
            await InvokeServiceAsync(async () =>
            {
                if (divisions.HasData()) return divisions;
                var url = $"{configuration.GetDomainCenterServiceUrl()}/services";
                using var client = new HttpClient(HttpClientHelper.CreateByPassSSLHandler());
                var item = await client.GetFromJsonAsync<List<string>>(url);
                divisions = item;
                return divisions;
            });
    }
}
