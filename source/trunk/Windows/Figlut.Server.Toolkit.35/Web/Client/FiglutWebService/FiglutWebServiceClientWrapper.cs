namespace Figlut.Server.Toolkit.Web.Client.FiglutWebService
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Web.Client;
    using Figlut.Server.Toolkit.Data;
using System.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Data.DB;
using Figlut.Server.Toolkit.Data.ORM;
    using System.Reflection.Emit;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    public class FiglutWebServiceClientWrapper
    {
        #region Constructors

        public FiglutWebServiceClientWrapper(IMimeWebServiceClient webServiceClient, int timeout)
        {
            if (webServiceClient == null)
            {
                throw new NullReferenceException(string.Format(
                    "{0} may not be set to null when constructing {1}.",
                    EntityReader<FiglutWebServiceClientWrapper>.GetPropertyName(p => p.WebServiceClient, false),
                    this.GetType().FullName));
            }
            _webServiceClient = webServiceClient;
            _timeout = timeout;
        }

        #endregion //Constructors

        #region Fields

        protected IMimeWebServiceClient _webServiceClient;
        protected int _timeout;

        #endregion //Fields

        #region Properties

        public IMimeWebServiceClient WebServiceClient
        {
            get { return _webServiceClient; }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException(string.Format(
                        "{0} may not be set to null on {1}.",
                        EntityReader<FiglutWebServiceClientWrapper>.GetPropertyName(p => p.WebServiceClient, false),
                        this.GetType().FullName));
                }
                _webServiceClient = value;
            }
        }

        #endregion //Properties

        #region Methods

        public T GetSqlSchema<T>(DatabaseSchemaInfoType schemaInfoType, string filters, out string rawOutput)
        {
            string queryString =
                string.IsNullOrEmpty(filters) ?
                string.Format("sqlschema/{0}", schemaInfoType.ToString()) :
                string.Format("sqlschema/{0}/{1}", schemaInfoType.ToString(), filters);
            return _webServiceClient.CallService<T>(
                queryString,
                null,
                HttpVerb.GET,
                out rawOutput,
                true,
                _timeout);
        }

        public SqlDatabase GetSqlDatabase(bool createOrmAssembly, bool saveOrmAssembly)
        {
            string rawOutput = string.Empty;
            SqlDatabase result = new SqlDatabase();
            result.Name = GetSqlSchema<string>(DatabaseSchemaInfoType.DatabaseName, null, out rawOutput);
            DataTable tablesSchema = GetSqlSchema<DataTable>(DatabaseSchemaInfoType.Tables, null, out rawOutput);
            Dictionary<string, DatabaseTableKeyColumns> tablesKeyColumns = new Dictionary<string, DatabaseTableKeyColumns>(); //Primary keys of all the tables.
            GetSqlSchema<List<DatabaseTableKeyColumns>>(DatabaseSchemaInfoType.TableKeyColumns, null, out rawOutput).ForEach(p => tablesKeyColumns.Add(p.TableName, p));
            foreach (DataRow t in tablesSchema.Rows)
            {
                SqlDatabaseTable table = new SqlDatabaseTable(t);
                if (table.IsSystemTable)
                {
                    continue;
                }
                if (!tablesKeyColumns.ContainsKey(table.TableName))
                {
                    throw new UserThrownException(string.Format("Could not find key columns for table {0}.", table.TableName), LoggingLevel.Minimum);
                }
                DatabaseTableKeyColumns tableKeys = tablesKeyColumns[table.TableName];
                if (table.IsSystemTable)
                {
                    continue;
                }
                if (result.Tables.Exists(table.TableName))
                {
                    throw new Exception(string.Format(
                        "{0} with name {1} already added.",
                        typeof(SqlDatabaseTable).FullName,
                        table.TableName));
                }
                result.AddTable(table);
                DataTable columnsSchema = GetSqlSchema<DataTable>(DatabaseSchemaInfoType.Columns, table.TableName, out rawOutput);
                table.PopulateColumnsFromSchema(columnsSchema);
                foreach (string keyColumn in tableKeys.KeyNames)
                {
                    if (!table.Columns.Exists(keyColumn))
                    {
                        throw new UserThrownException(string.Format("Could not find key column {0} on table {1}.", keyColumn, table.TableName), LoggingLevel.Minimum);
                    }
                    table.Columns[keyColumn].IsKey = true;
                }
            }
            if (createOrmAssembly)
            {
                result.CreateOrmAssembly(saveOrmAssembly);
            }
            return result;
        }

        public T Query<T>(
            FiglutWebServiceFilterString filterString,
            out string rawOutput)
        {
            return _webServiceClient.CallService<T>(
                filterString.ToString(),
                null,
                HttpVerb.GET,
                out rawOutput,
                true,
                _timeout);
        }

        public object Query(
            Type returnType,
            FiglutWebServiceFilterString filterString,
            out string rawOutput)
        {
            return _webServiceClient.CallService(
                returnType,
                filterString.ToString(),
                null,
                HttpVerb.GET,
                out rawOutput,
                true,
                _timeout);
        }

        public string Insert(
            FiglutWebServiceFilterString filterString,
            object requestPostObject)
        {
            string rawOutput = null;
            _webServiceClient.CallService<string>(
                filterString.ToString(),
                requestPostObject,
                HttpVerb.POST,
                out rawOutput,
                false,
                _timeout);
            return rawOutput;
        }

        public string Update(
            FiglutWebServiceFilterString filterString,
            string columnName,
            object requestPostObject)
        {
            string rawOutput = null;
            string queryString = 
                string.IsNullOrEmpty(columnName) ?
                filterString.ToString() :
                string.Format("{0}/{1}", filterString, columnName);
            _webServiceClient.CallService<string>(
                queryString,
                requestPostObject,
                HttpVerb.PUT,
                out rawOutput,
                false,
                _timeout);
            return rawOutput;
        }

        public string Delete(
            FiglutWebServiceFilterString filterString,
            string columnName,
            object requestPostObject)
        {
            string rawOutput = null;
            string queryString =
                string.IsNullOrEmpty(columnName) ?
                filterString.ToString() :
                string.Format("{0}/{1}", filterString, columnName);
            _webServiceClient.CallService<string>(
                queryString,
                requestPostObject,
                HttpVerb.DELETE,
                out rawOutput,
                false,
                _timeout);
            return rawOutput;
        }

        #endregion //Methods
    }
}