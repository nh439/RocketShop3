using RocketShop.Database.Model.Identity;
using RocketShop.Database.Model.Identity.Views;
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

    }
}
