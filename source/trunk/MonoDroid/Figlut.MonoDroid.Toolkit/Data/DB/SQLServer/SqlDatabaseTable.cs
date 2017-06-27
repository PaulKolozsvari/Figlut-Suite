namespace Figlut.MonoDroid.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Figlut.MonoDroid.Toolkit.Data.DB.SQLQuery;
    using System.Data;
    using System.Data.Common;

    #endregion //Using Directives

    [Serializable]
    public class SqlDatabaseTable<E> : SqlDatabaseTable where E : class
    {
        #region Constructors

        public SqlDatabaseTable() : base()
        {
        }

        public SqlDatabaseTable(string tableName) 
            : base(tableName)
        {
        }

        public SqlDatabaseTable(DataRow schemaRow) 
            : base(schemaRow)
        {
        }

        #endregion //Constructors

        #region Methods


        #endregion //Methods
    }
}