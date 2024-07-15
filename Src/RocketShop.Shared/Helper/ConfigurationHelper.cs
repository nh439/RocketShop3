using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Helper
{
    public static class ConfigurationHelper
    {
        public static string GetDominCenterServiceUrl(this IConfiguration configuration) => configuration.GetSection("DomainCenterService").Value!;
    }
}
