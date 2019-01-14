using Android.Content;
using System.Collections.Generic;

namespace Figlut.MonoDroid.Toolkit
{
	#region Using Directives

	using System;
	using Android.Database.Sqlite;

	#endregion //Using Directives

	/// <summary>
	/// Tutorial: http://www.codeproject.com/Articles/792883/Using-Sqlite-in-a-Xamarin-Android-Application-Deve
	/// Download URL: https://github.com/praeclarum/sqlite-net
	/// </summary>
	public class SqliteDataManagerHelper : Android.Database.Sqlite.SQLiteOpenHelper
	{
		#region Constructors

		public SqliteDataManagerHelper (Context context, string bdName, int version, Dictionary<string, string> createTableScripts) 
			: base(context, bdName, null, version)
		{
			if (string.IsNullOrEmpty (bdName)) 
			{
				throw new NullReferenceException (
					string.Format("databaseName may not be null or empty on {0}.",
						typeof(SqliteDataManagerHelper).Name));
			}
			if (context == null) 
			{
				throw new NullReferenceException (
					string.Format("context may not be null or empty on {0}.",
						typeof(SqliteDataManagerHelper).Name));
			}
			if (version < 1) 
			{
				throw new NullReferenceException (
					string.Format("version may not be less than 1 on {0}.",
						typeof(SqliteDataManagerHelper).Name));
			}
			if (createTableScripts == null) 
			{
				throw new NullReferenceException (
					string.Format("createTableScripts may not be null on {0}.",
						typeof(SqliteDataManagerHelper).Name));
			}
			_dbName = bdName;
			_context = context;
			_version = version;
			_createTableScripts = createTableScripts;
		}

		#endregion //Constructors

		#region Fields

		private string _dbName;
		private Context _context;
		private int _version;
		private Dictionary<string, string> _createTableScripts;

		#endregion //Fields

		public string DBName
		{
			get{ return _dbName; }
		}

		public Context Context
		{
			get{ return _context; }
		}

		public int Version
		{
			get{ return _version; }
		}

		#region Methods

		//Runs when the application is installed for the first time.
		public override void OnCreate (SQLiteDatabase db)
		{
			foreach (string script in _createTableScripts.Values) {
				db.ExecSQL (script);
			}
		}

		public override void OnUpgrade (SQLiteDatabase db, int oldVersion, int newVersion)
		{
			if (oldVersion < 2)
			{
				//perform any database upgrade tasks for versions prior to version 2.
			}
			if (oldVersion < 3)
			{
				//perform any database upgrade tasks for versions prior to version 3.
			}
		}

		#endregion //Methods
	}
}