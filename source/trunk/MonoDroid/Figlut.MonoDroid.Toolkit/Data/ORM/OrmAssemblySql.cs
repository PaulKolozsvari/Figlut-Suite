namespace Figlut.MonoDroid.Toolkit.Data.ORM
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Reflection.Emit;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.MonoDroid.Toolkit.Data.DB.SQLServer;

    #endregion //Using Directives

    public class OrmAssemblySql : OrmAssembly
    {
        #region Constructors

        public OrmAssemblySql(string assemblyName, AssemblyBuilderAccess assemblyBuilderAccess) :
            base(assemblyName, assemblyBuilderAccess)
        {
        }

        public OrmAssemblySql(string assemblyName, string assemblyFileName, AssemblyBuilderAccess assemblyBuilderAccess) :
            base(assemblyName, assemblyFileName, assemblyBuilderAccess)
        {
        }

        #endregion //Constructors

        public const string COLUMN_NAME_SCHEMA_ATTRIBUTE = "ColumnName";
        public const string ORDINAL_POSITION_SCHEMA_ATTRIBUTE = "ColumnOrdinal";
        public const string IS_NULLABLE_SCHEMA_ATTRIBUTE = "AllowDBNull";
        public const string DATA_TYPE_NAME_SCHEMA_ATTRIBUTE = "DataTypeName";

        #region Methods

        public OrmType CreateOrmTypeFromSqlDataReader(
            string typeName,
            System.Data.SqlClient.SqlDataReader reader,
            bool prefixWithAssemblyNamespace)
        {
            DataTable schemaTable = reader.GetSchemaTable();
            OrmType result = CreateOrmType(typeName, prefixWithAssemblyNamespace);
            foreach (DataRow r in schemaTable.Rows)
            {
                string columnName = r[COLUMN_NAME_SCHEMA_ATTRIBUTE].ToString();
                short ordinalPosition = Convert.ToInt16(r[ORDINAL_POSITION_SCHEMA_ATTRIBUTE]);
                bool isNullable = bool.Parse(r[IS_NULLABLE_SCHEMA_ATTRIBUTE].ToString());
                string dataTypeName = r[DATA_TYPE_NAME_SCHEMA_ATTRIBUTE].ToString();
                result.CreateOrmProperty(
                    columnName,
                    SqlTypeConverter.Instance.GetDotNetType(dataTypeName, isNullable));
            }
            result.CreateType();
            return result;
        }

        #region Destructors

        //~OrmAssemblySql()
        //{
        //    int test = 0;
        //}

        #endregion //Destructors

        #endregion //Methods
    }
}
