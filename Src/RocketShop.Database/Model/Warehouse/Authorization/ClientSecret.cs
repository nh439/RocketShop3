using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Warehouse.Authorization
{
    [Table(TableConstraint.ClientSecret)]
    public class ClientSecret
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public long Client {  get; set; }
        public string SecretValue { get; set; }
        public string? Description { get; set; }
        public string Salt { get; set; }=Guid.NewGuid().ToString();
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? Expired { get; set; }
    }
}
