namespace Figlut.MonoDroid.Toolkit.Data.DB
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    [Serializable]
    public class DatabaseTableKeyColumns
    {
        #region Constructors

        public DatabaseTableKeyColumns()
        {
            _keyNames = new List<string>();
        }

        public DatabaseTableKeyColumns(string tableName)
        {
            _tableName = tableName;
            _keyNames = new List<string>();
        }

        public DatabaseTableKeyColumns(string tableName, List<string> keyNames)
        {
            _tableName = tableName;
            _keyNames = keyNames;
        }

        #endregion //Constructors

        #region Fields

        protected string _tableName;
        protected List<string> _keyNames;

        #endregion //Fields

        #region Properties

        public string TableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        public List<string> KeyNames
        {
            get { return _keyNames; }
            set { _keyNames = value; }
        }

        #endregion //Properties
    }
}