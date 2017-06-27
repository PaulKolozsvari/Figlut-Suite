namespace Figlut.Server.Toolkit.Data.DB.SQLQuery
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using System.Data.SqlClient;

    #endregion //Using Directives

    public enum SqlQueryKeyword
    {
        SELECT,
        INSERT,
        UPDATE,
        DELETE,
        FROM,
        WHERE,
        NULL
    }

    public class SqlQuery
    {
        #region Constructors

        public SqlQuery()
        {
            _sqlQueryString = new StringBuilder();
            _sqlParameters = new List<SqlParameter>();
            _tableNamesInQuery = new List<string>();
            _whereClause = new List<WhereClauseColumn>();
        }

        public SqlQuery(SqlQueryKeyword keyword)
        {
            _sqlQueryString = new StringBuilder();
            _sqlParameters = new List<SqlParameter>();
            _tableNamesInQuery = new List<string>();
            _whereClause = new List<WhereClauseColumn>();
            _sqlQueryString.AppendLine(keyword.ToString());
        }

        public SqlQuery(string sqlQueryString)
        {
            _sqlQueryString = new StringBuilder(sqlQueryString);
            _sqlParameters = new List<SqlParameter>();
            _tableNamesInQuery = new List<string>();
            _whereClause = new List<WhereClauseColumn>();
        }

        #endregion //Constructors

        #region Fields

        protected StringBuilder _sqlQueryString;
        protected List<SqlParameter> _sqlParameters;
        protected bool _firstSelectColumn = true;
        protected bool _firstFromTable = true;
        protected List<WhereClauseColumn> _whereClause;
        protected List<string> _tableNamesInQuery;

        #endregion //Fields

        #region Properties

        public string SqlQuerySring
        {
            get { return _sqlQueryString.ToString(); }
        }

        public List<SqlParameter> SqlCeParameters
        {
            get { return _sqlParameters; }
        }

        public List<string> TableNamesInQuery
        {
            get { return _tableNamesInQuery; }
        }

        public List<WhereClauseColumn> WhereClause
        {
            get { return _whereClause; }
        }

        #endregion //Properties

        #region Methods

        public void Clear()
        {
            _sqlQueryString = new StringBuilder();
            _sqlParameters.Clear();
        }

        public void AppendStatement(string sqlStatement)
        {
            _sqlQueryString.AppendLine(sqlStatement);
        }


        public void AppendKeyword(SqlQueryKeyword keyword)
        {
            _sqlQueryString.AppendLine(keyword.ToString());
        }

        public void AppendSelectColumn(string columnName)
        {
            if (!_firstSelectColumn)
            {
                _sqlQueryString.AppendLine(",");
            }
            _sqlQueryString.Append(string.Format("[{0}]", columnName));
            _firstSelectColumn = false;
        }

        public void AppendSelectColumn(List<string> columnNames)
        {
            columnNames.ForEach(c => AppendSelectColumn(c));
            _sqlQueryString.AppendLine();
        }

        public void AppendSelectColumn(Type entityType, bool appendFromStatement)
        {
            PropertyInfo[] entityProperties = entityType.GetProperties();
            if (entityProperties.Length < 1)
            {
                throw new ArgumentException(string.Format(
                    "{0} has no properties to update in the local database.",
                    entityType.FullName));
            }
            List<string> selectColumns = new List<string>();
            foreach (PropertyInfo p in entityProperties)
            {
                if ((p.PropertyType != typeof(string) &&
                    p.PropertyType != typeof(byte) &&
                    p.PropertyType != typeof(byte[])) &&
                    (p.PropertyType.IsClass ||
                    p.PropertyType.IsEnum ||
                    p.PropertyType.IsInterface ||
                    p.PropertyType.IsNotPublic ||
                    p.PropertyType.IsPointer))
                {
                    continue;
                }
                selectColumns.Add(p.Name);
            }
            AppendSelectColumn(selectColumns);
            if (appendFromStatement)
            {
                AppendTable(entityType, true);
            }
        }

        public void AppendSelectColumn<E>(bool appendFromStatement) where E : class
        {
            AppendSelectColumn(typeof(E), appendFromStatement);
        }

        public void AppendTable(string tableName, bool prefixWithFromKeyword)
        {
            if (prefixWithFromKeyword)
            {
                AppendKeyword(SqlQueryKeyword.FROM);
            }
            if (!_firstFromTable)
            {
                _sqlQueryString.AppendLine(",");
            }
            _sqlQueryString.AppendLine(string.Format("[{0}]", tableName));
            _firstFromTable = false;
            _tableNamesInQuery.Add(tableName);
        }

        public void AppendTable(Type entityType, bool prefixWithFromKeyword)
        {
            AppendTable(entityType.Name, prefixWithFromKeyword);
        }

        public void AppendTable<E>(bool prefixWithFromKeyword) where E : class
        {
            AppendTable(typeof(E).Name, prefixWithFromKeyword);
        }

        public void AppendWhereColumns(List<WhereClauseColumn> whereClause)
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

        public bool SqlParameterExists(List<SqlParameter> sqlParameters, string parameterName)
        {
            foreach (SqlParameter p in sqlParameters)
            {
                if (p.ParameterName == parameterName)
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return _sqlQueryString.ToString();
        }

        #endregion //Methods
    }
}