﻿using RocketShop.Database.Model.Identity.Views;
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

        public static CreateViewSql UserFinacialView = new CreateViewSql($@"
create or replace view ""{TableConstraint.UserFinacialView}"" as
SELECT u.""UserId"", u.""AccountNo"", u.""BanckName"", u.""Currency"", u.""ProvidentFund"" AS ""ProvidentFundPerMonth"", p.""Balance"" AS ""AccumulatedProvidentFund"", u.""Salary"", u.""SocialSecurites"", u.""TotalAddiontialExpense"", u.""TotalPayment"", u.""TravelExpenses""
FROM ""UserFinancal"" AS u
INNER JOIN ""ProvidentFund"" AS p ON u.""UserId"" = p.""UserId"";",
           $"Drop View \"{TableConstraint.UserFinacialView}\"");

        public static CreateViewSql UserFinacialViewV2 = new CreateViewSql($@"
create or replace view ""{TableConstraint.UserFinacialView}"" as
SELECT u.""UserId"", u.""AccountNo"", u.""BankName"", u.""Currency"", u.""ProvidentFund"" AS ""ProvidentFundPerMonth"", p.""Balance"" AS ""AccumulatedProvidentFund"", u.""Salary"", u.""SocialSecurites"", u.""TotalAddiontialExpense"", u.""TotalPayment"", u.""TravelExpenses""
FROM ""UserFinancal"" AS u
INNER JOIN ""ProvidentFund"" AS p ON u.""UserId"" = p.""UserId"";",
           $@"
create or replace view ""{TableConstraint.UserFinacialView}"" as
SELECT u.""UserId"", u.""AccountNo"", u.""BanckName"", u.""Currency"", u.""ProvidentFund"" AS ""ProvidentFundPerMonth"", p.""Balance"" AS ""AccumulatedProvidentFund"", u.""Salary"", u.""SocialSecurites"", u.""TotalAddiontialExpense"", u.""TotalPayment"", u.""TravelExpenses""
FROM ""UserFinancal"" AS u
INNER JOIN ""ProvidentFund"" AS p ON u.""UserId"" = p.""UserId"";");

        public static CreateViewSql UserFinacialViewV3 = new CreateViewSql($@"
create or replace view ""{TableConstraint.UserFinacialView}"" as
SELECT u.""UserId"",s.""EmployeeCode"" as ""EmployeeCode"",s.""Prefix""||' '|| s.""Firstname"" || ' '|| s.""Surname"" as ""EmployeeName"", u.""AccountNo"", u.""BankName"", u.""Currency"", u.""ProvidentFund"" AS ""ProvidentFundPerMonth"", p.""Balance"" AS ""AccumulatedProvidentFund"", u.""Salary"", u.""SocialSecurites"", u.""TotalAddiontialExpense"", u.""TotalPayment"", u.""TravelExpenses""
FROM ""UserFinancal"" AS u
INNER JOIN ""ProvidentFund"" AS p ON u.""UserId"" = p.""UserId""
inner join ""AspNetUsers"" s on s.""Id""=u.""UserId"";",
           $@"
create or replace view ""{TableConstraint.UserFinacialView}"" as
SELECT u.""UserId"", u.""AccountNo"", u.""BankName"", u.""Currency"", u.""ProvidentFund"" AS ""ProvidentFundPerMonth"", p.""Balance"" AS ""AccumulatedProvidentFund"", u.""Salary"", u.""SocialSecurites"", u.""TotalAddiontialExpense"", u.""TotalPayment"", u.""TravelExpenses""
FROM ""UserFinancal"" AS u
INNER JOIN ""ProvidentFund"" AS p ON u.""UserId"" = p.""UserId"";");
        #endregion

    }


    public sealed record CreateViewSql(string Up,string Down);
}
