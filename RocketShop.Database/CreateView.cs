using RocketShop.Database.Model.Identity.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database
{
    public static class CreateView
    {
        #region IdentityContext
        public static CreateViewSql UserView = new CreateViewSql($@"
create or replace view ""{TableConstraint.UserView}"" as SELECT a.""Id"", a.""EmployeeCode"", a.""Email"", a.""Firstname"", a.""Surname"", a.""Prefix"", u.""Department"", u.""CurrentPosition"", u.""ManagerId"", NOT (a.""Resigned"") as ""Active"" FROM ""AspNetUsers"" AS a LEFT JOIN ""UserInformations"" AS u ON a.""Id"" = u.""UserId""
", $"Drop View \"{TableConstraint.UserView}\"");
        #endregion
    }

    public sealed record CreateViewSql(string Up,string Down);
}
