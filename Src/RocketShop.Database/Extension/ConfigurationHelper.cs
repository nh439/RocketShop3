using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Extension
{
    public static class ConfigurationHelper
    {
        public static string GetIdentityConnectionString(this IConfiguration configuration) => configuration.GetConnectionString("IdentityDatabase")!;
        public static string GetWarehouseConnectionString(this IConfiguration configuration) => configuration.GetConnectionString("WarehouseDatabase")!;
        public static string GetRetailConnectionString(this IConfiguration configuration) => configuration.GetConnectionString("RetailDatabase")!;
        public static string GetRepairConnectionString(this IConfiguration configuration) => configuration.GetConnectionString("RepairDatabase")!;      
    }
}
