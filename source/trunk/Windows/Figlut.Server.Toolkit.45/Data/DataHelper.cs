namespace Figlut.Server.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Data;
    using System.Reflection;
    using System.Data.SqlClient;
    using System.Collections;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Data.CSV;
    using System.Diagnostics;
    using System.Data.Common;

    #endregion //Using Directives

    public class DataHelper
    {
        #region Methods

        public static int GetColumnIndex(DataTable table, string columnName)
        {
            for (int i = 0; i < table.Columns.Count; i++)
            {
                if (table.Columns[i].ColumnName == columnName)
                {
                    return i;
                }
            }
            throw new NullReferenceException(string.Format(
                "Table {0} does not contain column with name {1}.",
                table.TableName,
                columnName));
        }

        public static object ChangeType(object value, Type conversionType)
        {
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType supplied to ChangeType may not be null.");
            }
            if (value == DBNull.Value)
            {
                return null;
            }
            if (conversionType.IsGenericType &&
                conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }
                conversionType = Nullable.GetUnderlyingType(conversionType);
            }
            return Convert.ChangeType(value, conversionType, null);
        }

        public static List<object> ParseReaderToEntities(DbDataReader reader, Type entityType, string propertyNameFilter)
        {
            propertyNameFilter = propertyNameFilter ?? string.Empty;
            List<object> result = new List<object>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    object e = Activator.CreateInstance(entityType);
                    foreach (PropertyInfo p in entityType.GetProperties())
                    {
                        if (!p.Name.Contains(propertyNameFilter) ||
                            (p.PropertyType != typeof(string) &&
                            p.PropertyType != typeof(byte) &&
                            p.PropertyType != typeof(byte[])) &&
                            (p.PropertyType.IsClass ||
                            p.PropertyType.IsEnum ||
                            p.PropertyType.IsInterface ||
                            p.PropertyType.IsNotPublic ||
                            p.PropertyType.IsPointer))
                        {
                            continue;
                        }
                        object value = null;
                        try
                        {
                            value = reader[p.Name];
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(string.Format(
                                "Could not find column {0} on {1}.",
                                p.Name,
                                reader.GetType().FullName),
                                ex);
                        }
                        p.SetValue(e, ChangeType(value, p.PropertyType), null);
                    }
                    result.Add(e);
                }
            }
            return result;
        }

        public static List<E> ParseReaderToEntities<E>(DbDataReader reader, string propertyNameFilter) where E : class
        {
            List<object> objects = ParseReaderToEntities(reader, typeof(E), propertyNameFilter);
            List<E> result = new List<E>();
            objects.ForEach(o => result.Add((E)o));
            return result;
        }

        public static E ParseDataRowToEntity<E>(DataRow row) where E : class
        {
            E result = Activator.CreateInstance<E>();
            foreach (PropertyInfo p in typeof(E).GetProperties())
            {
                if ((p.PropertyType != typeof(string) &&
                    p.PropertyType != typeof(byte) &&
                    p.PropertyType != typeof(byte[])) &&
                    (p.PropertyType.IsClass ||
                    p.PropertyType.IsEnum ||
                    p.PropertyType.IsInterface ||
                    p.PropertyType.IsNotPublic ||
                    p.PropertyType.IsPointer))
                {
                    continue;
                }
                object value = null;
                try
                {
                    value = row[p.Name];
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format(
                        "Could not find column {0} on {1}.",
                        p.Name,
                        row.GetType().FullName),
                        ex);
                }
                p.SetValue(result, ChangeType(value, p.PropertyType), null);
            }
            return result;
        }

        public static Type GetNullableType(Type type)
        {
            //Remove the Nullable<T> wrapper if the type is already nullable. If the type is not nullable the underlyingType result will be null.
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType == null && type.IsValueType)
            {
                return typeof(Nullable<>).MakeGenericType(type);
            }
            return type;
        }

        public static IList GetListOfType(Type entityType)
        {
            Type listType = typeof(List<>).MakeGenericType(entityType);
            return (IList)Activator.CreateInstance(listType);
        }

        public static IList GetListOfTypeFromObjectList(List<object> objects, Type entityType)
        {
            IList result = GetListOfType(entityType);
            objects.ForEach(o => result.Add(o));
            return result;
        }

        public static IList ConvertObjectsToTypedList(Type entityType, List<object> objects)
        {
            Type listType = typeof(List<>).MakeGenericType(entityType);
            IList result = (IList)Activator.CreateInstance(listType);
            foreach (object obj in objects)
            {
                result.Add(obj);
            }
            return result;
        }

        public static IList ConvertObjectsToTypedList(Type entityType, object[] objects)
        {
            Type listType = typeof(List<>).MakeGenericType(entityType);
            IList result = (IList)Activator.CreateInstance(listType);
            foreach (object obj in objects)
            {
                if (!obj.GetType().Equals(entityType))
                {
                    throw new ArgumentException(string.Format(
                        "Not all provided objects are of type {0}.",
                        entityType.FullName));
                }
                result.Add(obj);
            }
            return result;
        }

        public static object ConvertObjectListToTypedList(Type entityType, List<object> objects, bool performConversion = false)
        {
            var containedType = entityType.GenericTypeArguments.First();
            var enumerableType = typeof(System.Linq.Enumerable);
            var castMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.Cast)).MakeGenericMethod(containedType);
            var toListMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.ToList)).MakeGenericMethod(containedType);
            IEnumerable<object> itemsToCast;
            if (performConversion)
            {
                itemsToCast = objects.Select(item => Convert.ChangeType(item, containedType));
            }
            else
            {
                itemsToCast = objects;
            }
            var castedItems = castMethod.Invoke(null, new[] { itemsToCast });
            return toListMethod.Invoke(null, new[] { castedItems });
        }



        public static Array GetArrayOfType(Type entityType, int length)
        {
            return Array.CreateInstance(entityType, length);
        }

        public static Array ConvertObjectsToTypedArray(Type entityType, List<object> objects)
        {
            Array result = Array.CreateInstance(entityType, objects.Count);
            for (int i = 0; i < objects.Count; i++)
            {
                if (!objects[i].GetType().Equals(entityType))
                {
                    throw new ArgumentException(string.Format(
                        "Not all provided objects are of type {0}.",
                        entityType.FullName));
                }
                result.SetValue(objects[i], i);
            }
            return result;
        }

        public static Array ConvertObjectsToTypedArray(Type entityType, object[] objects)
        {
            Array result = Array.CreateInstance(entityType, objects.Length);
            for (int i = 0; i < objects.Length; i++)
            {
                if (!objects[i].GetType().Equals(entityType))
                {
                    throw new ArgumentException(string.Format(
                        "Not all provided objects are of type {0}.",
                        entityType.FullName));
                }
                result.SetValue(objects[i], i);
            }
            return result;
        }

        public static List<object> ConvertTypedArrayToObjectList(Array array)
        {
            List<object> result = new List<object>();
            for (int i = 0; i < array.Length; i++)
            {
                object o = array.GetValue(i);
                result.Add(o);
            }
            return result;
        }

        public static object[] ConvertTypedArrayToObjectArray(Array array)
        {
            return ConvertTypedArrayToObjectList(array).ToArray();
        }

        public static object GetDefaultValueOfType(Type type)
        {
            if (type.IsValueType)
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static List<E> GetEntitiesFromSerializedText<E>(
            string serializedText,
            ISerializer serializer) where E : class
        {
            List<object> objects = GetEntitiesFromSerializedText(serializedText, typeof(E), serializer);
            List<E> result = new List<E>();
            objects.ForEach(o => result.Add((E)o));
            return result;
        }

        public static List<object> GetEntitiesFromSerializedText(
            string serializedText, 
            Type entityType, 
            ISerializer serializer)
        {
            List<object> result = new List<object>();
            Array tempEmptyArray = Array.CreateInstance(entityType, 0);
            object deserializedObject = serializer.DeserializeFromText(tempEmptyArray.GetType(), new Type[] { entityType }, serializedText);
            Type deserializedObjectType = deserializedObject.GetType();
            if (DataHelper.GetGenericCollectionItemType(deserializedObject.GetType()) != null) //This is a generic collection
            {
                foreach (object e in (IList)deserializedObject)
                {
                    result.Add(e);
                }
            }
            else if (deserializedObjectType.IsArray)
            {
                foreach (object e in (Array)deserializedObject)
                {
                    result.Add(e);
                }
            }
            else
            {
                throw new ArgumentException(string.Format(
                    "{0} only supports getting entities serialized text when {1} returns generic collection (e.g. List<>) or array.",
                    typeof(DataHelper).FullName,
                    serializer.GetType().FullName));
            }
            return result;
        }

        public static DataTable GetDataTableFromEntities(List<object> entities, Type entityType)
        {
            DataTable result = EntityReader.GetDataTable(false, entityType);
            foreach (object o in entities)
            {
                DataRow row = EntityReader.PopulateDataRow(o, result.NewRow(), false, entityType);
                result.Rows.Add(row);
            }
            return result;
        }

        public static DataTable GetDataTableFromEntities<E>(List<E> entities) where E : class
        {
            List<object> objects = new List<object>();
            entities.ForEach(e => objects.Add(e));
            return GetDataTableFromEntities(objects, typeof(E));
        }

        public static List<object> GetEntitiesFromDataTable(DataTable table, Type entityType)
        {
            List<object> result = new List<object>();
            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    object e = Activator.CreateInstance(entityType);
                    EntityReader.PopulateFromDataRow(e, row);
                    result.Add(e);
                }
            }
            return result;
        }

        public static List<E> GetEntitiesFromDataTable<E>(DataTable table) where E : class
        {
            List<object> objects = GetEntitiesFromDataTable(table, typeof(E));
            List<E> result = new List<E>();
            objects.ForEach(o => result.Add((E)o));
            return result;
        }

        public static Type GetFirstParameterTypeOfGenericType(Type type)
        {
            if (!type.IsGenericType)
            {
                return type;
            }
            return type.GetGenericArguments()[0];
        }

        public static Type GetGenericCollectionItemType(Type type)
        {
            if (type.IsGenericType)
            {
                var args = type.GetGenericArguments();
                if (args.Length == 1 &&
                    typeof(ICollection<>).MakeGenericType(args).IsAssignableFrom(type))
                {
                    return args[0];
                }
            }
            return null;
        }

        public static string GetSqlDateTime(DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static void ReplaceTableNullsWithDbNulls(DataTable table)
        {
            foreach (DataColumn column in table.Columns)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row[column.ColumnName] == null)
                    {
                        row[column.ColumnName] = DBNull.Value;
                    }
                }
            }
        }

        public static List<object> ReverseListOrder(List<object> objects)
        {
            List<object> result = new List<object>();
            for (int i = objects.Count; i > -1; i--)
            {
                result.Add(objects[i]);
            }
            return result;
        }

        public static List<E> ReverseListOrder<E>(List<E> list)
        {
            List<E> result = new List<E>();
            for (int i = list.Count - 1; i > -1; i--)
            {
                result.Add(list[i]);
            }
            return result;
        }

        public static Dictionary<K,E> ReverseDictionaryOrder<K,E>(Dictionary<K,E> dictionary)
        {
            Dictionary<K, E> result = new Dictionary<K, E>();
            List<K> keys = dictionary.Keys.ToList();
            for (int i = keys.Count - 1; i > -1; i--)
            {
                K key = keys[i];
                result.Add(key, dictionary[key]);
            }
            return result;
        }

        public static DataTable FilterDataTable(DataTable inputTable, string filterText)
        {
            string filterTextLower = filterText.ToLower();
            DataTable result = inputTable.Clone();
            result.Rows.Clear();
            for (int i = 0; i < inputTable.Rows.Count; i++)
            {
                DataRow row = inputTable.Rows[i];
                bool includeRowInSearchResult = false;
                for (int j = 0; j < inputTable.Columns.Count; j++)
                {
                    object cellValue = row[j];
                    if (cellValue != null && cellValue.ToString().ToLower().Contains(filterTextLower))
                    {
                        includeRowInSearchResult = true;
                        break;
                    }
                }
                if (includeRowInSearchResult)
                {
                    result.ImportRow(row);
                }
            }
            return result;
        }

        #endregion //Methods
    }
}