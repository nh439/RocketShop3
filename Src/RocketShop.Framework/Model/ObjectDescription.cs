using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Model
{
    public class ObjectDescription
    {
        public string Name { get; internal set; }
        public Type Type { get; internal set; }
        public bool IsSpecialName { get; internal set; }
        public bool CanWrite { get; internal set; }
        public bool CanRead { get; internal set; }
        public PropertyAttributes Attributes { get; internal set; }
    }
}
