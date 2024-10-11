using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Attribute
{
    public class NHAutoTableNumberFormatDisplay([StringSyntax(StringSyntaxAttribute.NumericFormat)] string formatType) : System.Attribute
    {
        public readonly string FormatType = formatType;
    }

}
