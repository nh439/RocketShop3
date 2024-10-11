using RocketShop.Framework.Extension;
using RocketShop.Framework.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace RocketShop.Database.Model.NonDatabaseModel
{
    public class EmployeeMatrixReport :EmployeeDataReport
    {
        [NHAutoTableNewLineSeparator(",")]
        public string Roles { get; set; }
        [NHAutoTableNewLineSeparator(",")]
        public string Permissions { get; set; }
        public EmployeeMatrixReport() { }
        public EmployeeMatrixReport(EmployeeDataReport report,
            IEnumerable<string>? roles = null,
            IEnumerable<string>? permission = null)
        {
            UserId = report.UserId;
            EmployeeCode = report.EmployeeCode;
            Prefix = report.Prefix;
            Firstname = report.Firstname;
            Surname = report.Surname;
            Active = report.Active;
            CreateDate = report.CreateDate;
            CreateByName=report.CreateByName;
            Email =report.Email;
            HasInformation = report.HasInformation;
            HasFinancial = report.HasFinancial;
            Position = report.Position;
            Department = report.Department;
            ManagerName = report.ManagerName;
            if(roles.HasData())
                Roles = string.Join(",", roles!.Select(s=>$"- {s}")!);
            if(permission.HasData())
                Permissions = string.Join(",",permission!.Select(s => $"- {s}"));
        }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.