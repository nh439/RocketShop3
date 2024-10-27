using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Warehouse.Authorization
{
    [Table(TableConstraint.Token)]
    public class Token
    {
        [Key]
        public string TokenKey { get; set; }
        public long Client { get; set; }
        public int? RemainingAccess { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public int? TokenAge { get; set; }

    }
}
