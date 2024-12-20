using Microsoft.Extensions.Configuration;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Extension
{
    public static class ConfigurationExtension
    {
        public static bool EnabledSwagger(this IConfiguration configuration,string? configName = null)
        {
            try
            {
                configName = configName.HasMessage() ? configName : "EnabledSwagger";
                return configuration.GetSection(configName!).Get<bool>();
            }
            catch
            {
                return false;
            }
        }
        public static bool EnabledSqlLogging(this IConfiguration configuration,string? configName = null)
        {
            try
            {
                configName = configName.HasMessage() ? configName : "EnabledSqlLogging";
                return configuration.GetSection(configName!).Get<bool>();
            }
            catch
            {
                return false;
            }
        }
    }
}
