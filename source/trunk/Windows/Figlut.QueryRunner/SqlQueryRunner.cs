namespace Figlut.QueryRunner
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.ORM;
    using Figlut.Server.Toolkit.Data.QueryRunners;
    using Figlut.Server.Toolkit.Utilities.Serialization;

    #endregion //Using Directives

    public class SqlQueryRunner : ISqlQueryRunner
    {
        #region Methods

        public SqlQueryRunnerOutput ExecuteQuery(SqlQueryRunnerInput input)
        {
            try
            {
                ISerializer inputSerializer = null;
                ISerializer outputSerializer = null;
                SerializerHelper.GetSerialisers(null, input.AcceptContentType, out inputSerializer, out outputSerializer);
                List<object> entities = new List<object>();
                Type dotNetType = null;
                using (SqlConnection connection = new SqlConnection(input.ConnectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(input.SqlQueryString, connection))
                    {
                        command.CommandType = System.Data.CommandType.Text;
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!string.IsNullOrEmpty(input.OrmTypeName)) //The caller is expecting a list of entities back i.e. a SQL SELECT query was sent.
                            {
                                dotNetType = OrmCodeGenerator.GenerateType(input.OrmAssemblyName, input.OrmTypeName, reader, true);
                                entities = DataHelper.ParseReaderToEntities(reader, dotNetType);
                            }
                            else //The caller is not expecting a list of entities back i.e. a SQL Insert, Update or Delete was sent.
                            {
                                return new SqlQueryRunnerOutput(true, "SQL query executed successfully.");
                            }
                        }
                    }
                }
                string resultMessage = SerializerHelper.SerializeEntities(
                    entities,
                    dotNetType,
                    outputSerializer,
                    input.IncludeOrmTypeNamesInJsonResponse);
                return new SqlQueryRunnerOutput(true, resultMessage);
            }
            catch (Exception ex)
            {
                return new SqlQueryRunnerOutput(false, ex.Message);
            }
        }

        #endregion //Methods
    }
}