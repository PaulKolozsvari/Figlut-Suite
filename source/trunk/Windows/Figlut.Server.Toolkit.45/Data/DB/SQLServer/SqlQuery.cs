namespace Figlut.Server.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;

    #endregion //Using Directives

    public class SqlQuery : Query
    {
        #region Constructors

        public SqlQuery() : base()
        {
        }

        public SqlQuery(SqlQueryKeyword keyword) : base(keyword)
        {
        }

        public SqlQuery(string sqlQueryString) : base(sqlQueryString)
        {
        }

        #endregion //Constructors

        #region Methods

        public override void AppendWhereColumns(List<WhereClauseColumn> whereClause)
        {
            _sqlQueryString.AppendLine("WHERE");
            foreach (WhereClauseColumn whereColumn in whereClause)
            {
                string whereColumnName = whereColumn.ColumnName;
                if (whereColumn.UseParameter)
                {
                    string parameterName = string.Format("@{0}", DataShaper.GetUniqueIdentifier());
                    if (SqlParameterExists(_sqlParameters, parameterName))
                    {
                        throw new Exception(string.Format("Parameter with name {0} already added for where column {1}.", whereColumnName));
                    }
                    _sqlParameters.Add(new SqlParameter(parameterName, whereColumn.ColumnValue));
                    _sqlQueryString.Append(string.Format("[{0}] {1} {2}",
                        whereColumnName,
                        whereColumn.ComparisonOperator.ToString(),
                        parameterName));
                }
                else
                {
                    string value = whereColumn.WrapValueWithQuotes ? string.Format("''", whereColumn.ColumnValue) : string.Format("{0}", whereColumn.ColumnValue);
                    _sqlQueryString.Append(string.Format("[{0}] {1} {2}", whereColumnName, whereColumn.ComparisonOperator.ToString(), value));
                }
                if (whereColumn.LogicalOperatorAgainstNextColumn == null)
                {
                    _sqlQueryString.Append("");
                    break;
                }
                _sqlQueryString.AppendLine(string.Format(" {0}", whereColumn.LogicalOperatorAgainstNextColumn.ToString()));
            }
            whereClause.ForEach(w => _whereClause.Add(w));
        }

        #endregion //Methods
    }
}
