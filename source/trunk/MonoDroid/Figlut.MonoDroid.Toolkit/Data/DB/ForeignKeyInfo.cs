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
    public class ForeignKeyInfo
    {
        #region Constructors

        public ForeignKeyInfo()
        {
        }

        #endregion //Constructors

        #region Fields

        protected string _childTableName;
        protected string _childTableForeignKeyName;
        protected string _parentTableName;
        protected string _parentTablePrimaryKeyName;
        protected string _constraintName;

        #endregion //Fields

        #region Properties

        public string ChildTableName
        {
            get { return _childTableName; }
            set { _childTableName = value; }
        }

        public string ChildTableForeignKeyName
        {
            get { return _childTableForeignKeyName; }
            set { _childTableForeignKeyName = value; }
        }

        public string ParentTableName
        {
            get { return _parentTableName; }
            set { _parentTableName = value; }
        }

        public string ParentTablePrimaryKeyName
        {
            get { return _parentTablePrimaryKeyName; }
            set { _parentTablePrimaryKeyName = value; }
        }

        public string ConstraintName
        {
            get { return _constraintName; }
            set { _constraintName = value; }
        }

        #endregion //Properties
    }
}