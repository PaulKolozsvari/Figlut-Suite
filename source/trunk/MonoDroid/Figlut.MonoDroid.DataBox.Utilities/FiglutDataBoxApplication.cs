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
using Figlut.MonoDroid.Toolkit.Utilities;
using Figlut.MonoDroid.Toolkit.Web.Client;
using Figlut.MonoDroid.Toolkit.Utilities.Serialization;
using Figlut.MonoDroid.Toolkit.Web.Client.FiglutWebService;
using System.Net;
using Figlut.MonoDroid.DataBox.Configuration;
using Figlut.MonoDroid.Toolkit.Data.DB.SQLServer;
using System.IO;
using Figlut.MonoDroid.Toolkit.Data;
using Figlut.MonoDroid.Toolkit.Utilities.Logging;
using Figlut.MonoDroid.Toolkit;
using Android.Graphics;

#endregion //Using Directives

namespace Figlut.MonoDroid.DataBox
{
	public class FiglutDataBoxApplication
	{
		#region Singleton Setup

		private static FiglutDataBoxApplication _instance;

		public static FiglutDataBoxApplication Instance
		{
			get 
			{
				if (_instance == null) 
				{
					_instance = new FiglutDataBoxApplication ();
				}
				return _instance;
			}
		}

		#endregion Singleton Setup

		#region Constructors

		private FiglutDataBoxApplication()
		{
			ResetThemeColorToDefault ();
			ResetImageBannerFromSettings ();
		}

		#endregion //Constructors

		#region Fields

		private Android.Graphics.Color _themeColor;
		private Dictionary<string, Android.Graphics.Color> _systemColors;
		private Bitmap _imageBanner;

		#endregion Fields

		#region Properties

		public Android.Graphics.Color ThemeColor
		{
			get{ return _themeColor; }
		}

		public Dictionary<string, Android.Graphics.Color> SystemColors
		{
			get { 
				if (_systemColors == null || _systemColors.Count < 1) {
					_systemColors = Information.GetSystemColors ();
				}
				return _systemColors; 
			}
		}

		public Bitmap ImageBanner
		{
			get{ return _imageBanner; }
		}

		#endregion //Properties

		#region Constants

		private const string DEFAULT_FIGLUT_APPLICATION_BANNER_IMAGE_NAME = "FiglutApplicationBanner.png";
		private const string DEFAULT_THEME_COLOR_RGB = "#ff437e9f";

		public const int REQUEST_CODE_LOAD_DATA_BOX = 0;
		public const int REQUEST_CODE_MANAGE_ENTITY = 1;
		public const int REQUEST_CODE_CONFIGURATION_MANAGER = 2;
		public const int REQUEST_CODE_EDIT_SETTINGS = 3;
		public const int REQUEST_CODE_SELECT_FILE_PATH = 4;
		public const int REQUEST_CODE_LOGIN = 5;

		public const string EXTRA_SELECTED_DATA_BOX = "SelectedDataBox";
		public const string EXTRA_DATABOX_NAMES = "DataBoxNames";
		public const string EXTRA_ENTITY_OPERATION = "EntityOperation";
		public const string EXTRA_INPUT_CONTROL_NAME = "InputControlName";
		public const string EXTRA_INPUT_TYPE = "InputType";
		public const string EXTRA_REFRESH_DATA_BOX = "RefreshDataBox";
		public const string EXTRA_SETTINGS_CATEGORY_NAME = "SettingsCategoryName";
		public const string EXTRA_SETTINGS_NAME = "SettingName";
		public const string EXTRA_SELECTED_FILE_PATH = "SelectedFilePath";
		public const string EXTRA_SELECTED_FILE_NAME = "SelectedFileName";
		public const string EXTRA_LOGIN_INPUT_CONTROL = "LoginInputControl";
		public const string EXTRA_LOGIN_USER_NAME = "UserName";
		public const string EXTRA_LOGIN_PASSWORD = "Password";

		public const int DIALOG_INPUT_DATE = 0;
		public const int DIALOG_INPUT_TEXT = 1;
		public const int DIALOG_INPUT_SPINNER = 2;
		public const int DIALOG_INPUT_CHECK_BOX = 3;
		public const int DIALOG_INPUT_NUMBER_WHOLE = 4;

		public const int TEXT_SIZE_DIALOG_TITLE = 20;

		#endregion //Constants

		#region Methods

		public void Initialize(
			string authenticationUserName,
			string authenticationPassword,
			FiglutMonoDroidDataBoxSettings settings,
			bool wrapWebException)
		{
			GOC.Instance.ShowMessageBoxOnException = true;
			GOC.Instance.Logger = new Figlut.MonoDroid.Toolkit.Utilities.Logging.Logger (
				settings.LogToFile,
				settings.LoggingLevel,
				settings.LogFileName);
			GOC.Instance.SetEncoding (settings.FiglutWebServiceTextResponseEncoding);

			IMimeWebServiceClient webServiceClient = GetWebServiceClient (
				                                         settings.FiglutWebServiceMessagingFormat,
				                                         settings.FiglutWebServiceBaseUrl,
				                                         settings.UseAuthentication,
				                                         authenticationUserName,
				                                         authenticationPassword,
				                                         settings.AuthenticationDomainName);
			IMimeWebServiceClient webServiceClientSchema = GetWebServiceClient (
				                                               settings.FiglutWebServiceSchemaAcquisitionMessagingFormat,
				                                               settings.FiglutWebServiceBaseUrl,
				                                               settings.UseAuthentication,
				                                               authenticationUserName,
				                                               authenticationPassword,
				                                               settings.AuthenticationDomainName);

			GOC.Instance.FiglutWebServiceClient = new FiglutWebServiceClientWrapper (webServiceClient, settings.FiglutWebServiceWebRequestTimeout);
			GOC.Instance.FiglutWebServiceClientSchema = new FiglutWebServiceClientWrapper (webServiceClientSchema, settings.FiglutWebServiceWebRequestTimeout);

			GOC.Instance.FiglutWebServiceClient = new FiglutWebServiceClientWrapper(webServiceClient, settings.FiglutWebServiceWebRequestTimeout);
			GOC.Instance.FiglutWebServiceClientSchema = new FiglutWebServiceClientWrapper(webServiceClientSchema, settings.FiglutWebServiceWebRequestTimeout);
			if (string.IsNullOrEmpty (settings.DatabaseSchemaFileName)) {
				settings.ResetDatabaseSchemaFileName ();
				settings.SaveToFile ();
			}
			if (!settings.OfflineMode)
			{
				AcquireSchema(wrapWebException, System.IO.Path.Combine(Information.GetExecutingDirectory(), settings.DatabaseSchemaFileName));
			}
			else
			{
				ImportSchemaFromFile(System.IO.Path.Combine(Information.GetExecutingDirectory(), settings.DatabaseSchemaFileName));
			}
			RefreshThemeColorFromSettings ();
			RefreshImageBannerFromSettings ();
		}

		private IMimeWebServiceClient GetWebServiceClient(
			SerializerType serializerType,
			string figlutWebServiceBaseUrl,
			bool useAuthentication,
			string authenticationUserName,
			string authenticationPassword,
			string authenticationDomainName)
		{
			IMimeWebServiceClient result = null;
			WebServiceClient webServiceClient = null;
			switch (serializerType)
			{
				case SerializerType.XML:
				webServiceClient = new XmlWebServiceClient(figlutWebServiceBaseUrl);
				break;
				case SerializerType.JSON:
				webServiceClient = new JsonWebServiceClient(figlutWebServiceBaseUrl);
				break;
				case SerializerType.CSV:
				webServiceClient = new CsvWebServiceClient(figlutWebServiceBaseUrl);
				break;
				default:
				throw new ArgumentException(string.Format(
					"{0} not supported as a messaging format.",
					serializerType.ToString()));
			}
			if (useAuthentication)
			{
				webServiceClient.NetworkCredential = new NetworkCredential(
					authenticationUserName,
					authenticationPassword,
					authenticationDomainName);
			}
			result = (IMimeWebServiceClient)webServiceClient;
			return result;
		}

		public void AcquireSchema(bool wrapWebException, string databaseSchemaFilePath)
		{
			GOC.Instance.ClearDatabases ();
			SqlDatabase db = GOC.Instance.FiglutWebServiceClientSchema.GetSqlDatabase(
				true, 
				false, 
				Information.GetExecutingDirectory(), 
				wrapWebException);
			SqlDatabaseSchemaFile.ExportSchema(db, databaseSchemaFilePath);
			GOC.Instance.AddDatabase<SqlDatabase>(db);
		}

		public void ImportSchemaFromFile(string databaseSchemaFilePath)
		{
			SqlDatabase db = SqlDatabaseSchemaFile.ImportSchema(databaseSchemaFilePath, true, false, null);
			GOC.Instance.ClearDatabases();
			GOC.Instance.AddDatabase<SqlDatabase>(db);
		}

		public void RefreshThemeColorFromSettings()
		{
			FiglutMonoDroidDataBoxSettings settings = GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings> ();
			if (settings.ThemeColor == ColorName.SteelBlue) {
				ResetThemeColorToDefault ();
				return;
			}
			ResetThemeColorToDefault ();
			string colorNameLower = settings.ThemeColor.ToString().ToLower();
			foreach(string c in SystemColors.Keys)
			{
				if(c.ToLower() == colorNameLower)
				{
					_themeColor = SystemColors[c];
					return;
				}
			}
			StringBuilder colorsText = new StringBuilder ();
			foreach (string c in SystemColors.Keys) {
				colorsText.AppendFormat("{0},", c);
			}
			throw new UserThrownException (
				string.Format ("No system color with the name {0} exists. {1}", settings.ThemeColor, colorsText.ToString()), 
				LoggingLevel.Minimum);
		}

		public void ResetThemeColorToDefault()
		{
			_themeColor = Android.Graphics.Color.ParseColor (DEFAULT_THEME_COLOR_RGB);
		}

		public string RefreshImageBannerFromSettings()
		{
			string resultMessage = null;
			string imageFilePath = GOC.Instance.GetSettings<FiglutMonoDroidDataBoxSettings> ().ApplicationBannerImageFilePath;
			if (string.IsNullOrEmpty(imageFilePath) || 
				!File.Exists(imageFilePath)) { //Use the default one if either the path was not specidied or the path does not exist.
				ResetImageBannerFromSettings ();
				resultMessage = null;
			} else {
				if (_imageBanner != null) {
					_imageBanner.Recycle ();
				}
				_imageBanner = BitmapFactory.DecodeFile (imageFilePath);
				if (_imageBanner == null) {
					ResetThemeColorToDefault ();
					resultMessage = string.Format (
						"Could not load {0} as {1}. Reverted to default image.", 
						imageFilePath, 
						EntityReader<FiglutMonoDroidDataBoxSettings>.GetPropertyName (p => p.ApplicationBannerImageFilePath, false));
				}
			}
			return resultMessage;
		}

		public void ResetImageBannerFromSettings()
		{
			using (Stream imageStream = Application.Context.Assets.Open (DEFAULT_FIGLUT_APPLICATION_BANNER_IMAGE_NAME)) {
				_imageBanner = BitmapFactory.DecodeStream (imageStream);
				return;
			}
		}

		#endregion //Methods
	}
}