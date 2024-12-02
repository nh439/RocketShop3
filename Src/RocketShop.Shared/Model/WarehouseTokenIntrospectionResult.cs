using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Shared.Model
{
    public class WarehouseTokenIntrospectionResult 
    {
        public string TokenKey { get; set; }
        public string ClientId { get; set; }
        public int? RemainingAccess { get; set; }
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public TimeSpan? TokenAge { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public bool Usable { get; set; }
        public bool TokenExpired { get; set; }
        public bool AccessLimitReached { get; set; }
        public string[]? AllowedReadCollections { get; set; }
        public string[]? AllowedWriteCollections { get; set; }

    }
}
