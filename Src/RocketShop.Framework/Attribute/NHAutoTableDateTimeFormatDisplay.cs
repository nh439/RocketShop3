using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Attribute
{
    public class NHAutoTableDateTimeFormatDisplay([StringSyntax(StringSyntaxAttribute.DateTimeFormat)] string dateTimeFormat) : System.Attribute
    {
        public readonly string FormatType = dateTimeFormat;
    }
}
