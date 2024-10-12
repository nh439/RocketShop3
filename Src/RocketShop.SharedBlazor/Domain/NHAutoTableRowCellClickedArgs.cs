using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.SharedBlazor.Domain
{
    public sealed class NHAutoTableRowCellClickedArgs(string value,string variableName,long row,long column,long position)
    {
        public readonly string Value  = value;
        public readonly string VariableName = variableName;
        public long Row =row;
        public long Column =column;
        public long Position =position;
        public override string ToString() => $"{VariableName}->{Value},{Position}({Row},{Column})";
    }
}
