using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity
{
    [Table(TableConstraint.UserPayroll)]
    public class UserPayroll
    {
        [Key]
        public string PayRollId { get; set; }
        public string UserId { get; set; }
        public DateTime PayrollDate { get; set; }
        public decimal Salary { get; set; } = 0;
        public decimal SocialSecurites { get; set; } = 0;
        public decimal ProvidentFund { get; set; } = 0;
        public decimal TotalPayment { get; set; } = 0;
        public decimal TravelExpenses { get; set; } = 0;
        public decimal TotalAdditionalPay { get; set; } = 0;
    }
}
