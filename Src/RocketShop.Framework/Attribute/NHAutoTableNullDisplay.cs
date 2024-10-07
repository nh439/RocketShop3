using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Attribute
{
    public class NHAutoTableNullDisplay(string displayWhileNull) :System.Attribute
    {
        public readonly string DisplayWhileNull=displayWhileNull;
    }
}
