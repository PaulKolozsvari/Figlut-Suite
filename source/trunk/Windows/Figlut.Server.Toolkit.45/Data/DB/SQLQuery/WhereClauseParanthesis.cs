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
            foreach (WhereClauseColumn c in _whereClauseColumns)
            {
                result.Append(c.ToString());
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