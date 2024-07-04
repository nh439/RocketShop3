using LanguageExt;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Model.NonDatabaseModel
{
    public sealed record UserProfile(
        string Id,
        string EmployeeCode,
        string Firstname,
        string Surname,
        string? Email,
        UserInformation? Information = null
        )
    {
        public string GetGender()
        {
            if (Information.IsNull() || !Information!.Gender.HasValue)
                return "Not specified";
            return Information.Gender.If(x => x == 'F',"Female","Male")!;
        }
    }
}
