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
    [Table(TableConstraint.UserInformation)]
    public sealed class UserInformation
    {
        [Key]
        public string UserId { get; set; }
        public DateTime? BrithDay { get; set; }
        public DateTime? ResignDate { get; set; }
        public DateTime? StartWorkDate { get; set; }
        public string? Department { get; set; }
        public string? CurrentPosition { get; set; }
        public string? ManagerId { get; set; }
        public char? Gender { get; set; }
    }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.