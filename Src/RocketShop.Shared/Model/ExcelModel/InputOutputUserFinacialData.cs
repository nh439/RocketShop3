using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace RocketShop.Shared.Model.ExcelModel
{
    public record InputOutputUserFinacialData(
        string EmployeeCode,
        string BankName,
        string AccountNo,
        decimal Salary,
        decimal SocialSecurites,
        decimal TravelExpense,
        decimal ProvidentFundPerMonth);


}
