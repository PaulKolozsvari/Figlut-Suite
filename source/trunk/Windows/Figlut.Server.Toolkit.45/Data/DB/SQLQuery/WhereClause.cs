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

    public class WhereClause : IEnumerable<WhereClauseParanthesis>
    {
        #region Constructors

        public WhereClause()
        {
            _whereClauseParanthesisList = new List<WhereClauseParanthesis>();
        }

        #endregion //Constructors

        #region Fields

        public List<WhereClauseParanthesis> _whereClauseParanthesisList;

        #endregion //Fields

        #region Properties

        public List<WhereClauseParanthesis> WhereClauseParanthesisList
        {
            get { return _whereClauseParanthesisList; }
            set { _whereClauseParanthesisList = value; }
        }

        public int Count
        {
            get { return _whereClauseParanthesisList.Count; }
        }

        #endregion //Properties

        #region Methods

        public void Add(WhereClauseParanthesis whereClauseParanthesis)
        {
            _whereClauseParanthesisList.Add(whereClauseParanthesis);
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (WhereClauseParanthesis p in _whereClauseParanthesisList)
            {
                result.AppendLine(p.ToString());
            }
            return result.ToString();
        }

        public IEnumerator<WhereClauseParanthesis> GetEnumerator()
        {
            return _whereClauseParanthesisList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion //Methods
    }
}
