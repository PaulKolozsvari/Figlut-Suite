namespace Figlut.Mobile.Toolkit.WM.Data.DB.SQLCE
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Mobile.Toolkit.Data.DB;
    using Figlut.Mobile.Toolkit.Utilities;
    using Figlut.Mobile.Toolkit.Utilities.Serialization;
    using Figlut.Mobile.Toolkit.Data.DB.SQLCE;
    using Figlut.Mobile.Toolkit.WM.Data.DB.SQLCE;
    using System.IO;

    #endregion //Using Directives

    public class SqlDatabaseSchemaFile
    {
        #region Methods

        public static void ExportSchema(SqlCeDatabase database, string filePath)
        {
            GOC.Instance.GetSerializer(SerializerType.XML).SerializeToFile(
                database,
                new Type[] 
                {
                    typeof(SqlCeDatabaseTable),
                    typeof(SqlCeDatabaseTableColumn),
                    typeof(MobileDatabaseCache), 
                    typeof(MobileDatabase), 
                    typeof(MobileDatabaseTable), 
                    typeof(MobileDatabaseTableCache), 
                    typeof(MobileDatabaseTableColumn)
                },
                filePath);
        }

        public static SqlCeDatabase ImportSchema(
            string filePath, 
            bool createOrmAssembly, 
            bool saveOrmAssembly, 
            string ormAssemblyOutputDirectory)
        {
            SqlCeDatabase result = (SqlCeDatabase)GOC.Instance.GetSerializer(SerializerType.XML).DeserializeFromFile(
                typeof(SqlCeDatabase),
                new Type[] 
                {
                    typeof(SqlCeDatabaseTable), 
                    typeof(SqlCeDatabaseTableColumn),
                    typeof(MobileDatabaseCache), 
                    typeof(MobileDatabase), 
                    typeof(MobileDatabaseTable), 
                    typeof(MobileDatabaseTableCache), 
                    typeof(MobileDatabaseTableColumn)
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