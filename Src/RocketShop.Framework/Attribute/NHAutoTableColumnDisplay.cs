using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Attribute
{
    public class NHAutoTableColumnDisplay(string columnDisplay) : System.Attribute
    {
      public readonly string ColumnDisplay = columnDisplay;
    }
}
