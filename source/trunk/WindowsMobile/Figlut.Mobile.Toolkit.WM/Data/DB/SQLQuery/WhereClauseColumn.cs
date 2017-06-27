namespace Figlut.Mobile.Toolkit.Data.DB.SQLQuery
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;

    #endregion //Using Directives

    public class WhereClauseColumn
    {
        #region Constructors

        public WhereClauseColumn(
            string columnName,
            WhereClauseComparisonOperator comparisonOperator,
            object columnValue,
            bool useParameter,
            bool wrapValueWithQuotes)
        {
            _columnName = columnName;
            _comparisonOperator = comparisonOperator;
            _columnValue = columnValue;
            _useParameter = useParameter;
            _wrapValueWithQuotes = wrapValueWithQuotes;
        }

        public WhereClauseColumn(
            string columnName,
            WhereClauseComparisonOperator comparisonOperator,
            object columnValue,
            bool useParameter,
            bool wrapValueWithQuotes,
            WhereClauseLogicalOperator logicalOperatorAgainstNextColumn)
        {
            _columnName = columnName;
            _comparisonOperator = comparisonOperator;
            _columnValue = columnValue;
            _useParameter = useParameter;
            _wrapValueWithQuotes = wrapValueWithQuotes;
            LogicalOperatorAgainstNextColumn = logicalOperatorAgainstNextColumn;
        }

        #endregion //Constructors

        #region Fields

        protected string _columnName;
        protected WhereClauseComparisonOperator _comparisonOperator;
        protected object _columnValue;
        protected bool _useParameter;
        protected bool _wrapValueWithQuotes;
        protected WhereClauseLogicalOperator _logicalOperatorAgainstNextColumn;

        #endregion //Fields

        #region Properties

        public string ColumnName
        {
            get { return _columnName; }
            set { _columnName = value; }
        }

        public WhereClauseComparisonOperator ComparisonOperator
        {
            get { return _comparisonOperator; }
            set { _comparisonOperator = value; }
        }

        public object ColumnValue
        {
            get { return _columnValue; }
            set { _columnValue = value; }
        }

        public bool UseParameter
        {
            get { return _useParameter; }
            set { _useParameter = value; }
        }

        public bool WrapValueWithQuotes
        {
            get { return _wrapValueWithQuotes; }
            set { _wrapValueWithQuotes = value; }
        }

        public WhereClauseLogicalOperator LogicalOperatorAgainstNextColumn
        {
            get { return _logicalOperatorAgainstNextColumn; }
            set { _logicalOperatorAgainstNextColumn = value; }
        }

        #endregion //Properties

        #region Methods

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.AppendFormat(
                "{0}_{1}_{2}",
                ColumnName,
                ComparisonOperator.ToString(),
                ColumnValue);
            if (LogicalOperatorAgainstNextColumn != null)
            {
                result.AppendFormat("_{0}", LogicalOperatorAgainstNextColumn.Value);
            }
            return result.ToString();
        }

        #endregion //Methods
    }
}
