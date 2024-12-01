using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RocketShop.Framework.Attribute;

namespace RocketShop.Database.Model.Warehouse.Authorization
{
    [Table(TableConstraint.ClientHistory)]
    public class ClientHistory
    {
        [Key]
        [NHAutoTableSkipColumn]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [NHAutoTableSkipColumn]
        public string Key { get; set; }
        [NHAutoTableSkipColumn]
        public long ClientId { get; set; }
        [NHAutoTableDateTimeFormatDisplay("yyyy-MM-dd HH:mm:ss")]
        [NHAutoTableColumnDisplay("Query Date (UTC)")]
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [NHAutoTableColumnDisplay("Ip Address")]
        public string? IpAddress {  get; set; }
    }
}
