using LanguageExt;
using Microsoft.Extensions.Configuration;
using RocketShop.Framework.Extension;
using RocketShop.Framework.Services;
using RocketShop.Shared.Helper;
using RocketShop.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.SharedService
{
    public interface ISendEmailServices
    {
        Task<Either<Exception, string>> SendEmailAsync(MailRequest request);
    }
    public class SendEmailServices(IConfiguration configuration, Serilog.ILogger logger) :
        BaseServices("Send Email Service", logger),ISendEmailServices
    {
        public async Task< Either<Exception, string>> SendEmailAsync(MailRequest request) =>
            await InvokeServiceAsync(async () =>
            {
                var url = $"{configuration.GetDominCenterServiceUrl()}/mail";
                using HttpClient client = new HttpClient(HttpClientHelper.CreateByPassSSLHandler());
                var response = await client.PostAsJsonAsync(url, request);
                return await response.Content.ReadAsStringAsync();
            },
                retries: 3,
                intervalSecond: 3);
    }

}
