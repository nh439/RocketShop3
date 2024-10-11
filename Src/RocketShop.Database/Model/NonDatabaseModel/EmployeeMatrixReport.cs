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
    public class EmployeeMatrixReport 
    {
        [NHAutoTableSkipColumn]
        public string UserId { get; set; }

        [NHAutoTableColumnDisplay("Employee Id")]
        public string EmployeeCode { get; set; }

        public string? Prefix { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }

        [NHAutoTableTrueDisplay("<i class='check icon' />")]
        [NHAutoTableFalseDisplay("<i class='x icon' />")]
        public bool Active { get; set; }

        [NHAutoTableColumnDisplay("Create Date")]
        [NHAutoTableDateTimeFormatDisplay("yyyy-MM-dd HH:mm:ss")]
        public DateTime CreateDate { get; set; }

        [NHAutoTableColumnDisplay("Create By")]
        [NHAutoTableNullDisplay("SYSTEM")]
        public string? CreateByName { get; set; }
        public string Email { get; set; }

        [NHAutoTableColumnDisplay("Has Information")]
        [NHAutoTableTrueDisplay("<i class='check icon' />", "Yes")]
        [NHAutoTableFalseDisplay("<i class='x icon' />", "No")]
        public bool HasInformation { get; set; }

        [NHAutoTableColumnDisplay("Has Financial Data")]
        [NHAutoTableTrueDisplay("<i class='check icon' />", "Yes")]
        [NHAutoTableFalseDisplay("<i class='x icon' />", "No")]
        public bool HasFinancial { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }

        [NHAutoTableColumnDisplay("Manager Name")]
        [NHAutoTableNullDisplay("No Manager", "No Manager")]
        public string? ManagerName { get; set; }

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