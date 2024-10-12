using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.SharedBlazor.Domain
{
    /// <summary>
    /// Represents the arguments passed when a table row cell is clicked, including the value, 
    /// variable name, row, column, and position, as well as human-readable row and column positions.
    /// </summary>
    public sealed class NHAutoTableRowCellClickedArgs(string value,string variableName,long row,long column,long position)
    {
        /// <summary>
        /// The value contained in the clicked cell.
        /// </summary>
        public readonly string Value  = value;
        /// <summary>
        /// The name of the variable associated with the clicked cell.
        /// </summary>
        public readonly string VariableName = variableName;
        /// <summary>
        /// The zero-based index of the row.
        /// </summary>
        public readonly long Row =row;
        /// <summary>
        /// The zero-based index of the column.
        /// </summary>
        public readonly long Column =column;
        /// <summary>
        /// The position of the clicked cell.
        /// </summary>
        public readonly long Position =position;
        /// <summary>
        /// The human-readable row number (1-based index).
        /// </summary>
        public readonly long HumanRow = 1 + row;
        /// <summary>
        /// The human-readable column number (1-based index).
        /// </summary>
        public readonly long HumanColumn = 1 + column;
        /// <summary>
        /// The human-readable position (1-based index).
        /// </summary>
        public readonly long HumanPosition = 1 + position;
        public override string ToString() => $"{VariableName}->{Value},{HumanPosition}({HumanRow},{HumanColumn})";
    }
}
