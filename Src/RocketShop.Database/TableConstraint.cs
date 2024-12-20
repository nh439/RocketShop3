using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database
{
    public static class TableConstraint
    {
        #region Identity
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
        #endregion
        #region Audit
        public const string ActivityLog = "EventLog";
        #endregion
        #region Warehouse
        public const string Province = "Provinces";
        public const string District = "Districts";
        public const string SubDistrict = "SubDistricts";
        public const string AddressView = "V_Address";
        public const string Client = "Authorization_Client";
        public const string ClientSecret = "Authorization_Client_Secret";
        public const string Token = "Authorization_Token";
        public const string AllowedObject = "Authorization_Allowed_Object";
        public const string ClientHistory = "Authorization_Client_History";
        #endregion
        #region Retail
        public const string MainCategory = "M_Category";
        public const string SubCategory = "S_Category";
        public const string Product = "Products";
        public const string ProductAttribute = "ProductAttribute";
        public const string ProductImage = "ProductImage";
        #endregion
    }
}
