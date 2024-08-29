using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.NonDatabaseModel
{
    public class UserFinacialData : UserFinancialView
    {
        public List< UserAddiontialExpense> ? AddiontialExpenses { get;set; }
        public UserFinancal GetUserFinancal => new UserFinancal
        {
            AccountNo = AccountNo,
            BankName = BankName,
            Currency = Currency,
            ProvidentFund = ProvidentFundPerMonth,
            Salary = Salary,
            SocialSecurites = SocialSecurites,
            TotalAddiontialExpense = TotalAddiontialExpense,
            TotalPayment = TotalPayment,
            TravelExpenses = TravelExpenses,
            UserId = UserId
        };
        public UserFinacialData()
        {

        }

        public UserFinacialData(UserFinancal userFinancal,IEnumerable<UserAddiontialExpense>? userAddiontialExpenses= null,UserProvidentFund? userProvidentFund = null)
        {
            AccountNo = userFinancal.AccountNo;
            BankName  = userFinancal.BankName;
            Currency  = userFinancal.Currency;
            ProvidentFundPerMonth= userFinancal.ProvidentFund;
            Salary        = userFinancal.Salary;
            SocialSecurites = userFinancal.SocialSecurites;
            TotalAddiontialExpense = userFinancal.TotalAddiontialExpense;
            TotalPayment = userFinancal.TotalPayment;
            TravelExpenses = userFinancal.TravelExpenses;
            UserId = userFinancal.UserId;
            if (userAddiontialExpenses.HasData())
            {
                TotalAddiontialExpense = userAddiontialExpenses!.Select(s => s.Balance).Sum();
                AddiontialExpenses = userAddiontialExpenses!.ToList();
            }
            if (userProvidentFund.IsNotNull())
                AccumulatedProvidentFund = userProvidentFund.Balance;
        }


    }
}
