using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity.Views
{
    public class UserView
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        [Column("Id")]
        public string UserId { get; init; }
        public string EmployeeCode { get; init; }
        public string Email { get; init; }
        public string Firstname { get; init; }
        public string Surname { get; init; }
        public string? Prefix { get; init; }
        public string? Department { get; init; }
        [Column("CurrentPosition")]
        public string? Position { get; init; }
        public string? ManagerId { get; init; }
        public bool Active { get; init; }
        public bool Lock { get; init; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
