namespace Figlut.MonoDroid.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Collections;
    using System.IO;
    using System.Reflection;
    using System.Data;
    using Figlut.MonoDroid.Toolkit.Data.CSV;
    using Figlut.MonoDroid.Toolkit.Utilities;
    using Figlut.MonoDroid.Toolkit.Utilities.Serialization;

    #endregion //Using Directives

    public class EntityCache : IEnumerable
    {
        #region Constructors

        public EntityCache(Type entityType)
        {
            Type type = this.GetType();
            _name = DataShaper.ShapeCamelCaseString(type.Name);
            _defaultFilePath = Path.Combine(Information.GetExecutingDirectory(), string.Format("{0}.xml", type.Name));
            _entityType = entityType;

            _entities = new List<object>();
            _addedEntities = new List<object>();
            _deletedEntities = new List<object>();
        }

        public EntityCache(string name, string defaultFilePath, Type entityType)
        {
            _name = name;
            _defaultFilePath = defaultFilePath;
            _entityType = entityType;

            _entities = new List<object>();
            _addedEntities = new List<object>();
            _deletedEntities = new List<object>();
        }

        #endregion //Constructors

        #region Fields

        protected string _name;
        protected string _defaultFilePath;
        protected List<object> _entities;
        protected List<object> _addedEntities;
        protected List<object> _deletedEntities;
        protected Type _entityType;

        #endregion //Fields

        #region Properties

        public string Name
        {
            get { return _name; }
        }

        public string DefaultFilePath
        {
            get { return _defaultFilePath; }
        }

        public List<object> AddedEntities
        {
            get { return _addedEntities; }
        }

        public List<object> DeletedEntities
        {
            get { return _deletedEntities; }
        }

        public int Count
        {
            get { return _entities.Count; }
        }

        #endregion //Properties

        #region Methods

        private void ValidateEntityType(object e)
        {
            if (e.GetType() != _entityType)
            {
                throw new ArgumentException(string.Format(
                    "Expected entity of type {0} and received entity of type.",
                    _entityType.FullName,
                    e.GetType().FullName));
            }
        }

        public virtual void OverrideFromList(IList entities)
        {
            List<object> result = new List<object>();
            foreach (object e in entities)
            {
                ValidateEntityType(e); 
                result.Add(e);
            }
            Clear();
            _entities = null;
            _entities = result;
        }

        public virtual void Add(object e)
        {
            ValidateEntityType(e);
            _entities.Add(e);
            _addedEntities.Add(e);
        }

        public virtual void Delete(object e)
        {
            if (!_entities.Contains(e))
            {
                throw new NullReferenceException(string.Format(
                    "Could not find entity {0} of type {1} in this collection to be deleted.",
                    e.ToString(),
                    e.GetType().FullName));
            }
            _entities.Remove(e);
            if (_addedEntities.Contains(e))
            {
                _addedEntities.Remove(e);
            }
            else
            {
                _deletedEntities.Add(e);
            }
        }

        public virtual bool Exists(object e)
        {
            return _entities.Contains(e);
        }

        public virtual void Clear()
        {
            _entities.Clear();
            _addedEntities.Clear();
            _deletedEntities.Clear();
            GC.Collect();
        }

        public virtual object GetEntityByProperty(string propertyName, object propertyValue, bool exactMatch)
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add(propertyName, propertyValue);
            List<object> result = GetEntitiesByProperties(properties, exactMatch);
            return result.Count < 1 ? null : result[0];
        }

        public virtual List<object> GetEntitiesByProperties(Dictionary<string, object> properties, bool exactMatch)
        {
            List<object> result = new List<object>();
            if (properties == null) //Return all the items.
            {
                _entities.ForEach(p => result.Add(p));
                return result;
            }
            List<PropertyInfo> propertyList = new List<PropertyInfo>();
            foreach (string propertyName in properties.Keys)
            {
                PropertyInfo p = _entityType.GetProperty(propertyName);
                if (p == null)
                {
                    throw new NullReferenceException(string.Format(
                        "Could not find property with name {0} on entity type {1}.",
                        propertyName,
                        _entityType.FullName));
                }
                propertyList.Add(p);
            }
            if (exactMatch)
            {
                foreach (object e in _entities)
                {
                    bool include = true;
                    foreach (PropertyInfo p in propertyList)
                    {
                        object entityProvidedValue = p.GetValue(e, null);
                        object providedPropertyValue = properties[p.Name];
                        if (entityProvidedValue == null)
                        {
                            if (providedPropertyValue == null)
                            {
                                continue;
                            }
                            else
                            {
                                include = false;
                                break;
                            }
                        }
                        if (!entityProvidedValue.Equals(providedPropertyValue))
                        {
                            include = false;
                            break;
                        }
                    }
                    if (include)
                    {
                        result.Add(e);
                    }
                }
            }
            else
            {
                foreach (object e in _entities)
                {
                    bool include = true;
                    foreach (PropertyInfo p in propertyList)
                    {
                        object entityProvidedValue = p.GetValue(e, null);
                        object providedPropertyValue = properties[p.Name];
                        if (entityProvidedValue == null)
                        {
                            if (providedPropertyValue == null)
                            {
                                continue;
                            }
                            else
                            {
                                include = false;
                                break;
                            }
                        }
                        if (p.PropertyType == typeof(string)) //String (contains) comparison only on strings.
                        {
                            string entityValueStr = entityProvidedValue == null ? string.Empty : entityProvidedValue.ToString().ToLower();
                            if (!entityValueStr.Contains(providedPropertyValue.ToString().ToLower()))
                            {
                                include = false;
                                break;
                            }
                        }
                        else //Object comparison on anything else
                        {
                            if (!entityProvidedValue.Equals(providedPropertyValue))
                            {
                                include = false;
                                break;
                            }
                        }
                    }
                    if (include)
                    {
                        result.Add(e);
                    }
                }
            }
            return result;
        }

        public virtual bool PropertyValueExists(string propertyName, object propertyValue)
        {
            PropertyInfo p = _entityType.GetProperty(propertyName);
            if (p == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find property with name {0} on entity type {1}.",
                    propertyName,
                    _entityType.FullName));
            }
            foreach (object e in _entities)
            {
                object value = p.GetValue(e, null);
                if (propertyValue.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual DataTable GetDataTable(Dictionary<string, object> properties, bool exactMatch, bool shapeColumnNames)
        {
            DataTable result = EntityReader.GetDataTable(shapeColumnNames, _entityType);
            List<object> entities = GetEntitiesByProperties(properties, exactMatch);
            foreach (object e in entities)
            {
                DataRow row = EntityReader.PopulateDataRow(e, result.NewRow(), shapeColumnNames, _entityType);
                result.Rows.Add(row);
            }
            return result;
        }

        public virtual List<object> DataTableToList(DataTable table)
        {
            List<object> result = new List<object>();
            foreach (DataRow row in table.Rows)
            {
                object e = EntityReader.PopulateFromDataRow(Activator.CreateInstance(_entityType), row);
                result.Add(e);
            }
            return result;
        }

        public virtual void ExportToCsv(
            string filePath,
            Dictionary<string, object> properties,
            bool exactMatch,
            bool shapeColumnNames)
        {
            CsvWriter.WriteToFile(
                GetDataTable(properties, exactMatch, shapeColumnNames),
                true,
                true,
                filePath,
                null);
        }

        public virtual StringWriter ExportToStringWriter(
            Dictionary<string, object> properties,
            bool exactMatch,
            bool shapeColumnNames)
        {
            StringWriter result = new StringWriter();
            CsvWriter.WriteToStream(
                result,
                GetDataTable(properties, exactMatch, shapeColumnNames),
                true,
                true,
                null);
            return result;
        }

        public virtual MemoryStream ExportToStream(
            Dictionary<string, object> properties,
            bool exactMatch,
            bool shapeColumnNames)
        {
            return new MemoryStream((new ASCIIEncoding()).GetBytes(
                ExportToStringWriter(properties, exactMatch, shapeColumnNames).GetStringBuilder().ToString()));
        }

        public virtual void ImportFromCsv(string filePath, bool shapeColumnNames)
        {
            _entities = DataTableToList(CsvParser.ParseFromFile(
                filePath, 
                true,
                EntityReader.GetDataTable(shapeColumnNames, _entityType).Columns.Count));
        }

        public virtual bool RefreshFromServer()
        {
            return true;
        }

        public virtual bool SaveToServer()
        {
            return true;
        }

        public virtual void LoadFromFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                if (string.IsNullOrEmpty(_defaultFilePath))
                {
                    throw new NullReferenceException("File path not specified for entity cache.");
                }
                filePath = _defaultFilePath;
            }
            if (!File.Exists(filePath))
            {
                SaveToFile(filePath);
            }
            EntityCache entities =
                (EntityCache)GOC.Instance.GetSerializer(SerializerType.XML).DeserializeFromFile(this.GetType(), filePath);
            Clear();
            foreach (object e in entities)
            {
                Add(e);
            }
        }

        public virtual void SaveToFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                if (string.IsNullOrEmpty(_defaultFilePath))
                {
                    throw new NullReferenceException("File path not specified for entity cache.");
                }
                filePath = _defaultFilePath;
            }
            GOC.Instance.GetSerializer(SerializerType.XML).SerializeToFile(this, filePath);
        }

        public IEnumerator GetEnumerator()
        {
            return _entities.GetEnumerator();
        }

        #endregion //Methods
    }
}
