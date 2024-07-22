using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.GlobalConfiguration
{
    public sealed class ConfigurationCenter
    {
        public string IdentityUrl { get; set; }
        public string RetailUrl { get; set; }
        public string RepairUrl { get; set; }
        public string WarehouseAPIUrl { get; set; }
        public string WarehouseAdminUrl { get; set; }
        public string MachineLearningUrl { get; set; }
        public string HRUrl { get; set; }
    }
}
