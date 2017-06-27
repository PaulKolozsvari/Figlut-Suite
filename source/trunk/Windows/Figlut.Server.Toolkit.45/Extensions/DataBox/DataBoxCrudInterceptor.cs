namespace Figlut.Server.Toolkit.Extensions.DataBox
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Extensions.DataBox.Events;
    using Figlut.Server.Toolkit.Extensions.DataBox.Events.Crud;
    using Figlut.Server.Toolkit.Extensions.DataBox;
    using Figlut.Server.Toolkit.Winforms;
    using Figlut.Server.Toolkit.Extensions.ExtensionManaged;
    using Figlut.Server.Toolkit.Extensions.DataBox.Managers;

    #endregion //Using Directives

    public abstract class DataBoxCrudInterceptor
    {
        #region Constructors

        public DataBoxCrudInterceptor()
        {
            _extensionManagedEntityCache = new ExtensionManagedEntityCache();
            AddExtensionManagedEntities();
            SubscribeToCrudEvents();
        }

        #endregion //Constructors

        #region Fields

        private ExtensionManagedEntityCache _extensionManagedEntityCache;

        private SqlDatabaseTable _currentTable;
        private CustomDataGridView _currentGrid;
        private bool _filtersEnabled;
        private List<string> _hiddenProperties;
        private List<Type> _hiddenTypes;
        private Dictionary<string, Control> _inputControls;
        private bool _inputControlLabelsIncluded;
        private Control _firstInputControl;

        protected DataBoxManager _dataBoxManager;

        #endregion //Fields

        #region Properties

        public ExtensionManagedEntityCache ExtensionManagedEntities
        {
            get { return _extensionManagedEntityCache; }
        }

        public SqlDatabaseTable CurrentTable
        {
            get { return _currentTable; }
        }

        public CustomDataGridView CurrentGrid
        {
            get { return _currentGrid; }
        }

        public bool FiltersEnabled
        {
            get { return _filtersEnabled; }
        }

        public List<string> HiddenProperties
        {
            get { return _hiddenProperties; }
        }

        public List<Type> HiddenTypes
        {
            get { return _hiddenTypes; }
        }

        public Dictionary<string, Control> InputControls
        {
            get { return _inputControls; }
        }

        public bool InputControlLabelsIncluded
        {
            get { return _inputControlLabelsIncluded; }
        }

        public Control FirstInputControl
        {
            get { return _firstInputControl; }
        }

        public DataBoxManager DataBoxManager
        {
            get { return _dataBoxManager; }
        }

        #endregion //Properties

        #region Events

        public event OnBeforeRefreshFromServer OnBeforeRefreshFromServer;
        public event OnAfterRefreshFromServer OnAfterRefreshFromServer;

        public event OnBeforeGridRefresh OnBeforeGridRefresh;
        public event OnAfterGridRefresh OnAfterGridRefresh;
        
        public event OnBeforeAddInputControls OnBeforeAddInputControls;
        public event OnAfterAddInputControls OnAfterAddInputControls;

        public event OnBeforeCrudOperation OnBeforeUpdate;
        public event OnAfterCrudOperation OnAfterUpdate;

        public event OnBeforeCrudOperation OnBeforeCancelUpdate;
        public event OnAfterCrudOperation OnAfterCancelUpdate;

        public event OnBeforeCrudOperation OnBeforePrepareForUpdate;
        public event OnAfterCrudOperation OnAfterPrepapreForUpdate;

        public event OnBeforeCrudOperation OnBeforeAdd;
        public event OnAfterCrudOperation OnAfterAdd;

        public event OnBeforeCrudOperation OnBeforeDelete;
        public event OnAfterCrudOperation OnAfterDelete;

        public event OnBeforeSave OnBeforeSave;
        public event OnAfterSave OnAfterSave;

        #endregion //Events

        #region Methods

        public void SetDataBoxManager(DataBoxManager dataBoxManager)
        {
            _dataBoxManager = dataBoxManager;
        }

        #region Event Firing Methods

        public void PerformOnBeforeRefreshFromServer(BeforeRefreshFromServerArgs e)
        {
            if (OnBeforeRefreshFromServer != null)
            {
                OnBeforeRefreshFromServer(this, e);
            }
        }

        public void PerformOnAfterRefreshFromServer(AfterRefreshFromServerArgs e)
        {
            if (OnAfterRefreshFromServer != null)
            {
                OnAfterRefreshFromServer(this, e);
            }
        }

        public void PerformOnBeforeGridRefresh(BeforeGridRefreshArgs e)
        {
            if (OnBeforeGridRefresh != null)
            {
                _currentGrid = e.CurrentGrid;
                _filtersEnabled = e.FiltersEnabled;
                OnBeforeGridRefresh(this, e);
            }
        }

        public void PerformOnAfterGridRefresh(AfterGridRefreshArgs e)
        {
            if (OnAfterGridRefresh != null)
            {
                _currentGrid = e.CurrentGrid;
                _filtersEnabled = e.FiltersEnabled;
                _hiddenProperties = e.HiddenProperties;
                _hiddenTypes = e.HiddenTypes;
                OnAfterGridRefresh(this, e);
            }
        }

        public void PerformOnBeforeAddInputControls(BeforeAddInputControlsArgs e)
        {
            if (OnBeforeAddInputControls != null)
            {
                _inputControls = e.InputControls;
                _inputControlLabelsIncluded = e.InputControlLabelsIncluded;
                OnBeforeAddInputControls(this, e);
            }
        }

        public void PerformOnAfterAddInputControls(AfterAddInputControlsArgs e)
        {
            if (OnAfterAddInputControls != null)
            {
                _inputControls = e.InputControls;
                _inputControlLabelsIncluded = e.InputControlLabelsIncluded;
                _firstInputControl = e.FirstInputControl;
                OnAfterAddInputControls(this, e);
            }
        }

        public void PerformOnBeforeUpdate(BeforeCrudOperationArgs e)
        {
            if (OnBeforeUpdate != null)
            {
                OnBeforeUpdate(this, e);
            }
        }

        public void PerformOnAfterUpdate(AfterCrudOperationArgs e)
        {
            if (OnAfterUpdate != null)
            {
                OnAfterUpdate(this, e);
            }
        }

        public void PerformOnBeforeCancelUpdate(BeforeCrudOperationArgs e)
        {
            if (OnBeforeCancelUpdate != null)
            {
                OnBeforeCancelUpdate(this, e);
            }
        }

        public void PerformOnAfterCancelUpdate(AfterCrudOperationArgs e)
        {
            if (OnAfterCancelUpdate != null)
            {
                OnAfterCancelUpdate(this, e);
            }
        }

        public void PerformOnBeforePrepareForUpdate(BeforeCrudOperationArgs e)
        {
            if (OnBeforePrepareForUpdate != null)
            {
                OnBeforePrepareForUpdate(this, e);
            }
        }

        public void PerformOnAfterPrepareForUpdate(AfterCrudOperationArgs e)
        {
            if (OnAfterPrepapreForUpdate != null)
            {
                OnAfterPrepapreForUpdate(this, e);
            }
        }

        public void PerformOnBeforeAdd(BeforeCrudOperationArgs e)
        {
            if (OnBeforeAdd != null)
            {
                OnBeforeAdd(this, e);
            }
        }

        public void PerformOnAfterAdd(AfterCrudOperationArgs e)
        {
            if (OnAfterAdd != null)
            {
                OnAfterAdd(this, e);
            }
        }

        public void PerformOnBeforeDelete(BeforeCrudOperationArgs e)
        {
            if (OnBeforeDelete != null)
            {
                OnBeforeDelete(this, e);
            }
        }

        public void PerformOnAfterDelete(AfterCrudOperationArgs e)
        {
            if (OnAfterDelete != null)
            {
                OnAfterDelete(this, e);
            }
        }

        public void PerformOnBeforeSave(BeforeSaveArgs e)
        {
            if (OnBeforeSave != null)
            {
                OnBeforeSave(this, e);
            }
        }

        public void PerformOnAfterSave(AfterSaveArgs e)
        {
            if (OnAfterSave != null)
            {
                OnAfterSave(this, e);
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

        public virtual bool IsEntityOfType<E>(object entity)
        {
            bool result = entity.GetType().FullName == typeof(E).FullName;
            return result;
        }

        #endregion //Helper Methods

        #endregion //Methods
    }
}