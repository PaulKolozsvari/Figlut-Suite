namespace Figlut.Mobile.Toolkit.Data.DB.SQLCE
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Data.SqlTypes;
    using System.Data;

    #endregion //Using Directives

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/system.data.sqltypes.aspx
    /// </summary>
    public class SqlTypeConverter : EntityCache<string, SqlTypeConversionInfo>
    {
        #region Singleton Setup

        private static SqlTypeConverter _instance;

        public static SqlTypeConverter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SqlTypeConverter();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private SqlTypeConverter()
        {
            Add(new SqlTypeConversionInfo("bigint", typeof(SqlInt64), SqlDbType.BigInt, typeof(Int64)));
            Add(new SqlTypeConversionInfo("binary", typeof(SqlBytes), SqlDbType.VarBinary, typeof(Byte[])));
            Add(new SqlTypeConversionInfo("bit", typeof(SqlBoolean), SqlDbType.Bit, typeof(Boolean)));
            Add(new SqlTypeConversionInfo("char", typeof(SqlChars), SqlDbType.Char, typeof(char))); //this one may need work
            Add(new SqlTypeConversionInfo("cursor", null, SqlDbType.Variant, null));
            Add(new SqlTypeConversionInfo("date", typeof(SqlDateTime), SqlDbType.Date, typeof(DateTime)));
            Add(new SqlTypeConversionInfo("datetime", typeof(SqlDateTime), SqlDbType.DateTime, typeof(DateTime)));
            Add(new SqlTypeConversionInfo("datetime2", typeof(SqlDateTime), SqlDbType.DateTime2, typeof(DateTime)));
            Add(new SqlTypeConversionInfo("DATETIMEOFFSET", typeof(SqlDateTime), SqlDbType.DateTimeOffset, typeof(DateTime)));
            Add(new SqlTypeConversionInfo("decimal", typeof(SqlDecimal), SqlDbType.Decimal, typeof(Decimal)));
            Add(new SqlTypeConversionInfo("float", typeof(SqlDouble), SqlDbType.Float, typeof(Double)));
            //Add(new SqlTypeConversionInfo("geography", typeof(SqlGeography),typeof(null)));
            //Add(new SqlTypeConversionInfo("geometry", typeof(SqlGeometry),typeof(null)));
            //Add(new SqlTypeConversionInfo("hierarchyid", typeof(SqlHierarchyId),typeof(null)));
            Add(new SqlTypeConversionInfo("image", typeof(SqlBytes), SqlDbType.Image, typeof(byte[])));
            Add(new SqlTypeConversionInfo("int", typeof(SqlInt32), SqlDbType.Int, typeof(Int32)));
            Add(new SqlTypeConversionInfo("money", typeof(SqlMoney), SqlDbType.Money, typeof(Decimal)));
            //Add(new SqlTypeConversionInfo("nchar", typeof(SqlChars), SqlDbType.NChar, typeof(String)));
            Add(new SqlTypeConversionInfo("ntext", typeof(SqlChars), SqlDbType.NText, null));
            Add(new SqlTypeConversionInfo("numeric", typeof(SqlDecimal), SqlDbType.Decimal, typeof(Decimal)));
            Add(new SqlTypeConversionInfo("nvarchar", typeof(SqlChars), SqlDbType.NVarChar, typeof(String)));
            Add(new SqlTypeConversionInfo("nvarchar(1)", typeof(SqlChars), SqlDbType.NVarChar, typeof(Char)));
            Add(new SqlTypeConversionInfo("nchar(1)", typeof(SqlChars), SqlDbType.NVarChar, typeof(Char)));
            Add(new SqlTypeConversionInfo("real", typeof(SqlSingle), SqlDbType.Real, typeof(Single)));
            Add(new SqlTypeConversionInfo("rowversion", null, SqlDbType.Binary, typeof(Byte[])));
            Add(new SqlTypeConversionInfo("smallint", typeof(SqlInt16), SqlDbType.SmallInt, typeof(Int16)));
            Add(new SqlTypeConversionInfo("smallmoney", typeof(SqlMoney), SqlDbType.SmallMoney, typeof(Decimal)));
            Add(new SqlTypeConversionInfo("table", null, SqlDbType.Structured, null));
            Add(new SqlTypeConversionInfo("text", typeof(SqlString), SqlDbType.VarChar, typeof(string))); //this one may need work
            Add(new SqlTypeConversionInfo("time", null, SqlDbType.Time, typeof(TimeSpan)));
            Add(new SqlTypeConversionInfo("timestamp", null, SqlDbType.Timestamp, null));
            Add(new SqlTypeConversionInfo("tinyint", typeof(SqlByte), SqlDbType.TinyInt, typeof(Byte)));
            Add(new SqlTypeConversionInfo("uniqueidentifier", typeof(SqlGuid), SqlDbType.UniqueIdentifier, typeof(Guid)));
            Add(new SqlTypeConversionInfo("varbinary", typeof(SqlBytes), SqlDbType.VarBinary, typeof(Byte[])));
            Add(new SqlTypeConversionInfo("varbinary(1)", typeof(SqlBytes), SqlDbType.VarBinary, typeof(byte)));
            Add(new SqlTypeConversionInfo("binary(1)", typeof(SqlBytes), SqlDbType.Binary, typeof(byte)));
            Add(new SqlTypeConversionInfo("varchar", typeof(SqlString), SqlDbType.VarChar, typeof(string))); //this one may need work
            Add(new SqlTypeConversionInfo("xml", typeof(SqlXml), SqlDbType.Xml, typeof(string)));
            Add(new SqlTypeConversionInfo("sql_variant", null, SqlDbType.Variant, typeof(Object)));
        }

        #endregion //Constructors

        #region Methods

        public Type GetDotNetType(string sqlTypeName, bool isNullable)
        {
            if (!Exists(sqlTypeName))
            {
                throw new ArgumentException(string.Format(
                    "Could not find {0} for SQL Type Name {1}.",
                    typeof(SqlTypeConversionInfo).FullName,
                    sqlTypeName));
            }
            Type result = this[sqlTypeName].DotNetType;
            if (result == null)
            {
                throw new NullReferenceException(string.Format(
                    "No matching .NET type for {0}.",
                    sqlTypeName));
            }
            if (isNullable)
            {
                return DataHelper.GetNullableType(result);
            }
            return result;
        }

        public Type GetDotNetType(Type sqlType, bool isNullable)
        {
            foreach (SqlTypeConversionInfo typeInfo in this)
            {
                if (typeInfo.SqlType == null)
                {
                    continue;
                }
                if (!isNullable && typeInfo.SqlType.IsAssignableFrom(sqlType))
                {
                    if (typeInfo.DotNetType == null)
                    {
                        throw new NullReferenceException(string.Format(
                            "No matching .NET type for SQL type {0}.",
                            sqlType.FullName));
                    }
                    return typeInfo.DotNetType;
                }
                else if (DataHelper.GetNullableType(typeInfo.SqlType).IsAssignableFrom(sqlType))
                {
                    if (typeInfo.DotNetType == null)
                    {
                        throw new NullReferenceException(string.Format(
                            "No matching .NET type for SQL type {0}.",
                            sqlType.FullName));
                    }
                    return typeInfo.DotNetType;
                }
            }
            throw new ArgumentException(string.Format(
                "Could not find {0} for SQL Type {1}.",
                typeof(SqlTypeConversionInfo).FullName,
                sqlType.FullName));
        }

        public Type GetSqlType(string sqlTypeName, bool isNullable)
        {
            if (!Exists(sqlTypeName))
            {
                throw new ArgumentException(string.Format(
                    "Could not find {0} for SQL Type Name {1}.",
                    typeof(SqlTypeConversionInfo).FullName,
                    sqlTypeName));
            }
            Type result = this[sqlTypeName].SqlType;
            if (result == null)
            {
                throw new NullReferenceException(string.Format(
                    "No matching SQL type for {0}.",
                    sqlTypeName));
            }
            if (isNullable)
            {
                return DataHelper.GetNullableType(result);
            }
            return result;
        }

        public Type GetSqlType(Type dotNetType, bool isNullable)
        {
            foreach (SqlTypeConversionInfo typeInfo in this)
            {
                if (typeInfo.DotNetType == null)
                {
                    continue;
                }
                if (!isNullable && typeInfo.DotNetType.IsAssignableFrom(dotNetType))
                {
                    if (typeInfo.SqlType == null)
                    {
                        throw new NullReferenceException(string.Format(
                            "No matching SQL type for .NET type {0}.",
                            dotNetType.FullName));
                    }
                    return typeInfo.SqlType;
                }
                else if (DataHelper.GetNullableType(typeInfo.DotNetType).IsAssignableFrom(dotNetType))
                {
                    if (typeInfo.SqlType == null)
                    {
                        throw new NullReferenceException(string.Format(
                            "No matching SQL type for .NET type {0}.",
                            dotNetType.FullName));
                    }
                    return typeInfo.SqlType;
                }
            }
            throw new ArgumentException(string.Format(
                "Could not find {0} for .NET Type {1}.",
                typeof(SqlTypeConversionInfo).FullName,
                dotNetType.FullName));
        }

        public string GetSqlTypeNameFromDotNetType(Type dotNetType, bool isNullable)
        {
            foreach (SqlTypeConversionInfo typeInfo in this)
            {
                if (typeInfo.DotNetType == null)
                {
                    continue;
                }
                if (!isNullable && typeInfo.DotNetType.IsAssignableFrom(dotNetType))
                {
                    return typeInfo.SqlTypeName;
                }
                else if (DataHelper.GetNullableType(typeInfo.DotNetType).IsAssignableFrom(dotNetType))
                {
                    return typeInfo.SqlTypeName;
                }
            }
            throw new ArgumentException(string.Format(
                "Could not find {0} for .NET Type {1}.",
                typeof(SqlTypeConversionInfo).FullName,
                dotNetType.FullName));
        }

        public string GetSqlTypeNameFromSqlType(Type sqlType, bool isNullable)
        {
            foreach (SqlTypeConversionInfo typeInfo in this)
            {
                if (typeInfo.SqlType == null)
                {
                    continue;
                }
                if (!isNullable && typeInfo.SqlType.IsAssignableFrom(sqlType))
                {
                    return typeInfo.SqlTypeName;
                }
                else if (DataHelper.GetNullableType(typeInfo.SqlType).IsAssignableFrom(sqlType))
                {
                    return typeInfo.SqlTypeName;
                }
            }
            throw new ArgumentException(string.Format(
                "Could not find {0} for SQLs Type {1}.",
                typeof(SqlTypeConversionInfo).FullName,
                sqlType.FullName));
        }

        public SqlDbType GetSqlDbType(string sqlTypeName)
        {
            if (!Exists(sqlTypeName))
            {
                throw new ArgumentException(string.Format(
                    "Could not find {0} for SQL Type Name {1}.",
                    typeof(SqlTypeConversionInfo).FullName,
                    sqlTypeName));
            }
            return this[sqlTypeName].SqlDbType;
        }

        public SqlDbType GetSqlDbTypeFromDotNetType(Type dotNetType)
        {
            foreach (SqlTypeConversionInfo typeInfo in this)
            {
                if (typeInfo.DotNetType == null)
                {
                    continue;
                }
                if (typeInfo.DotNetType.IsAssignableFrom(dotNetType))
                {
                    if (typeInfo.SqlType == null)
                    {
                        throw new NullReferenceException(string.Format(
                            "No matching SQL type for .NET type {0}.",
                            dotNetType.FullName));
                    }
                    return typeInfo.SqlDbType;
                }
                else if (DataHelper.GetNullableType(typeInfo.DotNetType).IsAssignableFrom(dotNetType))
                {
                    if (typeInfo.SqlType == null)
                    {
                        throw new NullReferenceException(string.Format(
                            "No matching SQL type for .NET type {0}.",
                            dotNetType.FullName));
                    }
                    return typeInfo.SqlDbType;
                }
            }
            throw new ArgumentException(string.Format(
                "Could not find {0} for .NET Type {1}.",
                typeof(SqlTypeConversionInfo).FullName,
                dotNetType.FullName));
        }

        public SqlDbType GetSqlDbTypeFromSqlType(Type sqlType)
        {
            foreach (SqlTypeConversionInfo typeInfo in this)
            {
                if (typeInfo.SqlType == null)
                {
                    continue;
                }
                if (typeInfo.SqlType.IsAssignableFrom(sqlType))
                {
                    return typeInfo.SqlDbType;
                }
                else if (DataHelper.GetNullableType(typeInfo.SqlType).IsAssignableFrom(sqlType))
                {
                    return typeInfo.SqlDbType;
                }
            }
            throw new ArgumentException(string.Format(
                "Could not find {0} for SQLs Type {1}.",
                typeof(SqlTypeConversionInfo).FullName,
                sqlType.FullName));
        }

        #endregion //Methods
    }
}
