using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.AuditService.Domain
{
    public class ActivityLogAdvanceSearch
    {
        public DateTime? DateFrom {  get; set; }
        public DateTime? DateTo { get; set; }
        public string? Actor {  get; set; }
        public string? Division { get; set; }
        public string? ServiceName { get; set; }
        public string? Email { get; set; }
    }
}
