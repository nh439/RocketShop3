using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
namespace RocketShop.Database.Model.Warehouse.Authorization
{
    [Table(TableConstraint.Client)]
    public class Client
    {
        [Key]
        public long Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public int? TokenExpiration { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime? LockUntil { get; set; }
        public DateTime? Updated { get; set;} = DateTime.UtcNow;
        public string? CreateBy { get; set; }
        public string? UpdateBy { get; set; }
        public string? Application { get; set; }
        public int? MaxinumnAccess { get; set; }
        public bool RequireSecret { get; set; } = true;
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.