namespace Figlut.Server.Toolkit.Data.DB.LINQ.Logging
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    /// <summary>
    /// ORM entity for managing users and logging against their name to a corresponding SQL server table.
    /// The same table should exist in the SQL Server database as well as a corresponding LINQ to SQL entity type with the same fields.
    /// </summary>
    public class User
    {
        #region Properties

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public DateTime DateCreated { get; set; }

        #endregion //Properties
    }
}
