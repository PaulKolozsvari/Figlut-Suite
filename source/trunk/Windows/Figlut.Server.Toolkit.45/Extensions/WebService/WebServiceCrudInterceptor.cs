namespace Figlut.Server.Toolkit.Extensions.WebService
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Extensions.ExtensionManaged;
    using Figlut.Server.Toolkit.Extensions.WebService.Events.Crud;

    #endregion //Using Directives

    public abstract class WebServiceCrudInterceptor
    {
        #region Constructors

        public WebServiceCrudInterceptor()
        {
            _extensionManagedEntityCache = new ExtensionManagedEntityCache();
            AddExtensionManagedEntities();
            SubscribeToCrudEvents();
        }

        #endregion //Constructors

        #region Fields

        protected ExtensionManagedEntityCache _extensionManagedEntityCache;

        #endregion //Fields

        #region Properties

        public ExtensionManagedEntityCache ExtensionManagedEntities
        {
            get { return _extensionManagedEntityCache; }
        }

        #endregion //Properties

        #region Events

        public event OnBeforeWebGetSqlTable OnBeforeWebGetSqlTable;
        public event OnAfterWebGetSqlTable OnAfterWebGetSqlTable;
        public event OnBeforeWebInvokeSqlTable OnBeforeWebInvokeSqlTable;
        public event OnAfterWebInvokeSqlTable OnAfterWebInvokeSqlTable;
        public event OnBeforeWebInvokeSql OnBeforeWebInvokeSql;
        public event OnAfterWebInvokeSql OnAfterWebInvokeSql;

        #endregion //Events

        #region Methods

        #region Event Firing Methods

        public void PerformOnBeforeWebGetSqlTable(BeforeWebGetSqlTableArgs e)
        {
            if (OnBeforeWebGetSqlTable != null)
            {
                OnBeforeWebGetSqlTable(this, e);
            }
        }

        public void PerformOnAfterWebGetSqlTable(AfterWebGetSqlTableArgs e)
        {
            if (OnAfterWebGetSqlTable != null)
            {
                OnAfterWebGetSqlTable(this, e);
            }
        }

        public void PerformOnBeforeWebInvokeSqlTable(BeforeWebInvokeSqlTableArgs e)
        {
            if (OnBeforeWebInvokeSqlTable != null)
            {
                OnBeforeWebInvokeSqlTable(this, e);
            }
        }

        public void PerformOnAfterWebInvokeSqlTable(AfterWebInvokeSqlTableArgs e)
        {
            if (OnAfterWebInvokeSqlTable != null)
            {
                OnAfterWebInvokeSqlTable(this, e);
            }
        }

        public void PerformOnBeforeWebInvokeSql(BeforeWebInvokeSqlArgs e)
        {
            if (OnBeforeWebInvokeSql != null)
            {
                OnBeforeWebInvokeSql(this, e);
            }
        }

        public void PerformOnAfterWebInvokeSql(AfterWebInvokeSqlArgs e)
        {
            if (OnAfterWebInvokeSqlTable != null)
            {
                OnAfterWebInvokeSql(this, e);
            }
        }

        #endregion //Event Firing Methods

        #region Abstract Methods

        public abstract void AddExtensionManagedEntities();

        public abstract void SubscribeToCrudEvents();

        #endregion //Abstract Methods

        #region Helper Methods

        public virtual bool IsEntityToBeManaged(object entity)
        {
            bool result = entity != null && _extensionManagedEntityCache.Exists(entity.GetType().FullName);
            return result;
        }

        public virtual bool IsEntityToBeManaged(string entityFullTypeName)
        {
            bool result = _extensionManagedEntityCache.Exists(entityFullTypeName);
            return result;
        }

        public virtual bool IsEntityOfType<E>(object entity)
        {
            bool result = entity.GetType().FullName == typeof(E).FullName;
            return result;
        }

        public virtual bool IsEntityOfType<E>(string entityFullTypeName)
        {
            bool result = entityFullTypeName == typeof(E).FullName;
            return result;
        }

        #endregion //Helper Methods

        #endregion //Methods
    }
}