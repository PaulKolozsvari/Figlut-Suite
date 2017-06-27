namespace Figlut.Server.Toolkit.Utilities.Serialization
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Data.CSV;
    using Figlut.Server.Toolkit.Data;
    using System.Collections;

    #endregion //Using Directives

    public class CsvSerializer : ISerializer
    {
        #region Methods

        public List<object> GetObectListFromObject(object obj, out Type entityType)
        {
            List<object> result = null;
            if (DataHelper.GetGenericCollectionItemType(obj.GetType()) != null)
            {
                result = new List<object>();
                foreach (object e in (IList)obj)
                {
                    result.Add(e);
                }
            }
            else if (obj.GetType().IsArray)
            {
                Array array = (Array)obj;
                result = DataHelper.ConvertTypedArrayToObjectList(array);
            }
            else if (obj.GetType().Equals(typeof(List<object>)))
            {
                result = (List<object>)obj;
            }
            else
            {
                throw new ArgumentException(string.Format(
                    "{0} only supports the serialization of a List<object> or an array of objects.",
                    this.GetType().FullName));
            }
            if (result.Count < 1)
            {
                throw new ArgumentException(string.Format(
                    "The List<object> to be serialized by {0} must have at least one entity/object.",
                    this.GetType().FullName));
            }
            entityType = result[0].GetType();
            foreach (object e in result)
            {
                if (!e.GetType().Equals(entityType))
                {
                    throw new ArgumentException(string.Format(
                        "Not all entities/objects in the List<object> to be serialized by {0} are of the same type of {1}.",
                        this.GetType().FullName,
                        entityType.FullName));
                }
            }
            return result;
        }

        public void SerializeToFile(object obj, string filename)
        {
            Type entityType = null;
            List<object> entities = GetObectListFromObject(obj, out entityType);
            CsvWriter.WriteToFileFromEntities(entities, entityType, true, true, filename, null);
        }

        public void SerializeToFile(object obj, Type[] extraTypes, string filename)
        {
            SerializeToFile(obj, filename);
        }

        public string SerializeToText(object obj)
        {
            Type entityType = null;
            List<object> entities = GetObectListFromObject(obj, out entityType);
            return CsvWriter.WriteToStringFromEntities(entities, entityType, true, true, null);
        }

        public string SerializeToText(object obj, Type[] extraTypes)
        {
            return SerializeToText(obj);
        }

        public object DeserializeFromText(Type type, string text)
        {
            Type entityType = DataHelper.GetGenericCollectionItemType(type);
            if (entityType != null)
            {
                return CsvParser.ParseEntitiesFromString(text, true, entityType);
            }
            else if (type.IsArray)
            {
                return CsvParser.ParseEntitiesFromString(text, true, type.GetElementType());
            }
            else
            {
                throw new ArgumentException(string.Format(
                    "{0} only supports deserializing from a generic collection e.g. List<>.",
                    this.GetType().FullName));
            }
        }

        public object DeserializeFromText(Type type, Type[] extraTypes, string text)
        {
            return DeserializeFromText(type, text);
        }

        public object DeserializeFromFile(Type type, string filename)
        {
            Type entityType = DataHelper.GetGenericCollectionItemType(type);
            if (entityType == null)
            {
                throw new ArgumentException(string.Format(
                    "{0} only supports deserializing from a generic collection e.g. List<>.",
                    this.GetType().FullName));
            }
            return CsvParser.ParseEntitiesFromFile(filename, true, entityType);
        }

        public object DeserializeFromFile(Type type, Type[] extraTypes, string filename)
        {
            return DeserializeFromFile(type, filename);
        }

        #endregion //Methods
    }
}