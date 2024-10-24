using DocumentFormat.OpenXml.Office2010.Excel;
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
         public static CreateViewSql UserFinacialViewV4 = new CreateViewSql($@"
create or replace view ""{TableConstraint.UserFinacialView}"" as
SELECT u.""UserId"",s.""EmployeeCode"" as ""EmployeeCode"",COALESCE(s.""Prefix"",'')||' '|| s.""Firstname"" || ' '|| s.""Surname"" as ""EmployeeName"", u.""AccountNo"", u.""BankName"", u.""Currency"", u.""ProvidentFund"" AS ""ProvidentFundPerMonth"", p.""Balance"" AS ""AccumulatedProvidentFund"", u.""Salary"", u.""SocialSecurites"", u.""TotalAddiontialExpense"", u.""TotalPayment"", u.""TravelExpenses""
FROM ""UserFinancal"" AS u
INNER JOIN ""ProvidentFund"" AS p ON u.""UserId"" = p.""UserId""
inner join ""AspNetUsers"" s on s.""Id""=u.""UserId"";",
          $@"
create or replace view ""{TableConstraint.UserFinacialView}"" as
SELECT u.""UserId"",s.""EmployeeCode"" as ""EmployeeCode"",s.""Prefix""||' '|| s.""Firstname"" || ' '|| s.""Surname"" as ""EmployeeName"", u.""AccountNo"", u.""BankName"", u.""Currency"", u.""ProvidentFund"" AS ""ProvidentFundPerMonth"", p.""Balance"" AS ""AccumulatedProvidentFund"", u.""Salary"", u.""SocialSecurites"", u.""TotalAddiontialExpense"", u.""TotalPayment"", u.""TravelExpenses""
FROM ""UserFinancal"" AS u
INNER JOIN ""ProvidentFund"" AS p ON u.""UserId"" = p.""UserId""
inner join ""AspNetUsers"" s on s.""Id""=u.""UserId"";");

        public static CreateViewSql UserViewV2 = new CreateViewSql(
            $@"
            create or replace view ""{TableConstraint.UserView}"" as SELECT a.""Id"",
    a.""EmployeeCode"",
    a.""Email"",
    a.""Firstname"",
    a.""Surname"",
    a.""Prefix"",
    u.""Department"",
    u.""CurrentPosition"",
    u.""ManagerId"",
    NOT a.""Resigned"" AS ""Active"",
	(CURRENT_TIMESTAMP < a.""LockoutEnd"" and a.""LockoutEnd"" is not null ) as ""Lock""
   FROM ""AspNetUsers"" a
     LEFT JOIN ""UserInformations"" u ON a.""Id"" = u.""UserId"";
            ",
            $@"
            create or replace view ""{TableConstraint.UserView}"" as SELECT a.""Id"", a.""EmployeeCode"", a.""Email"", a.""Firstname"", a.""Surname"", a.""Prefix"", u.""Department"", u.""CurrentPosition"", u.""ManagerId"", NOT (a.""Resigned"") as ""Active"" FROM ""AspNetUsers"" AS a LEFT JOIN ""UserInformations"" AS u ON a.""Id"" = u.""UserId""
            "
            );

        public static CreateViewSql UserViewV3 = new CreateViewSql($@"
  create or replace view ""{TableConstraint.UserView}"" as SELECT a.""Id"",
    a.""EmployeeCode"",
    a.""Email"",
    a.""Firstname"",
    a.""Surname"",
    a.""Prefix"",
    u.""Department"",
    u.""CurrentPosition"",
    u.""ManagerId"",
    NOT a.""Resigned"" AS ""Active"",
	(CURRENT_TIMESTAMP < a.""LockoutEnd"" and a.""LockoutEnd"" is not null ) as ""Lock"",
	case  
	WHEN CURRENT_TIMESTAMP < a.""LockoutEnd"" AND a.""LockoutEnd"" IS NOT NULL = True
	then a.""LockoutEnd"" - CURRENT_TIMESTAMP
	ELSE null
	end as ""LockRemaining""
   FROM ""AspNetUsers"" a
     LEFT JOIN ""UserInformations"" u ON a.""Id"" = u.""UserId"";
",
            $@"
  create or replace view ""{TableConstraint.UserView}"" as SELECT a.""Id"",
    a.""EmployeeCode"",
    a.""Email"",
    a.""Firstname"",
    a.""Surname"",
    a.""Prefix"",
    u.""Department"",
    u.""CurrentPosition"",
    u.""ManagerId"",
    NOT a.""Resigned"" AS ""Active"",
	(CURRENT_TIMESTAMP < a.""LockoutEnd"" and a.""LockoutEnd"" is not null ) as ""Lock""
   FROM ""AspNetUsers"" a
     LEFT JOIN ""UserInformations"" u ON a.""Id"" = u.""UserId"";
");
        #endregion

        #region Audit Log
        #endregion

        #region Warehouse
        public static CreateViewSql CreateAddrView = new CreateViewSql(
            @$"create or replace view ""{TableConstraint.AddressView}"" as SELECT d.""Code"" AS ""DistrictCode"", d.""Id"" AS ""DistrictId"", d.""NameEN"" AS ""DistrictNameEN"", d.""NameTH"" AS ""DistrictNameTH"", s.""Code"" AS ""Id"", s.""Latitude"", s.""Longitude"", s.""PostalCode"", p.""Code"" AS ""ProvinceCode"", p.""Id"" AS ""ProvinceId"", p.""NameEN"" AS ""ProvinceNameEN"", p.""NameTH"" AS ""ProvinceNameTH"", s.""Id"" AS ""SubDistrictId"", s.""NameEN"" AS ""SubDistrictNameEN"", s.""NameTH"" AS ""SubDistrictNameTH"" FROM ""Provinces"" AS p INNER JOIN ""Districts"" AS d ON p.""Id"" = d.""ProvinceId"" INNER JOIN ""SubDistricts"" AS s ON d.""Id"" = s.""DistrictId""; ",
            $"drop view \"{TableConstraint.AddressView}\"");
        #endregion
    }


    public sealed record CreateViewSql(string Up,string Down);
}
