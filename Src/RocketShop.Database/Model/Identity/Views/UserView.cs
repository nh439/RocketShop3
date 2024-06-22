using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.Identity.Views
{
    public sealed record UserView(
        string UserId,
        string EmployeeCode,
        string Email,
        string Firstname,
        string Surname,
        string? Prefix,
        string? Department,
        string? Position,
        string? ManagerId,
        bool Active
        );
}
