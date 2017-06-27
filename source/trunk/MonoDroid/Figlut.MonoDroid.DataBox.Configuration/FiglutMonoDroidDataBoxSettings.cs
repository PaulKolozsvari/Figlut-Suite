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
using Figlut.MonoDroid.Toolkit.Utilities.SettingsFile;
using Figlut.MonoDroid.Toolkit.Utilities;
using Figlut.MonoDroid.Toolkit.Utilities.Serialization;
using Figlut.MonoDroid.Toolkit.Utilities.Logging;
using Figlut.MonoDroid.Toolkit;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox.Configuration
{
	public class FiglutMonoDroidDataBoxSettings : Settings
	{
		#region Constructors

		public FiglutMonoDroidDataBoxSettings()
		{
		}

		public FiglutMonoDroidDataBoxSettings(string filePath)
			: base(filePath)
		{
		}

		public FiglutMonoDroidDataBoxSettings(string name, string filePath)
			: base(name, filePath)
		{
		}

		#endregion //Constructors

		#region Fields

		#region Web Service

		private string _figlutWebServiceBaseUrl;
		private bool _useAuthentication;
		private bool _promptForCredentials;
		private string _authenticationDomainName;
		private string _authenticationUserName;
		private string _authenticationPassword;
		private int _figlutWebServiceWebRequestTimeout;
		private TextEncodingType _figlutWebServiceTextResponseEncoding;
		private SerializerType _figlutWebServiceMessagingFormat;
		private SerializerType _figlutWebServiceSchemaAcquisitionMessagingFormat;
		private bool _offlineMode;

		#endregion //Web Service

		#region DataBox

		private string _applicationTitle;
		private string _applicationVersion;
		private ColorName _themeColor;
		private string _applicationBannerImageFileName;
		private bool _shapeColumnNames;
		private int _primaryColumnIndex;
		private int _secondaryColumnIndex;
		private List<string> _hiddenTypeNames;
		private List<Type> _hiddenTypes;
		private string _entityIdColumnName;
		private bool _hideEntityIdColumn;
		private bool _treatZeroAsNull;
		private bool _closeAddWindowAfterAdd;
		private string _databaseSchemaFileName;
		private bool _useExtensions;
		private string _extensionAssemblyName;
		private string _crudInterceptorFullTypeName;
		private string _mainMenuExtensionFullTypeName;

		#endregion //DataBox

		#region Logging

		private bool _logToFile;
		private string _logFileName;
		private LoggingLevel _loggingLevel;

		#endregion //Logging

		#endregion //Fields

		#region Properties

		#region Web Service

		[SettingInfo("Web Service", DisplayName = "Base URL", Description = "The URL of the Figlut Web Service.", CategorySequenceId = 0)]
		public string FiglutWebServiceBaseUrl
		{
			get { return _figlutWebServiceBaseUrl; }
			set { _figlutWebServiceBaseUrl = value; }
		}

		[SettingInfo("Web Service", AutoFormatDisplayName = true, Description = "Whether or not tht Figlut Web Service requires clients to authenticate.", CategorySequenceId = 1)]
		public bool UseAuthentication
		{
			get { return _useAuthentication; }
			set { _useAuthentication = value; }
		}

		[SettingInfo("Web Service", AutoFormatDisplayName = true, Description = "Whether or not to prompt for credentials on application start up or otherwise use the credentials specified in these settings.", CategorySequenceId = 2)]
		public bool PromptForCredentials
		{
			get { return _promptForCredentials; }
			set { _promptForCredentials = value; }
		}

		[SettingInfo("Web Service", DisplayName = "Domain Name", Description = "The domain name (or hostname) to be used in the credentials when authenticating against the Figlut Web Service.", CategorySequenceId = 3)]
		public string AuthenticationDomainName
		{
			get { return _authenticationDomainName; }
			set { _authenticationDomainName = value; }
		}

		[SettingInfo("Web Service", DisplayName = "User Name", Description = "The windows user name to be used in the credentials when authenticating against the Figlut Web Service. N.B. Only used if the user is not prompted for credentials.", CategorySequenceId = 4)]
		public string AuthenticationUserName
		{
			get { return _authenticationUserName; }
			set { _authenticationUserName = value; }
		}

		[SettingInfo("Web Service", DisplayName = "Password", Description = "The password of the windows user to be used in the credentials when authentication against the Figlut Web Service.", CategorySequenceId = 5, PasswordChar = '*')]
		public string AuthenticationPassword
		{
			get { return _authenticationPassword; }
			set { _authenticationPassword = value; }
		}

		[SettingInfo("Web Service", DisplayName = "Web Request Timeout", Description = "The timeout in milliseconds of a web request made to the Figlut Web Service by the DataBox.", CategorySequenceId = 6)]
		public int FiglutWebServiceWebRequestTimeout
		{
			get { return _figlutWebServiceWebRequestTimeout; }
			set { _figlutWebServiceWebRequestTimeout = value; }
		}

		[SettingInfo("Web Service", DisplayName = "Text Response Encoding", Description = "The encoding of the text response from Figlut Web Service. The encoding of the Figlut DataBox and Web Service need to be configured to match.", CategorySequenceId = 7)]
		public TextEncodingType FiglutWebServiceTextResponseEncoding
		{
			get { return _figlutWebServiceTextResponseEncoding; }
			set { _figlutWebServiceTextResponseEncoding = value; }
		}

		[SettingInfo("Web Service", DisplayName = "Messaging Format", Description = "The format of the messages exchanged between the Figlut DataBox and the Web Service e.g. XML, JSON or CSV.", Visible = false, CategorySequenceId = 8)]
		public SerializerType FiglutWebServiceMessagingFormat
		{
			get { return _figlutWebServiceMessagingFormat; }
			set { _figlutWebServiceMessagingFormat = value; }
		}

		[SettingInfo("Web Service", DisplayName = "Schema Acquisition Messaging Format", Description = "The format of the messages exchanged between the Figlut DataBox and the Web Service when acquiring the database schema.", Visible = false, CategorySequenceId = 9)]
		public SerializerType FiglutWebServiceSchemaAcquisitionMessagingFormat
		{
			get { return _figlutWebServiceSchemaAcquisitionMessagingFormat; }
			set { _figlutWebServiceSchemaAcquisitionMessagingFormat = value; }
		}

		[SettingInfo("Web Service", AutoFormatDisplayName = true, Description = "Whether to download the database schema from the Figlut Web Service when DataBox starts up.", Visible = false, CategorySequenceId = 10)]
		public bool OfflineMode
		{
			get { return _offlineMode; }
			set { _offlineMode = value; }
		}

		#endregion //Web Service

		#region DataBox

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The name of the application. Leaving this setting blank defaults the title.", CategorySequenceId = 0)]
		public string ApplicationTitle
		{
			get { return _applicationTitle; }
			set { _applicationTitle = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The version of the application. Leaving this setting blank defaults the version.", CategorySequenceId = 1)]
		public string ApplicationVersion
		{
			get { return _applicationVersion; }
			set { _applicationVersion = value; ;}
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The theme color. Leaving this field blank defaults the color.", CategorySequenceId = 2)]
		public ColorName ThemeColor
		{
			get { return _themeColor; }
			set { _themeColor = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The image to be displayed in the application's banner.", SelectFilePath = true, CategorySequenceId = 3)]
		public string ApplicationBannerImageFilePath
		{
			get { return _applicationBannerImageFileName; }
			set { _applicationBannerImageFileName = value; ;}
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "Whether or not to rename the column names in the DataBox by adding spaces in between upper case letters for camel cased column names.", CategorySequenceId = 4)]
		public bool ShapeColumnNames
		{
			get { return _shapeColumnNames; }
			set { _shapeColumnNames = true; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The index of the main column value to display in the DataBox screen from where you select to see the details of a record.", CategorySequenceId = 5)]
		public int PrimaryColumnIndex
		{
			get{ return _primaryColumnIndex; }
			set{ _primaryColumnIndex = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName =  true, Description = "The index of the secondary column value to display in the DataBox from where you select to see the details of the a record.", CategorySequenceId = 6)]
		public int SecondaryColumnIndex
		{
			get{ return _secondaryColumnIndex; }
			set{ _secondaryColumnIndex = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The types of entity properties to hide from view in the DataBox.", CategorySequenceId = 7)]
		public List<string> HiddenTypeNames
		{
			get { return _hiddenTypeNames; }
			set
			{
				_hiddenTypeNames = value;
				RefreshHiddenTypes();
			}
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The name of the dynamically created column used to uniquely identify and lookup rows in the DataBox. This setting should not be changed unless the target database contains a table with a column name matching this setting's value i.e. because column names need to be unique.", CategorySequenceId = 8)]
		public string EntityIdColumnName
		{
			get { return _entityIdColumnName; }
			set { _entityIdColumnName = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "Whether or not to hide the dynamically created entity id column used to uniquely identify and look up rows in the DataBox. Under normal circumstances for presentation purposes this value should be set to 'true' i.e. in order to not display the entity ID.", CategorySequenceId = 9)]
		public bool HideEntityIdColumn
		{
			get { return _hideEntityIdColumn; }
			set { _hideEntityIdColumn = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "Whether to treat zeros as null. N.B. Setting this value to tru will prevent user from entering zero into not nullable numeric fields.", CategorySequenceId = 10)]
		public bool TreatZeroAsNull
		{
			get { return _treatZeroAsNull; }
			set { _treatZeroAsNull = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "Whether the add window should be closed after adding an entity.", CategorySequenceId = 11)]
		public bool CloseAddWindowAfterAdd
		{
			get { return _closeAddWindowAfterAdd; }
			set { _closeAddWindowAfterAdd = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The name of the file where the database schema info is saved to.", Visible = false, CategorySequenceId = 12)]
		public string DatabaseSchemaFileName
		{
			get { return _databaseSchemaFileName; }
			set { _databaseSchemaFileName = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "Whether or not the extension should be used. N.B. If it is to be used, then the ExtensionAssemblyName as well as the full type names of the extensions need to be configured.", Visible = false, CategorySequenceId = 13)]
		public bool UseExtensions
		{
			get { return _useExtensions; }
			set { _useExtensions = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The name of the assembly (DLL file) containing the Figlut Desktop DataBox extension e.g. FiglutExtension.dll. The assembly needs to be placed in the Figlut Desktop DataBox executing directory.", Visible = false, CategorySequenceId = 14)]
		public string ExtensionAssemblyName
		{
			get { return _extensionAssemblyName; }
			set { _extensionAssemblyName = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The full type name of the class acting as the CRUD interceptor e.g. FiglutExtension.MyDesktopDataBoxCrudInterceptor.", Visible = false, CategorySequenceId = 15)]
		public string CrudInterceptorFullTypeName
		{
			get { return _crudInterceptorFullTypeName; }
			set { _crudInterceptorFullTypeName = value; }
		}

		[SettingInfo("DataBox", AutoFormatDisplayName = true, Description = "The full type name of the class acting as the main menu extension e.g. FiglutExtension.MyDesktopDataBoxMainMenuExtension.", Visible = false, CategorySequenceId = 16)]
		public string MainMenuExtentionFullTypeName
		{
			get { return _mainMenuExtensionFullTypeName; }
			set { _mainMenuExtensionFullTypeName = value; }
		}

		#endregion //DataBox

		#region Logging

		[SettingInfo("Logging", DisplayName = "To File", Description = "Whether or not to log to a text log file in the executing directory.", CategorySequenceId = 0)]
		public bool LogToFile
		{
			get { return _logToFile; }
			set { _logToFile = value; }
		}

		[SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The name of the text log file to log to. The log file is written in the executing directory.", CategorySequenceId = 4)]
		public string LogFileName
		{
			get { return _logFileName; }
			set { _logFileName = value; }
		}

		[SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The extent of messages being logged: None = logging is disabled, Minimum = logs server start/stop and exceptions, Normal = logs additional information messages, Maximum = logs all requests and responses to the server.", CategorySequenceId = 5)]
		public LoggingLevel LoggingLevel
		{
			get { return _loggingLevel; }
			set { _loggingLevel = value; }
		}

		#endregion //Logging

		#endregion //Properties

		#region Methods

		public List<Type> GetHiddenTypes()
		{
			return this._hiddenTypes;
		}

		protected void RefreshHiddenTypes()
		{
			if (this._hiddenTypes == null)
			{
				this._hiddenTypes = new List<Type>();
			}
			this._hiddenTypes.Clear();
			foreach (string str in this._hiddenTypeNames)
			{
				Type item = Type.GetType(str, false);
				if (item == null)
				{
					throw new NullReferenceException(string.Format("Could not find type {0} to add as a hidden type. Ensure that it is a fully qualified type name.", str));
				}
				this._hiddenTypes.Add(item);
			}
		}

		public void ResetSettingsCategory(string categoryName)
		{
			if (categoryName == "Web Service") {
				ResetWebServiceSettingsCategory ();
			} else if (categoryName == "DataBox") {
				ResetDataBoxSettingsCategory ();
			} else if (categoryName == "Logging") {
				ResetLoggingSettingsCategory ();
			} else {
				throw new UserThrownException (
					string.Format ("No settings with the category name {0} was found to be reset.", categoryName),
					LoggingLevel.Minimum); 
			}
		}

		public void ResetAllSettings()
		{
			ResetWebServiceSettingsCategory ();
			ResetDataBoxSettingsCategory ();
			ResetLoggingSettingsCategory ();
		}

		private void ResetWebServiceSettingsCategory()
		{
			ShowMessageBoxOnException = true;
			FiglutWebServiceBaseUrl = "http://HostnameOrIpAddress:2983/Figlut/";
			UseAuthentication = false;
			PromptForCredentials = false;
			AuthenticationDomainName = string.Empty;
			AuthenticationUserName = string.Empty;
			AuthenticationPassword = string.Empty;
			FiglutWebServiceWebRequestTimeout = 3000;
			FiglutWebServiceTextResponseEncoding = TextEncodingType.UTF8;
			FiglutWebServiceMessagingFormat = SerializerType.JSON;
			FiglutWebServiceSchemaAcquisitionMessagingFormat = SerializerType.XML;
			OfflineMode = false;
		}

		public void ResetDataBoxSettingsCategory()
		{
			ApplicationTitle = "Figlut Android DataBox";
			ApplicationVersion = "1.0.0.0";
			ThemeColor = ColorName.SteelBlue;
			ApplicationBannerImageFilePath = null;
			ShapeColumnNames = true;
			PrimaryColumnIndex = 1;
			SecondaryColumnIndex = 2;
			HiddenTypeNames = new List<string> () {
				typeof(Guid).FullName,
				typeof(Nullable<Guid>).FullName,
				typeof(Byte[]).FullName
			};
			EntityIdColumnName = "ENTITY_ID_A2382C02EF06497A9BBFE4DB3C8290A8";
			HideEntityIdColumn = true;
			TreatZeroAsNull = true;
			CloseAddWindowAfterAdd = false;
			DatabaseSchemaFileName = "FiglutDatabaseSchema.json";
			UseExtensions = false;
			ExtensionAssemblyName = "Figlut.Extensions.dll";
			CrudInterceptorFullTypeName = "Figlut.Extensions.CrudInterceptor";
			MainMenuExtentionFullTypeName = "Figlut.Extensions.MainMenuExtension";
		}

		public void ResetLoggingSettingsCategory()
		{
			LogToFile = true;
			LogFileName = "Figlut.MonoDroid.DataBox.Log.txt";
			LoggingLevel = LoggingLevel.Maximum;
		}

		public void ResetDatabaseSchemaFileName()
		{
			DatabaseSchemaFileName = "FiglutDatabaseSchema.json";
		}

		#endregion //Methods
	}
}