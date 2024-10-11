using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Attribute
{
    public class NHAutoTableNewLineSeparator(string separator) :System.Attribute
    {
        public string Separator = separator;
    }
}
