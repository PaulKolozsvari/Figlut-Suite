namespace Figlut.Server.Toolkit.Extensions.DataBox.Managers
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;

    #endregion //Using Directives

    public class DataBoxPropertiesChangedArgs : EventArgs
    {
        #region Constructors

        public DataBoxPropertiesChangedArgs(
            SqlDatabaseTable currentTable,
            FiglutEntityCacheUnique currentEntityCache,
            List<string> hiddenProperties,
            List<string> extensionManagedProperties,
            Dictionary<string, Control> inputControls,
            Control firstInputControl,
            object entityUnderUpdate,
            Nullable<Guid> entityIdUnderUpdate,
            string currentDataBoxFilePath,
            bool filtersEnabled,
            bool unsavedChanges,
            bool readOnlyMode,
            bool inUpdateMode)
        {
            _currentTable = currentTable;
            _currentEntityCache = currentEntityCache;
            _hiddenProperties = hiddenProperties;
            _extensionManagedProperties = extensionManagedProperties;
            _inputControls = inputControls;
            _firstInputControl = firstInputControl;
            _entityUnderUpdate = entityUnderUpdate;
            _entityIdUnderUpdate = entityIdUnderUpdate;
            _currentDataBoxFilePath = currentDataBoxFilePath;
            _filtersEnabled = filtersEnabled;
            _unsavedChanges = unsavedChanges;
            _readOnlyMode = readOnlyMode;
            _inUpdateMode = inUpdateMode;
        }

        #endregion //Constructors

        #region Fields

        private SqlDatabaseTable _currentTable;
        private FiglutEntityCacheUnique _currentEntityCache;
        private List<string> _hiddenProperties;
        private List<string> _extensionManagedProperties;
        private Dictionary<string, Control> _inputControls;
        private Control _firstInputControl;
        private object _entityUnderUpdate;
        private Nullable<Guid> _entityIdUnderUpdate;
        private string _currentDataBoxFilePath;
        private bool _filtersEnabled;
        private bool _unsavedChanges = false;
        private bool _readOnlyMode = false;
        private bool _inUpdateMode;

        #endregion //Fields

        #region Properties

        public SqlDatabaseTable CurrentTable { get { return _currentTable; } }

        public FiglutEntityCacheUnique CurrentEntityCache { get { return _currentEntityCache; } }

        public List<string> HiddenProperties { get { return _hiddenProperties; } }

        public List<string> ExtensionManagedProperties { get { return _extensionManagedProperties; } }

        public Dictionary<string, Control> InputControls { get { return _inputControls; } }

        public Control FirstInputControl { get { return _firstInputControl; } }

        public object EntityUnderUpdate { get { return _entityUnderUpdate; } }

        public Nullable<Guid> EntityIdUnderUpdate { get { return _entityIdUnderUpdate; } }

        public string CurrentDataBoxFilePath { get { return _currentDataBoxFilePath; } }

        public bool FiltersEnabled { get { return _filtersEnabled; } }

        public bool UnsavedChanges { get { return _unsavedChanges; } }

        public bool ReadOnlyMode { get { return _readOnlyMode; } }

        public bool InUpdateMode { get { return _inUpdateMode; } }

        #endregion //Properties
    }
}
