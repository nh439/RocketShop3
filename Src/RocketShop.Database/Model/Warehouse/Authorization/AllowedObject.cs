using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Warehouse.Authorization
{
    [Table(TableConstraint.AllowedObject)]
    public class AllowedObject
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public long Client { get; set; }
        public string ObjectName { get; set; }
        public bool Read { get; set; } = true;
        public bool Write { get; set; } = false;
    }
}
