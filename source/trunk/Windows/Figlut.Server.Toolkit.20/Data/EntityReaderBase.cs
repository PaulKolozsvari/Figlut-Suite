namespace Figlut.Server.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Reflection;

    #endregion //Using Directives

    public class EntityReader
    {
        #region Methods

        public static DataTable GetDataTable(bool shapeColumnNames, Type entityType)
        {
            string tableName = shapeColumnNames ? DataShaper.ShapeCamelCaseString(entityType.Name) : entityType.Name;
            DataTable result = new DataTable(tableName);
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
                result.Columns.Add(new DataColumn(columnName)
                {
                    Caption = p.Name,
                    DataType = propertyType
                });
            }
            return result;
        }

        public static List<string> GetAllPropertyNames(bool shapeColumnNames, Type entityType)
        {
            List<string> result = new List<string>();
            foreach (PropertyInfo p in entityType.GetProperties())
            {
                string columnName = shapeColumnNames ? DataShaper.ShapeCamelCaseString(p.Name) : p.Name;
                result.Add(columnName);
            }
            return result;
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
            PropertyInfo p = entity.GetType().GetProperty(propertyName);
            p.SetValue(entity, value, null);
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
                if (p.PropertyType == typeof(Guid))
                {
                    value = new Guid(row[c.ColumnName].ToString());
                }
                else if (p.PropertyType == typeof(Boolean))
                {
                    value = Convert.ToBoolean(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Byte))
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
                else if (p.PropertyType == typeof(Int16))
                {
                    value = Convert.ToInt16(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Int32))
                {
                    value = Convert.ToInt32(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Int64))
                {
                    value = Convert.ToInt64(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(UInt16))
                {
                    value = Convert.ToUInt16(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(UInt32))
                {
                    value = Convert.ToUInt32(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(UInt64))
                {
                    value = Convert.ToUInt64(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Single))
                {
                    value = Convert.ToSingle(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Double))
                {
                    value = Convert.ToDouble(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(Decimal))
                {
                    value = Convert.ToDecimal(row[c.ColumnName]);
                }
                else if (p.PropertyType == typeof(DateTime))
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
            Type typeK = result.GetType();
            Type typeE = entity.GetType();
            foreach (PropertyInfo pK in typeK.GetProperties())
            {
                PropertyInfo pE = typeE.GetProperty(pK.Name);
                object valueE = pE.GetValue(entity, null);
                if (pK.PropertyType.IsEnum)
                {
                    int valueEInt = (int)valueE;
                    object kEnumValue = Enum.ToObject(pK.PropertyType, valueEInt);
                    pK.SetValue(result, kEnumValue, null);
                }
                else
                {
                    pK.SetValue(result, valueE, null);
                }
            }
            return result;
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

        #endregion //Methods
    }
}