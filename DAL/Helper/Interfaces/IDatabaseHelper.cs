using System;
using System.Data;

namespace DAL.Helper.Interfaces
{
    public interface IDatabaseHelper
    {
        /// <summary>
        /// Create and return an open DB connection.
        /// </summary>
        IDbConnection TConnection();
    }
}
