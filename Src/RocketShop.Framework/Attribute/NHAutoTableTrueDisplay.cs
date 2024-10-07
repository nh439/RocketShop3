using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Attribute
{
    //[System.AttributeUsage(System.AttributeTargets.Struct)]
    public class NHAutoTableTrueDisplay(string htmlContent) : System.Attribute
    {
        public readonly string HtmlContent= htmlContent;
    }
     //[System.AttributeUsage(System.AttributeTargets.Struct)]
    public class NHAutoTableFalseDisplay(string htmlContent) : System.Attribute
    {
        public readonly string HtmlContent= htmlContent;
    }

}
