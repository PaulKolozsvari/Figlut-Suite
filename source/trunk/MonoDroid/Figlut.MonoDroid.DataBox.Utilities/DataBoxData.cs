#region Using Directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Figlut.MonoDroid.Toolkit.Data;
using Figlut.MonoDroid.Toolkit.Data.DB.SQLServer;
using Figlut.MonoDroid.Toolkit.Utilities;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox.Utilities
{
	public class DataBoxData
	{
		#region Singleton Setup

		private static DataBoxData _instance;

		public static DataBoxData Instance
		{
			get 
			{
				if (_instance == null) 
				{
					_instance = new DataBoxData ();
				}
				return _instance;
			}
		}

		#endregion //Singleton Setup

		#region Constructors

		private DataBoxData()
		{
		}

		#endregion //Constructors

		#region Fields

		private FiglutEntityCacheUnique _currentEntityCache;
		private SqlDatabaseTable _currentTable;
		private List<string> _hiddenProperties;
		private Dictionary<string, View> _inputControls;
		private Nullable<Guid> _entityIdUnderUpdate;
		private object _entityUnderUpdate;
		private bool _inUpdateMode;
		private Nullable<Guid> _entityIdUnderDelete;
		private object _entityUnderDelete;
		private bool _inDeleteMode;
		private bool _unsavedChanges;
		private string _currentInputControlName;
		private bool _manageEntityChangesMade;

		#endregion Fields

		#region Properties

		public FiglutEntityCacheUnique CurrentEntityCache
		{
			get{ return _currentEntityCache; }
			set{ _currentEntityCache = value; }
		}

		public SqlDatabaseTable CurrentTable
		{
			get{ return _currentTable; }
			set{ _currentTable = value; }
		}

		public List<string> HiddenProperties
		{
			get{ return _hiddenProperties; }
			set{ _hiddenProperties = value; }
		}

		public Dictionary<string, View> InputControls
		{
			get{ return _inputControls; }
			set{ _inputControls = value; }
		}

		public Nullable<Guid> EntityIdUnderUpdate
		{
			get{ return _entityIdUnderUpdate; }
			set{ _entityIdUnderUpdate = value; }
		}

		public object EntityUnderUpdate
		{
			get{ return _entityUnderUpdate; }
			set{ _entityUnderUpdate = value; }
		}

		public bool InUpdateMode
		{
			get{ return _inUpdateMode; }
			set{ _inUpdateMode = value; }
		}

		public Nullable<Guid> EntityIdUnderDelete
		{
			get{ return _entityIdUnderDelete; }
			set{ _entityIdUnderDelete = value; }
		}

		public object EntityUnderDelete
		{
			get{ return _entityUnderDelete; }
			set{ _entityUnderDelete = value; }
		}

		public bool InDeleteMode
		{
			get{ return _inDeleteMode; }
			set{ _inDeleteMode = value; }
		}

		public bool UnsavedChanges
		{
			get{ return _unsavedChanges; }
			set{ _unsavedChanges = value; }
		}

		public string CurrentInputControlName
		{
			get{ return _currentInputControlName; }
			set{ _currentInputControlName = value; }
		}

		public bool ManageEntityChangesMade
		{
			get{ return _manageEntityChangesMade; }
			set{ _manageEntityChangesMade = value; }
		}

		#endregion //Properties

		#region Methods

		public void ClearData()
		{
			if (_currentEntityCache != null) 
			{
				_currentEntityCache.Clear ();
			}
			_currentEntityCache = null;
			_currentTable = null;
			if(_hiddenProperties != null)
			{
				_hiddenProperties.Clear ();
			}
			_hiddenProperties = null;
			if(_inputControls != null)
			{
				_inputControls.Clear ();
			}
			_inputControls = null;

			_entityIdUnderUpdate = null;
			_entityUnderUpdate = null;
			_inUpdateMode = false;

			_entityIdUnderDelete = null;
			_entityUnderDelete = null;
			_inDeleteMode = false;

			_manageEntityChangesMade = false;
		}

		public void ClearEntityUpdateMode()
		{
			UIHelper.ClearControls (InputControls);
			EntityUnderUpdate = null;
			EntityIdUnderUpdate = null;
			InUpdateMode = false;

			CurrentInputControlName = null;
			ManageEntityChangesMade = false;
		}

		public void ClearEntityDeleteMode()
		{
			EntityIdUnderDelete = null;
			EntityUnderDelete = null;
			InDeleteMode = false;
		}

		#endregion //Methods
	}
}