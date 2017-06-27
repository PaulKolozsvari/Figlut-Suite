using System;

namespace Figlut.MonoDroid.Toolkit.Data.DB
{
	[Serializable]
    public enum DatabaseSchemaInfoType
    {
        DatabaseName,
        Tables,
        Columns,
        TableKeyColumns,
        TableForeignKeyColumns,
    }
}
