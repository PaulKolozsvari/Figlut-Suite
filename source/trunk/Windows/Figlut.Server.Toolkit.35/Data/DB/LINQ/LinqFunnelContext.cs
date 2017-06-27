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

    #endregion //Using Directives

    /// <summary>
    /// A LINQ to SQL helper that uses generics to allow for easy Saving (Inserting/Updating), Retrieving and Deleting
    /// of entities.
    /// </summary>
    public class LinqFunnelContext
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
            _db.Connection.ConnectionString = settings.ConnectionString;
            _db.CommandTimeout = settings.LinqToSQLCommandTimeout;
            _db.DeferredLoadingEnabled = false;
        }

        #endregion //Constructors

        #region Fields

        private DataContext _db;
        private bool _contextIsFresh = true;

        #endregion //Fields

        #region Properties

        /// <summary>
        /// Get or set the DataContext.
        /// Throws an exception if attempting to set it to null or if attempting to retrieve
        /// it when it is null.
        /// </summary>
        protected DataContext DB
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
        public virtual List<LinqFunnelChangeResult> Save<E>(E entity, bool saveChildren) where E : class
        {
            PropertyInfo surrogateKey = GetEntitySurrogateKey<E>();
            bool containsIdentityColumn = IsIdentityColumn(surrogateKey);
            Type entityType = typeof(E);
            E original = null;
            object surrogateKeyValue = surrogateKey.GetValue(entity, null);
            original = GetEntityBySurrogateKey<E>(surrogateKeyValue, saveChildren);
            List<LinqFunnelChangeResult> result = null;
            if (original == null)
            {
                DB.GetTable<E>().InsertOnSubmit(entity);
                result = new List<LinqFunnelChangeResult>();
                result.Add(new LinqFunnelChangeResult()
                {
                    Function = "INSERT",
                    DateChanged = DateTime.Now,
                    EntityChanged = entityType.Name,
                    FieldChanged = surrogateKey.Name,
                    NewValue = surrogateKey.GetValue(entity, null)
                });
            }
            else
            {
                result = UpdateOriginalEntity<E>(original, entity, saveChildren);
            }
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
            PropertyInfo[] properties = typeof(E).GetProperties();
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
                        typeof(E).Name));
                }
                surrogateKey = p;
            }
            if (surrogateKey == null)
            {
                throw new NullReferenceException(string.Format("{0} does not have surrogate key.", typeof(E).Name));
            }
            return surrogateKey;
        }

        /// <summary>
        /// Determines whether a property is an identity column.
        /// </summary>
        /// <param name="p"></param>
        /// <returns>Returns true if the property is an identity column.</returns>
        public virtual bool IsIdentityColumn(PropertyInfo p)
        {
            object[] attributes = p.GetCustomAttributes(typeof(ColumnAttribute), false);
            ColumnAttribute columnAttribute = attributes.Length < 1 ? null : (ColumnAttribute)attributes[0];
            if (columnAttribute == null)
            {
                return false;
            }
            return columnAttribute.DbType.Contains("IDENTITY");
        }

        /// <summary>
        /// Updates the original entity with values of the latest entities. In other words, it copies the
        /// column values of the latest entity to the original entity.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table whose original record will be updated.</typeparam>
        /// <param name="original">The original entity retrieved from the database.</param>
        /// <param name="latest">The latest entity received from the client.</param>
        /// <returns>Returns a list of change results containing all the fields that were changed and their original and new values.</returns>
        private List<LinqFunnelChangeResult> UpdateOriginalEntity<E>(E original, E latest, bool saveChildren)
        {
            List<LinqFunnelChangeResult> result = new List<LinqFunnelChangeResult>();
            Type entityType = typeof(E);
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
                    Function = "UPDATE FIELD",
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
        public virtual List<LinqFunnelChangeResult> Delete<E>(E entity) where E : class
        {
            return this.Delete<E, object>(entity, false);
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
        public virtual List<LinqFunnelChangeResult> Delete<E, T>(E entity, bool createTombstone)
            where E : class
            where T : class
        {
            PropertyInfo surrogateKey = GetEntitySurrogateKey<E>();
            E original = GetEntityBySurrogateKey<E>(surrogateKey.GetValue(entity, null), false);
            if (original == null)
            {
                throw new Exception(
                    string.Format("Could not find entity with key {0} and value {1} to delete.",
                    GetEntitySurrogateKey<E>().Name,
                    GetEntitySurrogateKey<E>().GetValue(entity, null)));
            }
            if (createTombstone)
            {
                T tombstone = Activator.CreateInstance<T>();
                CopyToTombstoneEntity<E, T>(original, tombstone);
                T existingTombstone = GetEntityBySurrogateKey<T>(surrogateKey.GetValue(entity, null), false);
                if (existingTombstone != null)
                {
                    DB.GetTable<T>().DeleteOnSubmit(existingTombstone);
                }
                DB.GetTable<T>().InsertOnSubmit(tombstone);
            }
            DB.GetTable<E>().DeleteOnSubmit(original);
            DB.SubmitChanges();

            List<LinqFunnelChangeResult> result = new List<LinqFunnelChangeResult>();
            result.Add(new LinqFunnelChangeResult()
            {
                Function = "DELETE",
                DateChanged = DateTime.Now,
                EntityChanged = typeof(E).Name,
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
            DateTime threshold = DateTime.Now.Subtract(time);
            Table<E> table = DB.GetTable<E>();
            PropertyInfo dateField = typeof(E).GetProperty(dateFieldName);
            if ((dateField == null) || (dateField.PropertyType != typeof(DateTime)))
            {
                throw new Exception(
                    string.Format("Entity {0} does not contain the DateTime field with the name {1}",
                    typeof(E).GetType().Name,
                    dateFieldName));
            }
            List<E> toDelete = new List<E>();
            foreach (E e in table)
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
            Table<E> table = DB.GetTable<E>();
            table.DeleteAllOnSubmit(table.ToList<E>());
            DB.SubmitChanges();

            List<LinqFunnelChangeResult> result = new List<LinqFunnelChangeResult>();
            result.Add(new LinqFunnelChangeResult()
            {
                Function = "DELETE ALL",
                DateChanged = DateTime.Now,
                EntityChanged = typeof(E).Name,
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
            Type originalType = typeof(E);
            Type tombstoneType = typeof(T);
            PropertyInfo[] properties = originalType.GetProperties();
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
        /// Queries for and returns an entity from the table corresponding to the entity type. The query is performed
        /// on the surrogate key of the entity.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. which table the entity will be queried from.</typeparam>
        /// <param name="keyValue"></param>
        /// <returns>Returns an entity with the specified type and surrogate key. Returns null if one is not found.</returns>
        public virtual E GetEntityBySurrogateKey<E>(object keyValue, bool loadChildren) where E : class
        {
            SetDeferredLoadingEnabled(loadChildren);
            PropertyInfo surrogateKey = GetEntitySurrogateKey<E>();
            List<E> results = new List<E>();
            _contextIsFresh = false;
            foreach (E t in DB.GetTable<E>())
            {
                if (object.Equals(surrogateKey.GetValue(t, null), keyValue))
                {
                    return t;
                }
            }
            return null;
        }

        /// <summary>
        /// Queries for entities in a table corresponding to entity type. The query is performed on the column/field
        /// specified with the specified field value.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be returned.</typeparam>
        /// <param name="fieldName">The name of the field/column on which the query will be performed.</param>
        /// <param name="fieldValue">The value of the field which will be used for the query.</param>
        /// <returns>Returns a list of entities of the specified type with the specified field/column and field value.</returns>
        public virtual List<E> GetEntitiesByField<E>(string fieldName, object fieldValue, bool loadChildren) where E : class
        {
            SetDeferredLoadingEnabled(loadChildren);
            PropertyInfo field = typeof(E).GetProperty(fieldName);
            if (field == null)
            {
                throw new NullReferenceException(
                    string.Format("Entity {0} does not contain a field with the name {1}.",
                    typeof(E).Name,
                    fieldName));
            }
            List<E> results = new List<E>();
            foreach (E t in DB.GetTable<E>())
            {
                if (object.Equals(field.GetValue(t, null), fieldValue))
                {
                    results.Add(t);
                }
            }
            _contextIsFresh = false;
            return results;
        }

        /// <summary>
        /// Queries for all the entities in a table corresponging to the specfied entity type.
        /// </summary>
        /// <typeparam name="E">The entity type i.e. the table from which the entities will be returned.</typeparam>
        /// <returns>Returns all the entities of the specified type.</returns>
        public virtual List<E> GetAllEntities<E>(bool loadChildren) where E : class
        {
            if (loadChildren)
            {
                DB.DeferredLoadingEnabled = true;
            }
            _contextIsFresh = false;
            return DB.GetTable<E>().ToList();
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

        #endregion //Methods
    }
}