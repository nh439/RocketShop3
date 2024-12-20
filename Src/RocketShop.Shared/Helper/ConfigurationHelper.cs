using Microsoft.Extensions.Configuration;
using RocketShop.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Helper
{
    public static class ConfigurationHelper
    {
        public static string GetDomainCenterServiceUrl(this IConfiguration configuration) => configuration.GetSection("DomainCenterService").Value!;
        public static WHClientConfiguration? GetWarehouseCredential(this IConfiguration configuration,string WHKey = "Warehouse") => configuration.GetSection(WHKey).Get<WHClientConfiguration>();
    }
}
