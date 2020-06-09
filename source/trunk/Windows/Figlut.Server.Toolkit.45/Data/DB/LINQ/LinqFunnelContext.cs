namespace Figlut.Server.Toolkit.Data.DB.LINQ
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Reflection;
    using System.Data.Linq.Mapping;
    using System.Data.Linq;
    using System.Linq.Expressions;
    using System.Collections;
    using System.Text;

    #endregion //Using Directives

    /// <summary>
    /// A LINQ to SQL helper that uses generics to allow for easy Saving (Inserting/Updating), Retrieving and Deleting
    /// of entities.
    /// </summary>
    public class LinqFunnelContext : IDisposable
    {
        //TODO Implement CompiledQuery only available with .NET 4.0 : http://msdn.microsoft.com/en-us/library/bb896297.aspx

        #region Constructors

        /// <summary>
        /// Creates a new LINQ to SQL context. 
        /// </summary>
        /// <param name="db">The LINQ to SQL DataContext that must contain all the entity types etc.</param>
        /// <param name="applyContextSettings">Determines whether the settings in the settings file should be applied to the DataContext</param>
        public LinqFunnelContext(DataContext db, LinqFunnelSettings settings)
        {
            DB = db;
            _linqFunnelSettings = settings;
            _db.Connection.ConnectionString = _linqFunnelSettings.ConnectionString;
            _db.CommandTimeout = _linqFunnelSettings.LinqToSQLCommandTimeout;
            _db.DeferredLoadingEnabled = false;
        }

        #endregion //Constructors

        #region Fields

        protected DataContext _db;
        protected bool _contextIsFresh = true;
        protected LinqFunnelSettings _linqFunnelSettings;

        #endregion //Fields

        #region Properties

        public LinqFunnelSettings LinqFunnelSettings
        {
            get { return _linqFunnelSettings; }
        }

        /// <summary>
        /// Get or set the DataContext.
        /// Throws an exception if attempting to set it to null or if attempting to retrieve
        /// it when it is null.
        /// </summary>
        public DataContext DB
        {
            get
            {
                if (_db == null)
                {
                    throw new Exception("DataContext has not been set.");
                }
                return _db;
            }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException("DataContext may not be set to null.");
                }
                _db = value;
            }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Saves (updates/inserts) an entity to the table corrseponding to the entity type.
        /// If the entity's surrogate key is an identity entity will be inserted and not updated.
        /// </summary>
        /// <typeparam name="E">The type of the entity i.e. which table it will be saved to.</typeparam>
        /// <param name="entity">The the entity to save.</param>
        /// <returns>Returns a list of change results i.e. what entities where updated</returns>
        public virtual List<LinqFunnelChangeResult> Save<E>(E entity, object entityIdentifier, bool saveChildren) where E : class
        {
            return Save(typeof(E), entity, entityIdentifier, saveChildren);
        }

        /// <summary>
        /// Saves (updates/inserts) an entity to the table corrseponding to the entity type.
        /// If the entity's surrogate key is an identity entity will be inserted and not updated.
        /// </summary>
        /// <typeparam name="E">The type of the entity i.e. which table it will be saved to.</typeparam>
        /// <param name="entity">The the entity to save.</param>
        /// <returns>Returns a list of change results i.e. what entities where updated</returns>
        public virtual List<LinqFunnelChangeResult> Save(Type entityType, object entity, object entityIdentifier, bool saveChildren)
        {
            PropertyInfo surrogateKey = GetEntitySurrogateKey(entityType);
            bool containsIdentityColumn = IsIdentityColumn(surrogateKey);
            object original = null;
            object surrogateKeyValue = surrogateKey.GetValue(entity, null);
            original = GetEntityBySurrogateKey(entityType, surrogateKeyValue, saveChildren);
            List<LinqFunnelChangeResult> result = null;
            if (original == null)
            {
                DB.GetTable(entityType).InsertOnSubmit(entity);
                result = new List<LinqFunnelChangeResult>();
                result.Add(new LinqFunnelChangeResult()
                {
                    SurrogateKey = surrogateKeyValue,
                    EntityIdentifier = entityIdentifier,
                    Function = "INSERT",
                    DateChanged = DateTime.Now,
                    EntityChanged = entityType.Name,
                    FieldChanged = surrogateKey.Name,
                    NewValue = surrogateKey.GetValue(entity, null)
                });
            }
            else
            {
                result = UpdateOriginalEntity(entityType, original, entity, surrogateKeyValue, entityIdentifier, saveChildren);
            }
            DB.SubmitChanges();
            _contextIsFresh = false;
            return result;
        }

        public virtual List<LinqFunnelChangeResult> Insert<E>(E entity, object entityIdentifier, bool saveChildren) where E : class
        {
            return Insert(typeof(E), entity, entityIdentifier, saveChildren);
        }

        public virtual List<LinqFunnelChangeResult> Insert(Type entityType, object entity, object entityIdentifier, bool saveChildren)
        {
            PropertyInfo surrogateKey = GetEntitySurrogateKey(entityType);
            bool containsIdentityColumn = IsIdentityColumn(surrogateKey);
            object surrogateKeyValue = surrogateKey.GetValue(entity, null);

            DB.GetTable(entityType).InsertOnSubmit(entity);
            List<LinqFunnelChangeResult> result = new List<LinqFunnelChangeResult>();
            result.Add(new LinqFunnelChangeResult()
            {
                SurrogateKey = surrogateKeyValue,
                EntityIdentifier = entityIdentifier,
                Function = "INSERT",
                DateChanged = DateTime.Now,
                EntityChanged = entityType.Name,
                FieldChanged = surrogateKey.Name,
                NewValue = surrogateKey.GetValue(entity, null)
            });
            DB.SubmitChanges();
            _contextIsFresh = false;
            return result;
        }

        /// <summary>
        /// Determines the primary key of an entity type. The first primary key found on the entity type i.e.
        /// the assumption is made that the entity type only has one primary key, which is the surrogate key.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table whose surrogate key needs to be determined.</typeparam>
        /// <returns>Retruns the PropertyInfo corresponding to the column which is the surrogate key for the specified entity type.</returns>
        public virtual PropertyInfo GetEntitySurrogateKey<E>()
        {
            return EntityReader.GetLinqToSqlEntitySurrogateKey<E>();
        }

        /// <summary>
        /// Determines the primary key of an entity type. The first primary key found on the entity type i.e.
        /// the assumption is made that the entity type only has one primary key, which is the surrogate key.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table whose surrogate key needs to be determined.</typeparam>
        /// <returns>Retruns the PropertyInfo corresponding to the column which is the surrogate key for the specified entity type.</returns>
        public virtual PropertyInfo GetEntitySurrogateKey(Type entityType)
        {
            return EntityReader.GetLinqToSqlEntitySurrogateKey(entityType);
        }

        /// <summary>
        /// Determines whether a property is an identity column.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Returns true if the property is an identity column.</returns>
        public virtual bool IsIdentityColumn(PropertyInfo p)
        {
            return EntityReader.IsLinqToSqlEntityPropertyIdentityColumn(p);
        }

        /// <summary>
        /// Updates the original entity with values of the latest entities. In other words, it copies the
        /// column values of the latest entity to the original entity.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table whose original record will be updated.</typeparam>
        /// <param name="original">The original entity retrieved from the database.</param>
        /// <param name="latest">The latest entity received from the client.</param>
        /// <returns>Returns a list of change results containing all the fields that were changed and their original and new values.</returns>
        private List<LinqFunnelChangeResult> UpdateOriginalEntity<E>(E original, E latest, object surrogateKeyValue, object entityIdentifier, bool saveChildren) where E : class
        {
            return UpdateOriginalEntity(typeof(E), original, latest, surrogateKeyValue, entityIdentifier, saveChildren);
        }

        /// <summary>
        /// Updates the original entity with values of the latest entities. In other words, it copies the
        /// column values of the latest entity to the original entity.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table whose original record will be updated.</typeparam>
        /// <param name="original">The original entity retrieved from the database.</param>
        /// <param name="latest">The latest entity received from the client.</param>
        /// <returns>Returns a list of change results containing all the fields that were changed and their original and new values.</returns>
        private List<LinqFunnelChangeResult> UpdateOriginalEntity(
            Type entityType, 
            object original, 
            object latest, 
            object surrogateKeyValue,
            object entityIdentifier,
            bool saveChildren)
        {
            if (entityType != original.GetType())
            {
                throw new ArgumentException(string.Format(
                    "Entity Type of {0} does not match the original entity's type of {1}.", 
                    entityType.FullName, 
                    original.GetType().FullName));
            }
            if (original.GetType() != latest.GetType())
            {
                throw new ArgumentException(string.Format(
                    "Cannot update original of type {0} because latest entity is of type {1}.",
                    original.GetType().FullName,
                    latest.GetType().FullName));
            }
            List<LinqFunnelChangeResult> result = new List<LinqFunnelChangeResult>();
            PropertyInfo[] properties = entityType.GetProperties();
            foreach (PropertyInfo p in properties)
            {
                object[] columnAttributes = p.GetCustomAttributes(typeof(ColumnAttribute), false);
                ColumnAttribute columnAttribute = columnAttributes.Length < 1 ? null : (ColumnAttribute)columnAttributes[0];
                object[] associationAttributes = p.GetCustomAttributes(typeof(AssociationAttribute), false);
                AssociationAttribute associationAttribute = associationAttributes.Length < 1 ? null : (AssociationAttribute)associationAttributes[0];
                if (!saveChildren && associationAttribute != null)
                {
                    continue;//Children should not be saved and this is a property holding the children.
                }
                if ((columnAttribute == null || columnAttribute.IsPrimaryKey) && associationAttribute == null)
                {
                    continue;
                }
                object originalValue = p.GetValue(original, null);
                object latestValue = p.GetValue(latest, null);
                if (object.Equals(originalValue, latestValue))
                {
                    continue;
                }
                result.Add(new LinqFunnelChangeResult()
                {
                    SurrogateKey = surrogateKeyValue,
                    EntityIdentifier = entityIdentifier,
                    Function = "UPDATE",
                    DateChanged = DateTime.Now,
                    EntityChanged = entityType.Name,
                    FieldChanged = p.Name,
                    OriginalValue = originalValue,
                    NewValue = latestValue,
                });
                p.SetValue(original, latestValue, null);
            }
            _contextIsFresh = false;
            return result;
        }

        /// <summary>
        /// Deletes an entity from the table corresponding to the entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. which table it will be deleted from.</typeparam>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>Returns a list of change results.</returns>
        public virtual List<LinqFunnelChangeResult> Delete<E>(E entity, object entityIdentifier) where E : class
        {
            return this.Delete<E, object>(entity, entityIdentifier, false);
        }

        /// <summary>
        /// Deletes an entity from the table corresponding to the entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. which table it will be deleted from.</typeparam>
        /// <param name="entity">The entity to be deleted.</param>
        /// <returns>Returns a list of change results.</returns>
        public virtual List<LinqFunnelChangeResult> Delete(object entity, object entityIdentifier)
        {
            return this.Delete(entity, entityIdentifier, false, null);
        }

        /// <summary>
        /// Deletes an entity from the table correspoding to the entity type (E) and creates
        /// a tombstone in the table correspoging to the tombstone entity type (T) if the 
        /// createTombstone flag is set to true.
        /// </summary>
        /// <typeparam name="E">The entity type of the entity which will be deleted i.e. the table from where it will be deleted.</typeparam>
        /// <typeparam name="T">The tombstone entity type i.e. the table where an tombstone will be created.</typeparam>
        /// <param name="entity">The entity to be deleted</param>
        /// <param name="createTombstone">Indicates whether a tombstone should be created.</param>
        /// <returns>Returns a list of change results.</returns>
        public virtual List<LinqFunnelChangeResult> Delete<E, T>(E entity, object entityIdentifier, bool createTombstone)
            where E : class
            where T : class
        {
            return Delete(entity, entityIdentifier, createTombstone, typeof(T));
        }

        /// <summary>
        /// Deletes an entity from the table correspoding to the entity type (E) and creates
        /// a tombstone in the table correspoging to the tombstone entity type (T) if the 
        /// createTombstone flag is set to true.
        /// </summary>
        /// <typeparam name="E">The entity type of the entity which will be deleted i.e. the table from where it will be deleted.</typeparam>
        /// <typeparam name="T">The tombstone entity type i.e. the table where an tombstone will be created.</typeparam>
        /// <param name="entity">The entity to be deleted</param>
        /// <param name="createTombstone">Indicates whether a tombstone should be created.</param>
        /// <returns>Returns a list of change results.</returns>
        public virtual List<LinqFunnelChangeResult> Delete(object entity, object entityIdentifier, bool createTombstone, Type tombstoneType)
        {
            Type entityType = entity.GetType();
            PropertyInfo surrogateKey = GetEntitySurrogateKey(entityType);
            object surrogateKeyValue = surrogateKey.GetValue(entity, null);
            object original = GetEntityBySurrogateKey(entityType, surrogateKeyValue, false);
            if (original == null)
            {
                throw new Exception(
                    string.Format("Could not find entity with key {0} and value {1} to delete.",
                    GetEntitySurrogateKey(entityType).Name,
                    GetEntitySurrogateKey(entityType).GetValue(entity, null)));
            }
            if (createTombstone)
            {
                object tombstone = Activator.CreateInstance(tombstoneType);
                CopyToTombstoneEntity(entityType, tombstoneType, original, tombstone);
                object existingTombstone = GetEntityBySurrogateKey(tombstoneType, surrogateKey.GetValue(entity, null), false);
                if (existingTombstone != null)
                {
                    DB.GetTable(tombstoneType).DeleteOnSubmit(existingTombstone);
                }
                DB.GetTable(tombstoneType).InsertOnSubmit(tombstone);
            }
            DB.GetTable(entityType).DeleteOnSubmit(original);
            DB.SubmitChanges();

            List<LinqFunnelChangeResult> result = new List<LinqFunnelChangeResult>();
            result.Add(new LinqFunnelChangeResult()
            {
                SurrogateKey = surrogateKeyValue,
                EntityIdentifier = entityIdentifier,
                Function = "DELETE",
                DateChanged = DateTime.Now,
                EntityChanged = entityType.Name,
                FieldChanged = surrogateKey.Name,
            }); ;
            _contextIsFresh = false;
            return result;
        }

        /// <summary>
        /// Deletes an entity from the table corresponding to the entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. which table it will be deleted from.</typeparam>
        /// <param name="surrogatekeyValue">The entity to be deleted.</param>
        /// <returns>Returns a list of change results.</returns>
        public virtual List<LinqFunnelChangeResult> DeleteBySurrogateKey<E>(object surrogateKeyValue, object entityIdentifier) where E : class
        {
            return this.DeleteBySurrogateKey<E, object>(surrogateKeyValue, entityIdentifier, false);
        }

        /// <summary>
        /// Deletes an entity from the table corresponding to the entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. which table it will be deleted from.</typeparam>
        /// <param name="surrogatekeyValue">The entity to be deleted.</param>
        /// <returns>Returns a list of change results.</returns>
        public virtual List<LinqFunnelChangeResult> DeleteBySurrogateKey(object surrogateKeyValue, object entityIdentifier, Type entityType)
        {
            return this.DeleteBySurrogateKey(surrogateKeyValue, entityIdentifier, false, entityType, null);
        }

        /// <summary>
        /// Deletes an entity from the table with the specified surrogate key correspoding to the entity type (E) and creates
        /// a tombstone in the table correspoging to the tombstone entity type (T) if the 
        /// createTombstone flag is set to true.
        /// </summary>
        /// <typeparam name="E">The entity type of the entity which will be deleted i.e. the table from where it will be deleted.</typeparam>
        /// <typeparam name="T">The tombstone entity type i.e. the table where an tombstone will be created.</typeparam>
        /// <param name="surrogateKeyValue">The surrogate key of the entity to be deleted</param>
        /// <param name="createTombstone">Indicates whether a tombstone should be created.</param>
        /// <returns>Returns a list of change results.</returns>
        public virtual List<LinqFunnelChangeResult> DeleteBySurrogateKey<E, T>(object surrogateKeyValue, object entityIdentifier, bool createTombstone)
            where E : class
            where T : class
        {
            return DeleteBySurrogateKey(surrogateKeyValue, entityIdentifier, createTombstone, typeof(E), typeof(T));
        }

        /// <summary>
        /// Deletes an entity from the table with the specified surrogate key correspoding to the entity type (E) and creates
        /// a tombstone in the table correspoging to the tombstone entity type (T) if the 
        /// createTombstone flag is set to true.
        /// </summary>
        /// <typeparam name="E">The entity type of the entity which will be deleted i.e. the table from where it will be deleted.</typeparam>
        /// <typeparam name="T">The tombstone entity type i.e. the table where an tombstone will be created.</typeparam>
        /// <param name="surrogateKeyValue">The surrogate key of the entity to be deleted</param>
        /// <param name="createTombstone">Indicates whether a tombstone should be created.</param>
        /// <returns>Returns a list of change results.</returns>
        public virtual List<LinqFunnelChangeResult> DeleteBySurrogateKey(
            object surrogateKeyValue,
            object entityIdentifier,
            bool createTombstone, 
            Type entityType,
            Type tombstoneType)
        {
            PropertyInfo surrogateKey = GetEntitySurrogateKey(entityType);
            object original = GetEntityBySurrogateKey(entityType, surrogateKeyValue, false);
            if (original == null)
            {
                throw new Exception(
                    string.Format("Could not find entity with key {0} and value {1} to delete.",
                    GetEntitySurrogateKey(entityType).Name,
                    surrogateKeyValue));
            }
            if (createTombstone)
            {
                object tombstone = Activator.CreateInstance(tombstoneType);
                CopyToTombstoneEntity(original, tombstone);
                object existingTombstone = GetEntityBySurrogateKey(tombstoneType, surrogateKeyValue, false);
                if (existingTombstone != null)
                {
                    DB.GetTable(tombstoneType).DeleteOnSubmit(existingTombstone);
                }
                DB.GetTable(tombstoneType).InsertOnSubmit(tombstone);
            }
            DB.GetTable(entityType).DeleteOnSubmit(original);
            DB.SubmitChanges();

            List<LinqFunnelChangeResult> result = new List<LinqFunnelChangeResult>();
            result.Add(new LinqFunnelChangeResult()
            {
                SurrogateKey = surrogateKeyValue, 
                EntityIdentifier = entityIdentifier,
                Function = "DELETE",
                DateChanged = DateTime.Now,
                EntityChanged = entityType.Name,
                FieldChanged = surrogateKey.Name,
            });
            _contextIsFresh = false;
            return result;
        }

        /// <summary>
        /// Deletes all the entities in a given table older than the time specified.
        /// </summary>
        /// <typeparam name="E">The entity type of the entity which will be deleted i.e. the table from where it will be deleted.</typeparam>
        /// <param name="dateFieldName">The field name on the entity which must a date time field .</param>
        /// <param name="time">The time relative to the current time i.e. current time subracted by the this time sets the threshhold for entities deleted.</param>
        public virtual void DeleteOlderThan<E>(string dateFieldName, TimeSpan time) where E : class
        {
            DeleteOlderThan(typeof(E), dateFieldName, time);
        }

                /// <summary>
        /// Deletes all the entities in a given table older than the time specified.
        /// </summary>
        /// <typeparam name="E">The entity type of the entity which will be deleted i.e. the table from where it will be deleted.</typeparam>
        /// <param name="dateFieldName">The field name on the entity which must a date time field .</param>
        /// <param name="time">The time relative to the current time i.e. current time subracted by the this time sets the threshhold for entities deleted.</param>
        public virtual void DeleteOlderThan(Type entityType, string dateFieldName, TimeSpan time)
        {
            DateTime threshold = DateTime.Now.Subtract(time);
            ITable table = DB.GetTable(entityType);
            PropertyInfo dateField = entityType.GetProperty(dateFieldName);
            if ((dateField == null) || (dateField.PropertyType != typeof(DateTime)))
            {
                throw new Exception(
                    string.Format("Entity {0} does not contain the DateTime field with the name {1}",
                    entityType.Name,
                    dateFieldName));
            }
            List<object> toDelete = new List<object>();
            foreach (object e in table)
            {
                if ((DateTime)dateField.GetValue(e, null) < threshold)
                {
                    table.DeleteOnSubmit(e);
                }
            }
            DB.SubmitChanges();
            _contextIsFresh = false;
        }

        /// <summary>
        /// Deletes all entities from the table corresponding to the entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. of the table whose records (entities) will be deleted.</typeparam>
        public virtual List<LinqFunnelChangeResult> DeleteAll<E>() where E : class
        {
            return DeleteAll(typeof(E));
        }

        /// <summary>
        /// Deletes all entities from the table corresponding to the entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. of the table whose records (entities) will be deleted.</typeparam>
        public virtual List<LinqFunnelChangeResult> DeleteAll(Type entityType)
        {
            ITable table = DB.GetTable(entityType);
            List<object> entities = new List<object>();
            foreach (object e in table)
            {
                entities.Add(e);
            }
            table.DeleteAllOnSubmit(entities);
            DB.SubmitChanges();

            List<LinqFunnelChangeResult> result = new List<LinqFunnelChangeResult>();
            result.Add(new LinqFunnelChangeResult()
            {
                Function = "DELETE ALL",
                DateChanged = DateTime.Now,
                EntityChanged = entityType.Name,
            });
            _contextIsFresh = false;
            return result;
        }

        /// <summary>
        /// Copies all the values from the original entity to a tombstone entity. If the fields/columns on the two entities
        /// do not match an exception will be thrown.
        /// </summary>
        /// <param name="original">The original entity retrieved from the database.</param>
        /// <param name="tombstone">The tombstone entity containing the same fields/columns as the original entity.</param>
        public virtual void CopyToTombstoneEntity<E, T>(E original, T tombstone)
            where E : class
            where T : class
        {
            CopyToTombstoneEntity(typeof(E), typeof(T), original, tombstone);
        }

        /// <summary>
        /// Copies all the values from the original entity to a tombstone entity. If the fields/columns on the two entities
        /// do not match an exception will be thrown.
        /// </summary>
        /// <param name="original">The original entity retrieved from the database.</param>
        /// <param name="tombstone">The tombstone entity containing the same fields/columns as the original entity.</param>
        public virtual void CopyToTombstoneEntity(Type entityType, Type tombstoneType, object original, object tombstone)
        {
            PropertyInfo[] properties = entityType.GetProperties();
            foreach (PropertyInfo p in properties)
            {
                object[] attributes = p.GetCustomAttributes(typeof(ColumnAttribute), false);
                ColumnAttribute columnAttribute = attributes.Length < 1 ? null : (ColumnAttribute)attributes[0];
                if (columnAttribute == null)
                {
                    continue;
                }
                PropertyInfo tombstoneProperty = tombstoneType.GetProperty(p.Name);
                if (tombstoneProperty == null)
                {
                    throw new NullReferenceException(
                        string.Format(
                        "Could not find property on tombstone entity with the name {0}.",
                        p.Name));
                }
                object originalValue = p.GetValue(original, null);
                tombstoneProperty.SetValue(tombstone, originalValue, null);
            }
        }

        private void SetDeferredLoadingEnabled(bool value)
        {
            if (_contextIsFresh && DB.DeferredLoadingEnabled != value)
            {
                DB.DeferredLoadingEnabled = value;
            }
        }

        /// <summary>
        /// Queries for and returns the first entity filtered by the specified expression.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. which table the entity will be queried from.</typeparam>
        /// <param name="expression">The expression to use to filter by.</param>
        /// <returns>Returns the first entity filtered by the specified expression.</returns>
        public virtual E GetFirstEntity<E>(Expression<Func<E, bool>> expression) where E : class
        {
            return GetFirstEntity<E>(expression, false);
        }

        /// <summary>
        /// Queries for and returns the first entity filtered by the specified expression.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. which table the entity will be queried from.</typeparam>
        /// <param name="expression">The expression to use to filter by.</param>
        /// <param name="throwExceptionOnNotFound">Whether or not to to throw an exception if the result is null.</param>
        /// <returns>Returns the first entity filtered by the specified expression.</returns>
        public virtual E GetFirstEntity<E>(Expression<Func<E, bool>> expression, bool throwExceptionOnNotFound) where E : class
        {
            E result = DB.GetTable<E>().Where(expression.Compile()).FirstOrDefault();
            if (result == null && throwExceptionOnNotFound)
            {
                throw new Exception(string.Format("Could not find {0} for expression {1}'.",
                    typeof(E).Name,
                    expression.ToString()));
            }
            return result;
        }

        /// <summary>
        /// Queries for and returns a list of entities filtered by the specified expression.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. which table the entity will be queried from.</typeparam>
        /// <param name="expression">The expression to use to filter by.</param>
        /// <returns>Returns a list of entities filtered by the specified expression.</returns>
        public virtual List<E> GetEntities<E>(Expression<Func<E, bool>> expression) where E : class
        {
            return DB.GetTable<E>().Where(expression.Compile()).ToList();
        }

        //public virtual object GetEntityBySurrogateKey(Type entityType, object keyValue, bool loadChildren, bool throwExceptionOnNotFound)
        //{
        //    SetDeferredLoadingEnabled(loadChildren);
        //    PropertyInfo surrogateKey = GetEntitySurrogateKey(entityType);
        //    List<object> results = new List<object>();
        //    _contextIsFresh = false;
        //    string keyValueString = keyValue.ToString();
        //    foreach (object t in DB.GetTable(entityType))
        //    {
        //        object surrogateKeyValue = surrogateKey.GetValue(t, null);
        //        if (object.Equals(surrogateKeyValue, keyValue) || (surrogateKeyValue.ToString() == keyValueString))
        //        {
        //            return t;
        //        }
        //    }
        //    if (throwExceptionOnNotFound)
        //    {
        //        throw new Exception(string.Format("Could not find {0} with {1} of '{2}'.",
        //            entityType.Name,
        //            surrogateKey.Name,
        //            keyValue));
        //    }
        //    return null;
        //}

        /// <summary>
        /// Queries for and returns an entity from the table corresponding to the entity type. The query is performed
        /// on the surrogate key of the entity.
        /// </summary>
        /// <param name="entityType">The entity type i.e. which table the entity will be queried from.</param>
        /// <param name="keyValue">The value of the surrogate to search by.</param>
        /// <param name="loadChildren">Whether or not to load the children entities onto this entity.</param>
        /// <returns>Returns an entity with the specified type and surrogate key. Returns null if one is not found.</returns>
        public virtual object GetEntityBySurrogateKey(Type entityType, object keyValue, bool loadChildren)
        {
            return GetEntityBySurrogateKey(entityType, keyValue, loadChildren, false);
        }

        /// <summary>
        /// Queries for and returns an entity from the table corresponding to the entity type. The query is performed
        /// on the surrogate key of the entity.
        /// </summary>
        /// <param name="entityType">The entity type i.e. which table the entity will be queried from.</param>
        /// <param name="keyValue">The value of the surrogate to search by.</param>
        /// <param name="loadChildren">Whether or not to load the children entities onto this entity.</param>
        /// <param name="throwExceptionOnNotFound">Whether or not to throw an exception if the result is null.</param>
        /// <returns>Returns an entity with the specified type and surrogate key. Returns null if one is not found.</returns>
        public virtual object GetEntityBySurrogateKey(Type entityType, object keyValue, bool loadChildren, bool throwExceptionOnNotFound)
        {
            MethodInfo methodDefinition = GetType().GetMethod("GetEntityBySurrogateKey", new Type[] { typeof(object), typeof(bool), typeof(bool)}); //https://stackoverflow.com/questions/266115/pass-an-instantiated-system-type-as-a-type-parameter-for-a-generic-class
            MethodInfo method = methodDefinition.MakeGenericMethod(entityType);
            return method.Invoke(this, new object[] { keyValue, loadChildren, throwExceptionOnNotFound });
        }

        /// <summary>
        /// Queries for and returns an entity from the table corresponding to the entity type. The query is performed
        /// on the surrogate key of the entity.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. which table the entity will be queried from.</typeparam>
        /// <param name="keyValue">The value of the surrogate to search by.</param>
        /// <param name="loadChildren">Whether or not to load the children entities onto this entity.</param>
        /// <returns>Returns an entity with the specified type and surrogate key. Returns null if one is not found.</returns>
        public virtual E GetEntityBySurrogateKey<E>(object keyValue, bool loadChildren) where E : class
        {
            return GetEntityBySurrogateKey<E>(keyValue, loadChildren, false);
        }

        /// <summary>
        /// Queries for and returns an entity from the table corresponding to the entity type. The query is performed
        /// on the surrogate key of the entity.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. which table the entity will be queried from.</typeparam>
        /// <param name="keyValue">The value of the surrogate to search by.</param>
        /// <param name="loadChildren">Whether or not to load the children entities onto this entity.</param>
        /// <param name="throwExceptionOnNotFound">Whether or not to throw an exception if the result is null.</param>
        /// <returns>Returns an entity with the specified type and surrogate key. Returns null if one is not found.</returns>
        public virtual E GetEntityBySurrogateKey<E>(object keyValue, bool loadChildren, bool throwExceptionOnNotFound) where E : class
        {
            SetDeferredLoadingEnabled(loadChildren);
            Type entityType = typeof(E);
            PropertyInfo surrogateKey = GetEntitySurrogateKey(entityType);
            object keyValueConverted = EntityReader.ConvertValueTypeTo(keyValue, surrogateKey.PropertyType);
            ParameterExpression e = Expression.Parameter(entityType, "e");
            MemberExpression memberExpression = Expression.MakeMemberAccess(e, surrogateKey); //Name of surrogate key : left hand side of the expression.
            ConstantExpression constantExpression = Expression.Constant(keyValueConverted, surrogateKey.PropertyType); //Value of the surrogate key : right hand side of the expression.
            BinaryExpression binaryExpression = Expression.Equal(memberExpression, constantExpression);
            Expression<Func<E, bool>> lambdaExpression = Expression.Lambda<Func<E, bool>>(binaryExpression, e);
            return DB.GetTable<E>().SingleOrDefault(lambdaExpression);
        }

        /// <summary>
        /// Queries for entities in a table corresponding to entity type. The query is performed on the column/field
        /// specified with the specified field value.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be returned.</typeparam>
        /// <param name="fieldName">The name of the field/column on which the query will be performed.</param>
        /// <param name="fieldValue">The value of the field which will be used for the query.</param>
        /// <returns>Returns a list of entities of the specified type with the specified field/column and field value.</returns>
        //public virtual List<object> GetEntitiesByField(Type entityType, string fieldName, object fieldValue, bool loadChildren)
        //{
        //    SetDeferredLoadingEnabled(loadChildren);
        //    PropertyInfo field = entityType.GetProperty(fieldName);
        //    if (field == null)
        //    {
        //        throw new NullReferenceException(
        //            string.Format("Entity {0} does not contain a field with the name {1}.",
        //            entityType.Name,
        //            fieldName));
        //    }
        //    List<object> results = new List<object>();
        //    foreach (object t in DB.GetTable(entityType))
        //    {
        //        object eFieldValue = field.GetValue(t, null);
        //        if (eFieldValue.Equals(fieldValue) || eFieldValue.ToString().Equals(fieldValue.ToString()))
        //        {
        //            results.Add(t);
        //        }
        //    }
        //    _contextIsFresh = false;
        //    return results;
        //}

        /// <summary>
        /// Queries for entities in a table corresponding to entity type. The query is performed on the column/field
        /// specified with the specified field value.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be returned.</typeparam>
        /// <param name="fieldName">The name of the field/column on which the query will be performed.</param>
        /// <param name="fieldValue">The value of the field which will be used for the query.</param>
        /// <param name="loadChildren">Whether or not to load the children of the entities as well.</param>
        /// <returns>Returns a list of entities of the specified type with the specified field/column and field value.</returns>
        public virtual List<object> GetEntitiesByField(Type entityType, string fieldName, object fieldValue, bool loadChildren)
        {
            MethodInfo methodDefinition = GetType().GetMethod("GetEntitiesByField", new Type[] { typeof(string), typeof(object), typeof(bool) }); //https://stackoverflow.com/questions/266115/pass-an-instantiated-system-type-as-a-type-parameter-for-a-generic-class
            MethodInfo method = methodDefinition.MakeGenericMethod(entityType);
            object queryResult = method.Invoke(this, new object[] { fieldName, fieldValue, loadChildren });
            IList genericList = (IList)queryResult;
            List <object> result = new List<object>();
            foreach (var e in genericList)
            {
                result.Add(e);
            }
            return result;
        }

        /// <summary>
        /// Queries for entities in a table corresponding to entity type. The query is performed on the column/field
        /// specified with the specified field value.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be returned.</typeparam>
        /// <param name="fieldName">The name of the field/column on which the query will be performed.</param>
        /// <param name="fieldValue">The value of the field which will be used for the query.</param>
        /// <param name="loadChildren">Whether or not to load the children of the entities as well.</param>
        /// <returns>Returns a list of entities of the specified type with the specified field/column and field value.</returns>
        public virtual List<E> GetEntitiesByField<E>(string fieldName, object fieldValue, bool loadChildren) where E : class
        {
            SetDeferredLoadingEnabled(loadChildren);

            Type entityType = typeof(E);
            PropertyInfo field = entityType.GetProperty(fieldName);
            object keyValueConverted = EntityReader.ConvertValueTypeTo(fieldValue, field.PropertyType);
            ParameterExpression e = Expression.Parameter(entityType, "e");
            MemberExpression memberExpression = Expression.MakeMemberAccess(e, field); //Name of surrogate key : left hand side of the expression.
            ConstantExpression constantExpression = Expression.Constant(keyValueConverted, field.PropertyType); //Value of the surrogate key : right hand side of the expression.
            BinaryExpression binaryExpression = Expression.Equal(memberExpression, constantExpression);
            Expression<Func<E, bool>> lambdaExpression = Expression.Lambda<Func<E, bool>>(binaryExpression, e);
            return DB.GetTable<E>().Where(lambdaExpression).ToList();
        }

        /// <summary>
        /// Queries for all the entities in a table corresponging to the specfied entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be returned.</typeparam>
        /// <returns>Returns all the entities of the specified type.</returns>
        public virtual List<E> GetAllEntities<E>(bool loadChildren) where E : class
        {
            List<E> result = new List<E>();
            GetAllEntities(typeof(E), loadChildren).ForEach(e => result.Add((E)e));
            return result;
        }

        /// <summary>
        /// Queries for all the entities in a table corresponging to the specfied entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be returned.</typeparam>
        /// <returns>Returns all the entities of the specified type.</returns>
        public virtual List<object> GetAllEntities(Type entityType, bool loadChildren)
        {
            if (loadChildren)
            {
                DB.DeferredLoadingEnabled = true;
            }
            _contextIsFresh = false;
            List<object> result = new List<object>();
            foreach(object e in DB.GetTable(entityType))
            {
                result.Add(e);
            }
            return result;
        }

        /// <summary>
        /// Returns the total count of an entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be counted.</typeparam>
        /// <returns>Returns the total count of an entity type.</returns>
        public virtual int GetTotalCount<E>() where E : class
        {
            _contextIsFresh = false;
            return DB.GetTable<E>().Count();
        }

        /// <summary>
        /// Returns the total count of an entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be counted.</typeparam>
        /// <returns>Returns the total count of an entity type.</returns>
        public virtual long GetTotalCountLong<E>() where E : class
        {
            _contextIsFresh = false;
            return DB.GetTable<E>().LongCount();
        }

        /// <summary>
        /// Returns the total count of an entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be counted.</typeparam>
        /// <returns>Returns the total count of an entity type.</returns>
        public virtual int GetTotalCount(Type entityType)
        {
            _contextIsFresh = false;
            return GetAllEntities(entityType, false).Count;
        }

        /// <summary>
        /// Returns the total count of an entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be counted.</typeparam>
        /// <returns>Returns the total count of an entity type.</returns>
        public virtual long GetTotalCountLong(Type entityType)
        {
            _contextIsFresh = false;
            return GetAllEntities(entityType, false).LongCount();
        }

        /// <summary>
        /// Searches through all changes that caused a concurrency conflict in the DataContext and 
        /// compiles an error message listing all the conflicts on all tables and each member (field) that caused the conflicts.
        /// This method should be called after having called SubmitChanges on a DataContext and where a ChangeConflictException
        /// has occured.
        /// </summary>
        /// <returns></returns>
        public static string GetLinqToSqlChangeConflictErrorMessage(DataContext dataContext)
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("LINQ to SQL optimistic concurrency error:");
            result.AppendLine();
            foreach (ObjectChangeConflict conflict in dataContext.ChangeConflicts)
            {
                MetaTable metatable = dataContext.Mapping.GetTable(conflict.Object.GetType());
                result.AppendLine($"Conflict Table name: {metatable.TableName}");
                object entityInConflict = conflict.Object;
                PropertyInfo surrogateKey = EntityReader.GetLinqToSqlEntitySurrogateKey(entityInConflict.GetType());
                bool containsIdentityColumn = EntityReader.IsLinqToSqlEntityPropertyIdentityColumn(surrogateKey);
                object surrogateKeyValue = surrogateKey.GetValue(entityInConflict, null);
                result.Append($"Conflict Entity ID: {surrogateKeyValue}");
                foreach (MemberChangeConflict memberConflict in conflict.MemberConflicts)
                {
                    object currentValue = memberConflict.CurrentValue;
                    object originalValue = memberConflict.OriginalValue;
                    object databaseValue = memberConflict.DatabaseValue;
                    MemberInfo member = memberConflict.Member;
                    result.AppendLine($"Conflict Member: {member.Name}");
                    result.AppendLine($"Current Value: {currentValue}");
                    result.AppendLine($"Original Value {originalValue}");
                    result.AppendLine($"Database Value: {databaseValue}");
                }
                result.AppendLine();
            }
            return result.ToString();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
            }
        }

        #endregion //Methods
    }
}