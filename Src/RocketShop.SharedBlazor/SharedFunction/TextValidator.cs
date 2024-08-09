using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.SharedBlazor.SharedFunction
{
    public static class TextValidator
    {
        public static string InvalidDisplay(bool inValidResult) =>
            inValidResult ? "is-invalid" : string.Empty;
    }
}
