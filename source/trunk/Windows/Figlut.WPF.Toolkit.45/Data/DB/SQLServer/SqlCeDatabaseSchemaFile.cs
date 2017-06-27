namespace Figlut.Server.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Utilities;

    #endregion //Using Directives

    public class SqlDatabaseSchemaFile
    {
        #region Methods

        public static void ExportSchema(SqlDatabase database, string filePath)
        {
            GOC.Instance.GetSerializer(SerializerType.XML).SerializeToFile(
                database,
                new Type[] 
                {
                    typeof(SqlDatabaseTable),
                    typeof(SqlDatabaseTableColumn),
                    typeof(DatabaseCache), 
                    typeof(Database), 
                    typeof(DatabaseTable), 
                    typeof(DatabaseTableColumn)
                },
                filePath);
        }

        public static SqlDatabase ImportSchema(
            string filePath, 
            bool createOrmAssembly, 
            bool saveOrmAssembly, 
            string ormAssemblyOutputDirectory)
        {
            SqlDatabase result = (SqlDatabase)GOC.Instance.GetSerializer(SerializerType.XML).DeserializeFromFile(
                typeof(SqlDatabase),
                new Type[] 
                {
                    typeof(SqlDatabaseTable), 
                    typeof(SqlDatabaseTableColumn),
                    typeof(DatabaseCache), 
                    typeof(Database), 
                    typeof(DatabaseTable), 
                    typeof(DatabaseTableColumn)
                },
                filePath);
            if (createOrmAssembly)
            {
                result.CreateOrmAssembly(saveOrmAssembly, ormAssemblyOutputDirectory);
            }
            return result;
        }

        #endregion //Methods
    }
}