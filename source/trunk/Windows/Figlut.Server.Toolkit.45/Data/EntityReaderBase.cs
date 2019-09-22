namespace Figlut.Server.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Data;
    using System.Reflection;
    using System.Data.Linq.Mapping;

    #endregion //Using Directives

    public class EntityReader
    {
        #region Methods

        public static bool IsTypeIsNullable(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static DataTable GetDataTable(bool shapeColumnNames, Type entityType)
        {
            return GetDataTable(shapeColumnNames, entityType, null);
        }

        public static DataTable GetDataTable(bool shapeColumnNames, Type entityType, List<string> topColumnNames)
        {
            string tableName = shapeColumnNames ? DataShaper.ShapeCamelCaseString(entityType.Name) : entityType.Name;
            DataTable result = new DataTable(tableName);
            Dictionary<string, DataColumn> columns = new Dictionary<string, DataColumn>();
            foreach (PropertyInfo p in entityType.GetProperties())
            {
                Type propertyType = p.PropertyType;
                if (p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = p.PropertyType.GetGenericArguments()[0];
                }
                if (propertyType == typeof(Boolean))
                {
                    propertyType = typeof(String);
                }
                string columnName = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                columns.Add(columnName, new DataColumn(columnName)
                {
                    Caption = p.Name,
                    DataType = propertyType
                });
            }
            if (topColumnNames != null)
            {
                foreach (string c in topColumnNames) //Add the top columns to the DataTable.
                {
                    if (!columns.ContainsKey(c))
                    {
                        continue;
                    }
                    result.Columns.Add(columns[c]);
                    columns.Remove(c);
                }
            }
            columns.Keys.ToList().ForEach(c => result.Columns.Add(columns[c])); //Add the remaining columns that were not mentioned in the list of top columns.
            columns.Clear();
            return result;
        }

        public static List<string> GetAllPropertyNames(bool shapeColumnNames, Type entityType)
        {
            return GetAllPropertyNames(shapeColumnNames, entityType, null, null);
        }

        public static List<string> GetAllPropertyNames(
            bool shapeColumnNames, 
            Type entityType, 
            List<string> hiddenProperties, 
            List<string> unmanagedProperies)
        {
            List<string> result = new List<string>();
            foreach (PropertyInfo p in entityType.GetProperties())
            {
                string columnName = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                if ((hiddenProperties != null && hiddenProperties.Contains(columnName)) ||
                    (unmanagedProperies != null && unmanagedProperies.Contains(columnName)))
                {
                    continue; //This column name should not be added to the result list.
                }
                result.Add(columnName);
            }
            return result;
        }

        public static bool PropertyExists(string propertyName, object entity)
        {
            return PropertyExists(propertyName, entity.GetType());
        }

        public static bool PropertyExists(string propertyName, Type entityType)
        {
            foreach (PropertyInfo p in entityType.GetProperties())
            {
                if (p.Name == propertyName)
                {
                    return true;
                }
            }
            return false;
        }

        public static object GetPropertyValue(string propertyName, object entity, bool useDBNull)
        {
            PropertyInfo p = entity.GetType().GetProperty(propertyName);
            object result = p.GetValue(entity, null);
            if (useDBNull && (result == null))
            {
                result = DBNull.Value;
            }
            return result;
        }

        public static void SetPropertyValue(string propertyName, object entity, object value)
        {
            SetPropertyValue(propertyName, entity, value, false);
        }

        public static void SetPropertyValue(string propertyName, object entity, object value, bool typeCastValue)
        {
            PropertyInfo p = entity.GetType().GetProperty(propertyName);
            if (typeCastValue)
            {
                p.SetValue(entity, ConvertValueTypeTo(value, p.PropertyType));
            }
            else
            {
                p.SetValue(entity, value);
            }
        }

        public static T ConvertValueTypeTo<T>(object value)
        {
            if (value == null || value.GetType() == typeof(System.DBNull))
            {
                return default(T);
            }
            return (T)ConvertValueTypeTo(value, typeof(T));
        }

        public static object ConvertValueTypeTo(object value, Type typeToConvertTo)
        {
            if (value == null || value.GetType() == typeof(DBNull))
            {
                return Activator.CreateInstance(typeToConvertTo);
            }
            if (typeToConvertTo.Equals(value.GetType()))
            {
                return value;
            }
            else if (typeToConvertTo.Equals(typeof(Guid)) || typeToConvertTo.Equals(typeof(Nullable<Guid>)))
            {
                return new Guid(value.ToString());
            }
            else if (typeToConvertTo.Equals(typeof(String)))
            {
                return Convert.ToString(value);
            }
            else if (typeToConvertTo.Equals(typeof(Int16)) || typeToConvertTo.Equals(typeof(Nullable<Int16>)))
            {
                return Convert.ToInt16(value);
            }
            else if (typeToConvertTo.Equals(typeof(Int32)) || typeToConvertTo.Equals(typeof(Nullable<Int32>)))
            {
                return Convert.ToInt32(value);
            }
            else if (typeToConvertTo.Equals(typeof(Int64)) || typeToConvertTo.Equals(typeof(Nullable<Int64>)))
            {
                return Convert.ToInt64(value);
            }
            else if (typeToConvertTo.Equals(typeof(UInt16)) || typeToConvertTo.Equals(typeof(Nullable<UInt16>)))
            {
                return Convert.ToUInt16(value);
            }
            else if (typeToConvertTo.Equals(typeof(UInt32)) || typeToConvertTo.Equals(typeof(Nullable<UInt32>)))
            {
                return Convert.ToUInt32(value);
            }
            else if (typeToConvertTo.Equals(typeof(UInt64)) || typeToConvertTo.Equals(typeof(Nullable<UInt64>)))
            {
                return Convert.ToUInt64(value);
            }
            else if (typeToConvertTo.Equals(typeof(Single)) || typeToConvertTo.Equals(typeof(Nullable<Single>)))
            {
                return Convert.ToSingle(value);
            }
            else if (typeToConvertTo.Equals(typeof(Double)) || typeToConvertTo.Equals(typeof(Nullable<Double>)))
            {
                return Convert.ToDouble(value);
            }
            else if (typeToConvertTo.Equals(typeof(Decimal)) || typeToConvertTo.Equals(typeof(Nullable<Decimal>)))
            {
                return Convert.ToDecimal(value);
            }
            else if (typeToConvertTo.Equals(typeof(DateTime)) || typeToConvertTo.Equals(typeof(Nullable<DateTime>)))
            {
                return Convert.ToDateTime(value);
            }
            else if (typeToConvertTo.Equals(typeof(Boolean)) || typeToConvertTo.Equals(typeof(Nullable<Boolean>)))
            {
                return Convert.ToBoolean(value);
            }
            else
            {
                throw new ArgumentException(string.Format("Unexpected vaue type {0} to convert to.", typeToConvertTo.FullName));
            }
        }

        public static object PopulateFromDataRow(object entity, DataRow row)
        {
            Type entityType = entity.GetType();
            foreach (DataColumn c in row.Table.Columns)
            {
                PropertyInfo p = entityType.GetProperty(c.Caption);
                if (p == null)
                {
                    continue;
                }
                object value = null;
                if ((row[c.ColumnName] == null) || (row[c.ColumnName].ToString() == string.Empty))
                {
                    //do nothing, leave value as null.
                }
                else if (p.PropertyType == typeof(Guid))
                {
                    value = new Guid(row[c.ColumnName].ToString());
                }
                else if (p.PropertyType == typeof(Nullable<Guid>))
                {
                    value = new Guid(row[c.ColumnName].ToString());
                }
                else if (p.PropertyType == typeof(Boolean))
                {
                    value = Convert.ToBoolean(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<Boolean>))
                {
                    value = Convert.ToBoolean(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Byte))
                {
                    value = Convert.ToByte(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<Byte>))
                {
                    value = Convert.ToByte(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Byte[]))
                {
                    value = Convert.FromBase64String(row[c.ColumnName].ToString());
                }
                else if (p.PropertyType == typeof(Char))
                {
                    value = Convert.ToChar(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<Char>))
                {
                    value = Convert.ToChar(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Int16))
                {
                    value = Convert.ToInt16(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<Int16>))
                {
                    value = Convert.ToInt16(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Int32))
                {
                    value = Convert.ToInt32(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<Int32>))
                {
                    value = Convert.ToInt32(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Int64))
                {
                    value = Convert.ToInt64(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<Int64>))
                {
                    value = Convert.ToInt64(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(UInt16))
                {
                    value = Convert.ToUInt16(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<UInt16>))
                {
                    value = Convert.ToUInt16(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(UInt32))
                {
                    value = Convert.ToUInt32(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<UInt32>))
                {
                    value = Convert.ToUInt32(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(UInt64))
                {
                    value = Convert.ToUInt64(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<UInt64>))
                {
                    value = Convert.ToUInt64(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Single))
                {
                    value = Convert.ToSingle(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<Single>))
                {
                    value = Convert.ToSingle(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Double))
                {
                    value = Convert.ToDouble(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<Double>))
                {
                    value = Convert.ToDouble(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Decimal))
                {
                    value = Convert.ToDecimal(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<Decimal>))
                {
                    value = Convert.ToDecimal(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(DateTime))
                {
                    value = Convert.ToDateTime(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Nullable<DateTime>))
                {
                    value = Convert.ToDateTime(row[c.ColumnName]);
                }
                else if ((p.PropertyType == typeof(Enum)) || (p.PropertyType.BaseType == typeof(Enum)))
                {
                    value = Enum.Parse(p.PropertyType, row[c.ColumnName].ToString(), true);
                }
                else
                {
                    value = row[c.ColumnName];
                }
                p.SetValue(entity, value, null);
            }
            return entity;
        }

        public static DataRow PopulateDataRow(object entity, DataRow row, bool shapePropertyNames, Type entityType)
        {
            foreach (PropertyInfo p in entityType.GetProperties())
            {
                object propertyValue = GetPropertyValue(p.Name, entity, true);
                string propertyName = shapePropertyNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                row[propertyName] = propertyValue;
            }
            return row;
        }

        public static object ConvertTo(object entity, Type typeToConvertTo)
        {
            object result = Activator.CreateInstance(typeToConvertTo);
            Type typeFrom = result.GetType();
            Type typeTo = entity.GetType();
            foreach (PropertyInfo pFrom in typeFrom.GetProperties())
            {
                PropertyInfo pTo = typeTo.GetProperty(pFrom.Name);
                if (pTo == null)
                {
                    continue;
                }
                object valueE = pTo.GetValue(entity, null);
                if (pFrom.PropertyType.IsEnum)
                {
                    int valueEInt = (int)valueE;
                    object kEnumValue = Enum.ToObject(pFrom.PropertyType, valueEInt);
                    pFrom.SetValue(result, kEnumValue);
                }
                else
                {
                    pFrom.SetValue(result, valueE);
                }
            }
            return result;
        }

        public static void CopyProperties(object sourceObject, object destinationObject, bool swallowExceptionsOnMismatch)
        {
            CopyProperties(sourceObject, destinationObject, swallowExceptionsOnMismatch, false);
        }

        public static void CopyProperties(object sourceObject, object destinationObject, bool swallowExceptionsOnMismatch, bool ignoreClassProperties)
        {
            Type typeSource = sourceObject.GetType();
            Type typeDestination = destinationObject.GetType();
            Dictionary<string, PropertyInfo> propertiesDestination = new Dictionary<string, PropertyInfo>();
            typeDestination.GetProperties().ToList().ForEach(p => propertiesDestination.Add(p.Name, p));
            foreach (PropertyInfo pSource in typeSource.GetProperties())
            {
                try
                {
                    if (!propertiesDestination.ContainsKey(pSource.Name) && swallowExceptionsOnMismatch)
                    {
                        continue; //To prevent exceptions as they are expensive.
                    }
                    if ((pSource.PropertyType.IsClass && !pSource.PropertyType.Equals(typeof(string))) && ignoreClassProperties)
                    {
                        continue;
                    }
                    PropertyInfo pDestination = propertiesDestination[pSource.Name];
                    object valueSource = pSource.GetValue(sourceObject);
                    if (pSource.PropertyType.IsEnum)
                    {
                        int valueSourceInt = (int)valueSource;
                        object valueSourceEnum = Enum.ToObject(pDestination.PropertyType, valueSourceInt);
                        pDestination.SetValue(destinationObject, valueSourceEnum);
                    }
                    else
                    {
                        pDestination.SetValue(destinationObject, valueSource);
                    }
                }
                catch (Exception ex)
                {
                    if (!swallowExceptionsOnMismatch)
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a the names of properties of the specified entity type whose types match are in the specified list of propertyTypes.
        /// </summary>
        /// <param name="entityType">The type of the property to reflect against.</param>
        /// <param name="propertyTypes">The types of which the properties of the entity should be.</param>
        /// <returns></returns>
        public static List<string> GetPropertyNamesByType(Type entityType, List<Type> propertyTypes, bool shapeColumnNames)
        {
            List<string> result = new List<string>();
            Dictionary<string, Type> propertyTypesLookup = new Dictionary<string, Type>();
            propertyTypes.ForEach(p => propertyTypesLookup.Add(p.FullName, p));
            foreach (PropertyInfo p in entityType.GetProperties())
            {
                if (propertyTypesLookup.ContainsKey(p.PropertyType.FullName)) //Name of property should be included.
                {
                    string columnName = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                    result.Add(columnName);
                }
            }
            return result;
        }

        /// <summary>
        /// Determines the primary key of an entity type. The first primary key found on the entity type i.e.
        /// the assumption is made that the entity type only has one primary key, which is the surrogate key.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table whose surrogate key needs to be determined.</typeparam>
        /// <returns>Retruns the PropertyInfo corresponding to the column which is the surrogate key for the specified entity type.</returns>
        public static PropertyInfo GetLinqToSqlEntitySurrogateKey<E>()
        {
            return GetLinqToSqlEntitySurrogateKey(typeof(E));
        }

        /// <summary>
        /// Determines the primary key of an entity type. The first primary key found on the entity type i.e.
        /// the assumption is made that the entity type only has one primary key, which is the surrogate key.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table whose surrogate key needs to be determined.</typeparam>
        /// <returns>Retruns the PropertyInfo corresponding to the column which is the surrogate key for the specified entity type.</returns>
        public static PropertyInfo GetLinqToSqlEntitySurrogateKey(Type entityType)
        {
            PropertyInfo[] properties = entityType.GetProperties();
            PropertyInfo surrogateKey = null;
            foreach (PropertyInfo p in properties)
            {
                object[] attributes = p.GetCustomAttributes(typeof(ColumnAttribute), false);
                ColumnAttribute columnAttribute = attributes.Length < 1 ? null : (ColumnAttribute)attributes[0];
                if ((columnAttribute == null) || (!columnAttribute.IsPrimaryKey))
                {
                    continue;
                }
                if (surrogateKey != null)
                {
                    throw new Exception(
                        string.Format("{0} has more than one primary key. A surrogate key has to be a single field.",
                        entityType.Name));
                }
                surrogateKey = p;
            }
            if (surrogateKey == null)
            {
                throw new NullReferenceException(string.Format("{0} does not have surrogate key.", entityType.Name));
            }
            return surrogateKey;
        }

        /// <summary>
        /// Determines whether a property is an identity column.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Returns true if the property is an identity column.</returns>
        public static bool IsLinqToSqlEntityPropertyIdentityColumn(PropertyInfo p)
        {
            object[] attributes = p.GetCustomAttributes(typeof(ColumnAttribute), false);
            ColumnAttribute columnAttribute = attributes.Length < 1 ? null : (ColumnAttribute)attributes[0];
            if (columnAttribute == null)
            {
                return false;
            }
            return columnAttribute.DbType.Contains("IDENTITY");
        }

        #endregion //Methods
    }
}