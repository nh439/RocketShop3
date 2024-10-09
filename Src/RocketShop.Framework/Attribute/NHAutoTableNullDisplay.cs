using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Attribute
{
    public class NHAutoTableNullDisplay(string displayWhileNull,string? value = null) :System.Attribute
    {
        public readonly string DisplayWhileNull=displayWhileNull;
        public readonly string? Value=value;
    }
}
