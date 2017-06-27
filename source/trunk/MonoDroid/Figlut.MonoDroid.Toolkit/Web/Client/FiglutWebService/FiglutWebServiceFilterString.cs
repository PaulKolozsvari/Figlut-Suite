namespace Figlut.MonoDroid.Toolkit.Web.Client.FiglutWebService
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.MonoDroid.Toolkit.Data.DB.SQLQuery;

    #endregion //Using Directives

    public class FiglutWebServiceFilterString
    {
        #region Constructors

        public FiglutWebServiceFilterString(string tableName, List<WhereClauseColumn> filters)
        {
            StringBuilder filterString = new StringBuilder(string.Format("sqltable/{0}", tableName));
            if (filters != null && filters.Count > 0)
            {
                filterString.Append("/");
                for (int i = 0; i < filters.Count; i++)
                {
                    WhereClauseColumn w = filters[i];
                    filterString.AppendFormat(
                        "{0}_{1}_{2}",
                        w.ColumnName,
                        w.ComparisonOperator.ToString(),
                        w.ColumnValue);
                    if (w.LogicalOperatorAgainstNextColumn != null)
                    {
                        filterString.AppendFormat("_{1}");
                    }
                    if (i < (filters.Count - 1)) //This is the last filter.
                    {
                        filterString.AppendFormat(",");
                    }
                }
            }
            _filterString = filterString.ToString();
        }

        #endregion //Constructors

        #region Fields

        protected string _filterString;

        #endregion //Fields

        #region Properties

        public string FilterString
        {
            get { return _filterString; }
        }

        #endregion //Properties

        #region Methods

        public override string ToString()
        {
            return _filterString;
        }

        #endregion //Methods
    }
}