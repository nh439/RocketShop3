using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.SharedBlazor.Domain
{
    /// <summary>
    /// Represents the arguments passed when a table column is clicked, including the value and index,
    /// with a human-readable column index.
    /// </summary>
    public sealed class NHTableColumnClickedArgs (string value,int index)
    {
        /// <summary>
        /// The value associated with the clicked column.
        /// </summary>
        public readonly string Value = value;
        /// <summary>
        /// The human-readable (1-based) index of the clicked column.
        /// </summary>
        public readonly string HumanIndex = 1 + value;
        /// <summary>
        /// The zero-based index of the clicked column.
        /// </summary>
        public readonly int Index = index;
        /// <summary>
        /// Returns a string representation of the clicked column's data.
        /// </summary>
        public override string ToString() => $"{Value},{HumanIndex}";
     
    }
}
