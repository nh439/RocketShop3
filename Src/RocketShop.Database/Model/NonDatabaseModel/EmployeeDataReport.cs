using RocketShop.Framework.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.NonDatabaseModel
{
    public class EmployeeDataReport
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
        public DateTime CreateDate { get; set; }

        [NHAutoTableColumnDisplay("Create By")]
        [NHAutoTableNullDisplay("SYSTEM")]
        public string? CreateByName { get; set; }
        public string Email { get; set; }

        [NHAutoTableColumnDisplay("Has Information")]
        [NHAutoTableTrueDisplay("<i class='check icon' />","Yes")]
        [NHAutoTableFalseDisplay("<i class='x icon' />","No")]
        public bool HasInformation { get; set; }

        [NHAutoTableColumnDisplay("Has Financial Data")]
        [NHAutoTableTrueDisplay("<i class='check icon' />","Yes")]
        [NHAutoTableFalseDisplay("<i class='x icon' />","No")]
        public bool HasFinancial { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }

        [NHAutoTableColumnDisplay("Manager Name")]
        [NHAutoTableNullDisplay("No Manager", "No Manager")]
        public string? ManagerName { get; set; }

    }
}
