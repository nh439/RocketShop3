using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace RocketShop.Database.Model.Identity
{
    [Table(TableConstraint.User)]
    public sealed class User : IdentityUser
    {
        [Required]

        public string EmployeeCode { get; set; }
        [Required]
        public string Firstname { get; set; }
        [Required]
        public string Surname { get; set; }
        public string? Prefix { get; set; }
        public bool Resigned { get; set; } = false;
        public bool EmailVerified { get; set; } = false;
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoggedIn { get; set; }
        public DateTime? LastLoggedOut { get; set; }
        
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
