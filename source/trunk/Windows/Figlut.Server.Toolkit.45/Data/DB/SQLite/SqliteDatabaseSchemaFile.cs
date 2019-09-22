namespace Figlut.Server.Toolkit.Data.DB.SQLite
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

    public class SqliteDatabaseSchemaFile
    {
        #region Methods

        public static void ExportSchema(SqliteDatabase database, string filePath)
        {
			GOC.Instance.GetSerializer(SerializerType.JSON).SerializeToFile(
                database,
                new Type[] 
                {
                    typeof(SqliteDatabaseTable),
                    typeof(SqliteDatabaseTableColumn),
                    typeof(DatabaseCache),
                    typeof(Database),
                    typeof(DatabaseTable),
                    typeof(DatabaseTableColumn),
					typeof(DatabaseTableForeignKeyColumns),
					typeof(DatabaseTableKeyColumns),
					typeof(ForeignKeyInfo),
					typeof(Dbms),
					typeof(SqliteTypeConverter),
					typeof(SqliteTypeConversionInfo)
                },
                filePath);
        }

        public static SqliteDatabase ImportSchema(
            string filePath, 
            bool createOrmAssembly, 
            bool saveOrmAssembly, 
            string ormAssemblyOutputDirectory)
        {
            SqliteDatabase result = (SqliteDatabase)GOC.Instance.GetSerializer(SerializerType.JSON).DeserializeFromFile(
                typeof(SqliteDatabase),
                new Type[] 
                {
                    typeof(SqliteDatabaseTable), 
                    typeof(SqliteDatabaseTableColumn),
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