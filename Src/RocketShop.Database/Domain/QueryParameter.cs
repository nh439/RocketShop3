using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Database.Domain
{
    public sealed record QueryParameter(string paramName, object value);
}
