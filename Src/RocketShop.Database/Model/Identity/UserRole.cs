using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity
{
    [Table(TableConstraint.UserRole)]
    public class UserRole
    {
        public string UserId { get; set; }
        public int RoleId { get; set; } 
    }
}
