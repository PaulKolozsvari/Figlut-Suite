namespace Figlut.ORM
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;

    #endregion //Using Directives

    public class FiglutOrmArgs
    {
        #region Constructors

        public FiglutOrmArgs()
        {
        }

        public FiglutOrmArgs(
            string databaseServerName,
            string databaseName,
            string databaseUserName,
            string databaseUserPassword)
        {
            _databaseServerName = databaseServerName;
            _databaseName = databaseName;
            _databaseUserName = databaseName;
            _databaseUserPassword = databaseUserPassword;
            RefreshDatabaseConnectionString();
        }

        public FiglutOrmArgs(string databaseConnectionString)
        {
            _databaseConnectionString = databaseConnectionString;
        }

        #endregion //Constructors

        #region Constants

        public const string HELP_ARGUMENT = "/help";
        public const string HELP_QUESTION_MARK_ARGUMENT = "/?";
        public const string DATABASE_SERVER_NAME = "/dbserver";
        public const string DATABASE_NAME = "/dbname";
        public const string DATABASE_USER_NAME = "/dbuser";
        public const string DATABASE_USER_PASSWORD = "/dbpassword";
        public const string DATABASE_CONNECTION_STRING = "/dbconnectionstring";
        public const string OUTPUT_DIRECTORY = "/outputdir";

        #endregion //Constants

        #region Fields

        private bool _displayHelp;
        private string _databaseServerName;
        private string _databaseName;
        private string _databaseUserName;
        private string _databaseUserPassword;
        private string _databaseConnectionString;
        private string _outputDirectory;

        #endregion //Fields

        #region Properties

        public bool DisplayHelp
        {
            get { return _displayHelp; }
            set { _displayHelp = value; }
        }

        public string DatabaseServerName
        {
            get { return _databaseServerName; }
            set { _databaseServerName = value; }
        }

        public string DatabaseName
        {
            get { return _databaseName; }
            set { _databaseName = value; }
        }

        public string DatabaseUserName
        {
            get { return _databaseUserName; }
            set { _databaseUserName = value; }
        }

        public string DatabaseUserPassword
        {
            get { return _databaseUserPassword; }
            set { _databaseUserPassword = value; }
        }

        public string DatabaseConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_databaseConnectionString))
                {
                    RefreshDatabaseConnectionString();
                }
                return _databaseConnectionString;
            }
            set { _databaseConnectionString = value; }
        }

        public string OutputDirectory
        {
            get
            {
                if (string.IsNullOrEmpty(_outputDirectory))
                {
                    _outputDirectory = Information.GetExecutingDirectory();
                }
                return _outputDirectory;
            }
            set { _outputDirectory = value; }
        }

        #endregion //Properties

        #region Methods

        public void RefreshDatabaseConnectionString()
        {
            _databaseConnectionString = string.Format(
                "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}",
                _databaseServerName,
                _databaseName,
                _databaseUserName,
                _databaseUserPassword);
        }

        public void ValidateArguments()
        {
            if (!string.IsNullOrEmpty(_databaseConnectionString))
            {
                return;
            }
            if (string.IsNullOrEmpty(_databaseServerName))
            {
                throw new NullReferenceException(string.Format("{0} not set.", DATABASE_SERVER_NAME));
            }
            if(string.IsNullOrEmpty(_databaseName))
            {
                throw new NullReferenceException(string.Format("{0} not set.", DATABASE_NAME));
            }
            if (string.IsNullOrEmpty(_databaseUserName))
            {
                throw new NullReferenceException(string.Format("{0} not set.", DATABASE_USER_NAME));
            }
            if (string.IsNullOrEmpty(_databaseUserPassword))
            {
                throw new NullReferenceException(string.Format("{0} not set.", DATABASE_USER_PASSWORD));
            }
            if (!Directory.Exists(OutputDirectory))
            {
                throw new DirectoryNotFoundException(string.Format("Could not find {0} {1}", OUTPUT_DIRECTORY, OutputDirectory));
            }
        }

        #endregion //Methods
    }
}