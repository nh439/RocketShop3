using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RocketShop.Shared.Model.ExcelModel
{
    public class InputOutputUserFinacialData
    {
        public string EmployeeCode { get; set; }
        public string BankName { get; set; }
        public string AccountNo { get; set; }
        public decimal Salary { get; set; }
        public decimal SocialSecurites { get; set; }
        public decimal TravelExpense { get; set; }
        public decimal ProvidentFundPerMonth { get; set; }
    }


}
