namespace Figlut.Server.Toolkit.Data
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
    using Figlut.Server.Toolkit.Data.CSV;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Serialization;

    #endregion //Using Directives

    public class EntityCacheUnique
    {
        #region Constructors

        public EntityCacheUnique(Type entityType)
        {
            Type type = this.GetType();
            _name = DataShaper.ShapeCamelCaseString(type.Name);
            _defaultFilePath = Path.Combine(Information.GetExecutingDirectory(), string.Format("{0}.xml", type.Name));
            _entityType = entityType;

            _entities = new Dictionary<Guid,object>();
            _addedEntities = new Dictionary<Guid,object>();
            _deletedEntities = new Dictionary<Guid,object>();
            _updatedEntities = new Dictionary<Guid, object>();
        }

        public EntityCacheUnique(string name, string defaultFilePath, Type entityType)
        {
            _name = name;
            _defaultFilePath = defaultFilePath;
            _entityType = entityType;

            _entities = new Dictionary<Guid,object>();
            _addedEntities = new Dictionary<Guid,object>();
            _deletedEntities = new Dictionary<Guid,object>();
            _updatedEntities = new Dictionary<Guid, object>();
        }

        #endregion //Constructors

        #region Fields

        protected string _name;
        protected string _defaultFilePath;
        protected Dictionary<Guid, object> _entities;
        protected Dictionary<Guid, object> _addedEntities;
        protected Dictionary<Guid, object> _deletedEntities;
        protected Dictionary<Guid, object> _updatedEntities;
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

        public Dictionary<Guid, object> AddedEntities
        {
            get { return _addedEntities; }
        }

        public Dictionary<Guid, object> DeletedEntities
        {
            get { return _deletedEntities; }
        }

        public Dictionary<Guid, object> UpdatedEntities
        {
            get { return _updatedEntities; }
        }

        public int Count
        {
            get { return _entities.Count; }
        }

        #endregion //Properties

        #region Indexers

        public object this[Guid entityId]
        {
            get
            {
                if (!_entities.ContainsKey(entityId))
                {
                    return null;
                }
                return _entities[entityId];
            }
        }

        #endregion //Indexers

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
            Dictionary<Guid, object> result = new Dictionary<Guid, object>();
            foreach (object e in entities)
            {
                ValidateEntityType(e);
                Guid entityId;
                while (true)
                {
                    entityId = Guid.NewGuid();
                    if (_entities.ContainsKey(entityId))
                    {
                        continue;
                    }
                    break;
                }
                result.Add(entityId, e);
            }
            Clear();
            _entities = null;
            _entities = result;
        }

        /// <summary>
        /// Adds the specified entity to collection of updated entities to be used when saving to the server.
        /// </summary>
        /// <param name="entityId">The ID of the entity that was updated.</param>
        /// <param name="e">The entity that was update.</param>
        public virtual void NotifyEntityUpdated(Guid entityId, object e)
        {
            ValidateEntityType(e);
            if (!_addedEntities.ContainsKey(entityId) && !_updatedEntities.ContainsKey(entityId))
            {
                _updatedEntities.Add(entityId, e);
            }
        }

        public virtual void Add(object e)
        {
            ValidateEntityType(e);
            Guid entityId;
            while (true)
            {
                entityId = Guid.NewGuid();
                if (_entities.ContainsKey(entityId))
                {
                    continue;
                }
                break;
            }
            _entities.Add(entityId, e);
            _addedEntities.Add(entityId, e);
        }

        public virtual void Delete(Guid entityId)
        {
            if (!_entities.ContainsKey(entityId))
            {
                throw new NullReferenceException(string.Format(
                    "Could not find entity with ID {0} in this collection to be deleted.",
                    entityId.ToString()));
            }
            object e = _entities[entityId];
            _entities.Remove(entityId);
            if (_addedEntities.ContainsKey(entityId))
            {
                _addedEntities.Remove(entityId);
            }
            else
            {
                _deletedEntities.Add(entityId, e);
            }
        }

        public virtual bool Exists(Guid entityId)
        {
            return _entities.ContainsKey(entityId);
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
            Dictionary<Guid, object> result = GetEntitiesByProperties(properties, exactMatch);
            return result.Count < 1 ? null : result.First().Value;
        }

        public virtual Dictionary<Guid, object> GetEntitiesByProperties(Dictionary<string, object> properties, bool exactMatch)
        {
            Dictionary<Guid, object> result = new Dictionary<Guid, object>();
            if (properties == null) //Return all the items.
            {
                foreach (Guid id in _entities.Keys)
                {
                    result.Add(id, _entities[id]);
                }
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
                foreach (Guid id in _entities.Keys)
                {
                    object e = _entities[id];
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
                        result.Add(id, e);
                    }
                }
            }
            else
            {
                foreach (Guid id in _entities.Keys)
                {
                    object e = _entities[id];
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
                        result.Add(id, e);
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
            foreach (object e in _entities.Values)
            {
                object value = p.GetValue(e, null);
                if (propertyValue.Equals(value))
                {
                    return true;
                }
            }
            return false;
        }

        public virtual DataTable GetDataTable(
            Dictionary<string, object> properties, 
            bool exactMatch, 
            bool shapeColumnNames,
            string entityIdColumnName)
        {
            bool includeEntityIdColumn = !string.IsNullOrEmpty(entityIdColumnName);
            DataTable result = EntityReader.GetDataTable(shapeColumnNames, _entityType);
            if (includeEntityIdColumn)
            {
                Type entityIdType = typeof(Guid);
                result.Columns.Add(new DataColumn(entityIdColumnName)
                {
                    Caption = entityIdColumnName,
                    DataType = entityIdType
                });
            }
            Dictionary<Guid, object> entities = GetEntitiesByProperties(properties, exactMatch);
            foreach (Guid id in entities.Keys)
            {
                object e = entities[id];
                DataRow row = EntityReader.PopulateDataRow(e, result.NewRow(), shapeColumnNames, _entityType);
                if (includeEntityIdColumn)
                {
                    row[entityIdColumnName] = id;
                }
                result.Rows.Add(row);
            }
            return result;
        }

        public virtual Dictionary<Guid, object> DataTableToDictionary(DataTable table)
        {
            Dictionary<Guid, object> result = new Dictionary<Guid, object>();
            foreach (DataRow row in table.Rows)
            {
                object e = EntityReader.PopulateFromDataRow(Activator.CreateInstance(_entityType), row);
                Guid entityId;
                while (true)
                {
                    entityId = Guid.NewGuid();
                    if (result.ContainsKey(entityId))
                    {
                        continue;
                    }
                    break;
                }
                result.Add(entityId, e);
            }
            return result;
        }

        public virtual void ExportToCsv(
            string filePath,
            Dictionary<string, object> properties,
            bool exactMatch,
            bool shapeColumnNames,
            string entityIdColumnName)
        {
            CsvWriter.WriteToFile(
                GetDataTable(properties, exactMatch, shapeColumnNames, entityIdColumnName),
                true,
                true,
                filePath,
                null);
        }

        public virtual StringWriter ExportToStringWriter(
            Dictionary<string, object> properties,
            bool exactMatch,
            bool shapeColumnNames,
            string entityIdColumnName)
        {
            StringWriter result = new StringWriter();
            CsvWriter.WriteToStream(
                result,
                GetDataTable(properties, exactMatch, shapeColumnNames, entityIdColumnName),
                true,
                true,
                null);
            return result;
        }

        public virtual MemoryStream ExportToStream(
            Dictionary<string, object> properties,
            bool exactMatch,
            bool shapeColumnNames,
            string entityIdColumnName)
        {
            return new MemoryStream((new ASCIIEncoding()).GetBytes(
                ExportToStringWriter(properties, exactMatch, shapeColumnNames, entityIdColumnName).GetStringBuilder().ToString()));
        }

        public virtual void ImportFromCsv(string filePath, bool shapeColumnNames)
        {
            _entities = DataTableToDictionary(CsvParser.ParseFromFile(
                filePath, 
                true,
                EntityReader.GetDataTable(shapeColumnNames, _entityType).Columns.Count));
        }

        public virtual bool RefreshFromServer()
        {
            UIHelper.DisplayInformation(string.Format(
                "{0} has no implementation for saving to server.",
                this.GetType().Name));
            return true;
        }

        public virtual bool SaveToServer()
        {
            UIHelper.DisplayInformation(string.Format(
                "{0} has no implementation for saving to server.",
                this.GetType().Name));
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
