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
    /// ORM entity for logging to a corresponding SQL server table.
    /// The same table should exist in the SQL Server database as well as a corresponding LINQ to SQL entity type with the same fields.
    /// </summary>
    public class ServerAction
    {
        #region Properties

        public int ServerActionId { get; set; }

        public string Function { get; set; }

        public Nullable<Guid> UserId { get; set; }

        public string UserName { get; set; }

        public string EntityChanged { get; set; }

        public string FieldChanged { get; set; }

        public string OriginalValue { get; set; }

        public string NewValue { get; set; }

        public string Comments { get; set; }

        public DateTime DateCreated { get; set; }

        #endregion //Properties
    }
}
