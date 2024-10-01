using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.AuditLog.Base
{
    public class BaseLogging
    {
        [Key]
        public string Id { get; set; }=Guid.NewGuid().ToString();
        public DateTime LogDate { get; set; }=DateTime.UtcNow;
        public string Actor { get; set; }
        /// <summary>
        /// Project Name (e.g Identity,Retail)
        /// </summary>
        public string Division { get; set; }
        /// <summary>
        /// Service Name (e.g Financial Service)
        /// </summary>
        public string ServiceName { get; set; }
        public string ActorName { get; set; }
        public string ActorEmail { get; set; }
    }
}
