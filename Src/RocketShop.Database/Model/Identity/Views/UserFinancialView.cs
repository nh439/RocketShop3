using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity.Views
{
    public class UserFinancialView 
    {
        public string UserId { get; set; }
        public decimal Salary { get; set; } = 0;
        public decimal SocialSecurites { get; set; } = 0;
        public decimal ProvidentFundPerMonth { get; set; } = 0;
        public decimal TotalPayment { get; set; } = 0;
        public decimal TravelExpenses { get; set; } = 0;
        public decimal TotalAddiontialExpense { get; set; } = 0;
        public string BanckName { get; set; }
        public string AccountNo { get; set; }
        public string Currency { get; set; } = "THB";
        public decimal AccumulatedProvidentFund { get; set; }

    }
}
