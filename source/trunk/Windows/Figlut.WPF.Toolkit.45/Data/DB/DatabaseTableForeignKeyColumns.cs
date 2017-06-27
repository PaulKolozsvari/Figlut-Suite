namespace Figlut.Server.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    [Serializable]
    public class DatabaseTableForeignKeyColumns
    {
        #region Constructors

        public DatabaseTableForeignKeyColumns()
        {
            _foreignKeys = new List<ForeignKeyInfo>();
        }

        public DatabaseTableForeignKeyColumns(string tableName)
        {
            _tableName = tableName;
            _foreignKeys = new List<ForeignKeyInfo>();
        }

        public DatabaseTableForeignKeyColumns(string tableName, List<ForeignKeyInfo> foreignKeys)
        {
            _tableName = tableName;
            _foreignKeys = foreignKeys;
        }

        #endregion //Constructors

        #region Fields

        protected string _tableName;
        protected List<ForeignKeyInfo> _foreignKeys;

        #endregion //Fields

        #region Properties

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public List<ForeignKeyInfo> ForeignKeys
        {
            get { return _foreignKeys; }
            set { _foreignKeys = value; }
        }

        #endregion //Properties
    }
}