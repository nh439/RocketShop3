using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity
{
    [Table(TableConstraint.UserAddiontialExpense)]
    public class UserAddiontialExpense
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserId { get; set; }
        public bool OneTimePay { get; set; }
        public int? Month { get; set; }
        public int? Year { get; set; }
        public string PreiodType { get; set; }
        public decimal Balance { get; set; }
        public bool Paid { get; set; }=false;
    }
}
