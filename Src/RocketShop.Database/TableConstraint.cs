using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database
{
    public static class TableConstraint
    {
        public const string User = "AspNetUsers";
        public const string UserInformation = "UserInformations";
        public const string UserView = "V_User";
        public const string Role = "Roles";
        public const string UserRole = "UserRoles";
        public const string ChangePasswordHistory = "ChangePasswordHistories";
        public const string UserFinancal = "UserFinancal";
        public const string UserAddiontialExpense = "AddiontialExpense";
        public const string UserProvidentFund = "ProvidentFund";
        public const string UserPayroll = "Payroll";
        public const string AdditionalPayroll = "AdditionalPayroll";
        public const string UserFinacialView = "V_UFinacial";
    }
}
