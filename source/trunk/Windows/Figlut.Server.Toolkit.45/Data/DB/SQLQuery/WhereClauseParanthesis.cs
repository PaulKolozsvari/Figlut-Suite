namespace Figlut.Server.Toolkit.Data.DB.SQLQuery
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class WhereClauseParanthesis : IEnumerable<WhereClauseColumn>
    {
        #region Constructors

        public WhereClauseParanthesis()
        {
            _whereClauseColumns = new List<WhereClauseColumn>();
        }

        #endregion //Constructors

        #region Fields

        private List<WhereClauseColumn> _whereClauseColumns;
        private WhereClauseLogicalOperator _whereClauseLogicalOperatorAgainsNextParanthesis;

        #endregion //Fields

        #region Properties

        public List<WhereClauseColumn> WhereClauseColumns
        {
            get { return _whereClauseColumns; }
            set { _whereClauseColumns = value; }
        }

        public WhereClauseLogicalOperator LogicalOperatorAgainstNextParanthesis
        {
            get { return _whereClauseLogicalOperatorAgainsNextParanthesis; }
            set { _whereClauseLogicalOperatorAgainsNextParanthesis = value; }
        }

        public int Count
        {
            get { return _whereClauseColumns.Count; }
        }

        #endregion //Properties

        #region Methods

        public void Add(WhereClauseColumn whereClauseColumn)
        {
            _whereClauseColumns.Add(whereClauseColumn);
        }

        public override string ToString()
        {
            if (_whereClauseColumns == null || _whereClauseColumns.Count < 1)
            {
                return string.Empty;
            }
            StringBuilder result = new StringBuilder();
            result.Append("(");
            for (int i = 0; i < _whereClauseColumns.Count; ++i)
            {
                WhereClauseColumn c = _whereClauseColumns[i];
                result.Append(c.ToString());
                if (i < (_whereClauseColumns.Count - 1)) //Not the last where clause column in the where clause paranthesis.
                {
                    result.Append("_");
                }
            }
            result.Append(")");
            if (LogicalOperatorAgainstNextParanthesis != null)
            {
                result.AppendFormat("_{0}", LogicalOperatorAgainstNextParanthesis.Value);
            }
            return result.ToString();
        }

        public IEnumerator<WhereClauseColumn> GetEnumerator()
        {
            return _whereClauseColumns.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion //Methods
    }
}