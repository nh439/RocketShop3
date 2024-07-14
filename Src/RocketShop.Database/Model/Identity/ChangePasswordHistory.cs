using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity
{
    [Table(TableConstraint.ChangePasswordHistory)]
    public class ChangePasswordHistory
    {
        [Key]
        public string Id { get; set; }  = Guid.NewGuid().ToString();
        public string UserId { get; set; } = string.Empty;
        public DateTime ChangeAt { get; set; } = DateTime.Now;
        public bool Reset { get; set; }=false;
    }
}
