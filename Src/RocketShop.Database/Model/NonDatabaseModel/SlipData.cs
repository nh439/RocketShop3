using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.NonDatabaseModel
{
    public class SlipData :UserPayroll
    {
        public SlipData() { }
        public SlipData(UserPayroll? userPayroll, IEnumerable<AdditionalPayroll>? additionalPayrolls = null)
        {
            if (userPayroll.IsNotNull())
            {
                PayRollId = userPayroll!.PayRollId;
                UserId = userPayroll!.UserId;
                PayrollDate = userPayroll!.PayrollDate;
                Salary = userPayroll!.Salary;
                SocialSecurites = userPayroll!.SocialSecurites;
                ProvidentFund = userPayroll!.ProvidentFund;
                TotalPayment = userPayroll!.TotalPayment;
                TravelExpenses = userPayroll!.TravelExpenses;
                TotalAdditionalPay = userPayroll!.TotalAdditionalPay;
                Cancelled = userPayroll!.Cancelled;
            }
            additionalPayrolls.HasDataAndTranformData(f => AdditionalPayrolls = f.ToList());
    }
        public List<AdditionalPayroll>? AdditionalPayrolls { get; set; }
        public UserPayroll? ToUserPayroll()
        {
            return new UserPayroll
            {
                PayrollDate = PayrollDate,
                PayRollId = PayRollId,
                ProvidentFund = ProvidentFund,
                TotalPayment = TotalPayment,
                Salary = Salary,
                SocialSecurites = SocialSecurites,
                TotalAdditionalPay = TotalAdditionalPay,
                TravelExpenses = TravelExpenses,
                UserId = UserId
            };
        }
        public List<AdditionalPayroll>? ToAdditionalPayroll() =>
            AdditionalPayrolls;

        public bool HasAdditional
        {
            get
            {
                return AdditionalPayrolls.HasData();
            }
        }
    }
}
