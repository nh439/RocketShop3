using RocketShop.Database.Model.AuditLog.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.AuditLog
{
    [Table(TableConstraint.ActivityLog)]
    public class ActivityLog :BaseLogging
    {
        public string LogDetail { get; set; }
    }
}
