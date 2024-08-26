using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity
{
    [Table(TableConstraint.UserProvidentFund)]
    public class UserProvidentFund
    {
        [Key]
        public string UserId { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; } = "THB";
    }
}
