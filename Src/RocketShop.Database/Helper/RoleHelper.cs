using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;
using RocketShop.Database.Model.Identity;
using RocketShop.Framework.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Helper
{
    public static class RoleHelper
    {
        public static string[] ListAllPermission() =>
                typeof(Role)
                .GetProperties()
            .Where(x=>x.Name != nameof(Role.RoleName))
                .Select(p=>p.Name)
                .ToArray();

        public static List<string> GetAllowedPermission(this Role role)
        {
            List<string> returnValues = new List<string>();
            var prop = typeof(Role)
                .GetProperties();
            prop.HasDataAndForEach(x =>
            {
                var val = x.GetValue(role);
                if (val?.GetType() == typeof(bool) && ((bool?)val).IsTrue()) 
                    returnValues.Add(x.Name);
                
            });
            return returnValues;
        }

    }
}
