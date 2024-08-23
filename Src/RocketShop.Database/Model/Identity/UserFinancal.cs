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
        /// <summary>
        /// จำนวนเงินที่ไปกองทุนสำรองชีพ
        /// </summary>
        public decimal ProvidentFund { get; set; } = 0;
        /// <summary>
        /// เงินที่ พนง ได้ (salary - (SocalSecurities+ProvidentFund) + (TotalAddiontialExpense+TravelExpenses))
        /// </summary>
        public decimal TotalPayment { get; set; } = 0;
        public decimal TravelExpenses { get; set; } = 0;
        /// <summary>
        /// รวมค่าใช้จ่ายเพิ่มเติม(คิดเฉพาะที่เป็นค่าใช้จ่ายรายเดือน)
        /// </summary>
        public decimal TotalAddiontialExpense { get; set; } = 0;
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public string Currency { get; set; } = "THB";
    }
}
