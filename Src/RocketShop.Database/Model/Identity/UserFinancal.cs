using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity
{
    [Table(TableConstraint.UserFinancal)]
    public class UserFinancal
    {
        [Key]
        public string UserId { get; set; }
        public decimal Salary { get; set; } = 0;
        public decimal SocialSecurites { get; set; } = 0;
        public decimal ProvidentFund { get; set; } = 0;
        public decimal TotalPayment { get; set; } = 0;
        public decimal TravelExpenses { get; set; } = 0;    
        public decimal TotalAddiontialExpense { get; set; } = 0;
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string Currency { get; set; } = "THB";
    }
}
