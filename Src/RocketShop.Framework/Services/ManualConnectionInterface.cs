using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketShop.Framework.Services
{
    /// <summary>
    /// Clears the current database connection, releasing resources and resetting the connection state.
    /// </summary>
    public interface ManualConnectionInterface
    {
        /// <summary>
        /// Clears the current database connection, releasing resources and resetting the connection state.
        /// </summary>
        void ClearConnection();
        /// <summary>
        /// Overrides the current database connection with a new one.
        /// </summary>
        /// <param name="newConnection">The new database connection to use.</param>
        void OverrideConnection(IDbConnection newConnection);
        /// <summary>
        /// Opens the database connection, establishing a connection to the data source.
        /// </summary>
        void OpenConnection();
        /// <summary>
        /// Closes the database connection, releasing associated resources.
        /// </summary>
        void CloseConnection();
        /// <summary>
        /// Retrieves the current database connection instance.
        /// </summary>
        /// <returns>The active <see cref="IDbConnection"/> instance, or <c>null</c> if no connection is set.</returns>
        IDbConnection? GetConnection();
    }
}
