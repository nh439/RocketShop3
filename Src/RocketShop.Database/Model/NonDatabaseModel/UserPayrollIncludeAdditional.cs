using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.NonDatabaseModel
{
    public class UserPayrollIncludeAdditional : UserPayroll
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public UserPayrollIncludeAdditional() { }

        public UserPayrollIncludeAdditional(UserPayroll payroll, IEnumerable<AdditionalPayroll>? additionalPayrolls = null)
        {
            PayRollId = payroll.PayRollId;
            UserId = payroll.UserId;
            PayrollDate = payroll.PayrollDate;
            Salary = payroll.Salary;
            SocialSecurites = payroll.SocialSecurites;
            ProvidentFund = payroll.ProvidentFund;
            TotalPayment = payroll.TotalPayment;
            TravelExpenses = payroll.TravelExpenses;
            TotalAdditionalPay = additionalPayrolls.HasData() ? additionalPayrolls!.Select(s => s.Value).Sum() : payroll.TotalAdditionalPay;
            additionalPayrolls.HasDataAndTranformData(a => AdditionalPayrolls = a.ToList());
        }
        public List<AdditionalPayroll>? AdditionalPayrolls { get; set; }
        public bool HasAdditional
        {
            get
            {
                return AdditionalPayrolls.HasData();
            }
        }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
    }
}
