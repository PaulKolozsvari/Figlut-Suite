namespace Figlut.MonoDroid.Toolkit.Data.DB.SQLServer
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Reflection;
    using Figlut.MonoDroid.Toolkit.Data.DB.SQLQuery;
    using System.Data;
    using System.Data.Common;

    #endregion //Using Directives

    [Serializable]
    public class SqliteDatabaseTable : DatabaseTable
    {
        #region Constructors

        public SqliteDatabaseTable() : base()
        {
        }

        public SqliteDatabaseTable(string tableName) 
            : base(tableName)
        {
        }

        public SqliteDatabaseTable(DataRow schemaRow)
            : base(schemaRow)
        {
        }

        #endregion //Constructors

        #region Constants

        public const string TABLE_NAME_SCHEMA_ATTRIBUTE = "table_name";
        public const string SYS_DIAGRAMS_TABLE_NAME = "sysdiagrams";

        #endregion //Constants

		#region Fields

		private SqliteDataManagerHelper _dbHelper;

		#endregion //Fields

		#region Properties

		public SqliteDataManagerHelper DbHelper
		{
			get{ return _dbHelper; }
			set{ _dbHelper = value; }
		}

		#endregion //Properties

        #region Methods

        public override void PopulateFromSchema(DataRow schemaRow)
        {
            _tableName = schemaRow[TABLE_NAME_SCHEMA_ATTRIBUTE].ToString();
            _isSystemTable = _tableName.Contains(SYS_DIAGRAMS_TABLE_NAME);
        }

        public override void PopulateColumnsFromSchema(DataTable columnsSchema)
        {
            _columns.Clear();
            List<DatabaseTableColumn> tempColumns = new List<DatabaseTableColumn>();
            foreach (DataRow row in columnsSchema.Rows)
            {
                tempColumns.Add(new SqliteDatabaseTableColumn(row));
            }
            tempColumns.OrderBy(c => c.OrdinalPosition).ToList().ForEach(c => _columns.Add(c.ColumnName, c));
        }

		public void AddColumnsByEntityType(Type entityType)
		{
			foreach (PropertyInfo p in entityType.GetProperties()) 
			{
				if (p.PropertyType == typeof(IntPtr) || 
					p.PropertyType == typeof(UIntPtr) ||
					p.PropertyType == typeof(Java.Lang.Class))
				{
					continue;
				}
				SqliteDatabaseTableColumn c = new SqliteDatabaseTableColumn ();
				c.ColumnName = p.Name;
				c.DataType = SqliteTypeConverter.Instance.GetSqlTypeNameFromDotNetType (
					p.PropertyType, 
					EntityReader.IsTypeIsNullable (p.PropertyType));
				_columns.Add (c);
			}
		}

		public string GetSqlCreateTableScript()
		{
			StringBuilder result = new StringBuilder ();
			string tableScript = string.Format ("CREATE TABLE IF NOT EXISTS {0}(", _tableName);
			result.AppendLine (tableScript);
			foreach (SqliteDatabaseTableColumn column in _columns)
			{
				string columnScript = string.Format ("{0} {1},", column.ColumnName, column.DataType);
				result.AppendLine (columnScript);
			}
			string resultString = result.ToString ();
			string lastCharacter = resultString.Substring (resultString.Length - 2, 1);
			if (lastCharacter == ",") {
				resultString = resultString.Substring (0, resultString.Length - 2);
			}
			resultString += ");";
			return resultString;
		}

        #endregion //Methods
    }
}