using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Warehouse.Authorization
{
    [Table(TableConstraint.ClientHistory)]
    public class ClientHistory
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Key { get; set; }
        public long ClientId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string? IpAddress {  get; set; }
    }
}
