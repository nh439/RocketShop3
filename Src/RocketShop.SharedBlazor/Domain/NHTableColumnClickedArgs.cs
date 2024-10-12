using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.SharedBlazor.Domain
{
    public sealed class NHTableColumnClickedArgs (string value,int index)
    {
        public readonly string Value = value;
        public readonly int Index = index;
    }
}
