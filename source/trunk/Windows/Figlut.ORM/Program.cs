namespace Figlut.ORM
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Utilities;

    #endregion //Using Directives

    class Program
    {
        #region Fields

        private static SqlDatabase db = null;

        #endregion //Fields

        #region Methods

        private static FiglutOrmArgs ParseArguments(string[] args)
        {
            FiglutOrmArgs result = new FiglutOrmArgs();
            if (args.Length < 1)
            {
                result.DisplayHelp = true;
                return result;
            }
            foreach (string a in args)
            {
                string aLower = a.ToLower();
                if(aLower == FiglutOrmArgs.HELP_ARGUMENT || aLower == FiglutOrmArgs.HELP_QUESTION_MARK_ARGUMENT)
                {
                    result.DisplayHelp = true;
                    return result;
                }
                string[] parameters = a.Split('|');
                if(parameters.Length != 2)
                {
                    throw new ArgumentException(string.Format("Invalid argument format for {0}.", a));
                }
                string pName = parameters[0];
                string pValue = parameters[1];
                switch (pName.ToLower())
                {
                    case FiglutOrmArgs.DATABASE_SERVER_NAME:
                        result.DatabaseServerName = pValue;
                        break;
                    case FiglutOrmArgs.DATABASE_NAME:
                        result.DatabaseName = pValue;
                        break;
                    case FiglutOrmArgs.DATABASE_USER_NAME:
                        result.DatabaseUserName = pValue;
                        break;
                    case FiglutOrmArgs.DATABASE_USER_PASSWORD:
                        result.DatabaseUserPassword = pValue;
                        break;
                    case FiglutOrmArgs.DATABASE_CONNECTION_STRING:
                        result.DatabaseConnectionString = pValue;
                        break;
                    case FiglutOrmArgs.OUTPUT_DIRECTORY:
                        result.OutputDirectory = pValue;
                        break;
                    default:
                        throw new ArgumentException(string.Format("Invalid argument {0}. Use help switch for valid arguments.", a));
                }
            }
            result.ValidateArguments();
            return result;
        }

        private static void DisplayHelp()
        {
            Console.WriteLine("*** Figlut ORM Help ***");
            Console.WriteLine();
            Console.WriteLine("{0} or {1} : Display usage.", FiglutOrmArgs.HELP_ARGUMENT, FiglutOrmArgs.HELP_QUESTION_MARK_ARGUMENT);
            Console.WriteLine("{0}|<database server instance name>", FiglutOrmArgs.DATABASE_SERVER_NAME);
            Console.WriteLine("{0}|<database name>", FiglutOrmArgs.DATABASE_NAME);
            Console.WriteLine("{0}|<database user name>", FiglutOrmArgs.DATABASE_USER_NAME);
            Console.WriteLine("{0}|<database user password>", FiglutOrmArgs.DATABASE_USER_PASSWORD);
            Console.WriteLine("{0}|<database connection string>", FiglutOrmArgs.DATABASE_CONNECTION_STRING);
            Console.WriteLine("{0}|<ORM DLL output directory>", FiglutOrmArgs.OUTPUT_DIRECTORY);
            Console.WriteLine();
            Console.WriteLine("N.B. Setting the connection string overrides any other database arguments specified.");
            Console.WriteLine();
            Console.WriteLine("e.g. (using SQL authentication):");
            Console.WriteLine();
            Console.WriteLine(@"""/dbserver|PAULKOLOZSV78C6\MSSQLSERVER2008"" ""/dbname|StockTaker"" ""/dbuser|sa"" ""/dbpassword|mypassword"" ""/outputdir|C:\temp""");
            Console.WriteLine();
            Console.WriteLine("e.g. (using windows authentication):");
            Console.WriteLine();
            Console.WriteLine(@"""/dbconnectionstring|Data Source=PAULKOLOZSV78C6\MSSQLSERVER2008;Initial Catalog=StockTaker;Integrated Security=True"" ""/outputdir|C:\temp""");
        }

        static void Main(string[] args)
        {
            try
            {
                FiglutOrmArgs arguments = ParseArguments(args);
                if (arguments.DisplayHelp)
                {
                    DisplayHelp();
                    return;
                }
                GenerateOrmAssembly(arguments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void GenerateOrmAssembly(FiglutOrmArgs arguments)
        {
            db = new SqlDatabase();
            db.OnDatabaseFeedback += delegate(object sender, Database.OnDatabaseFeedbackEventArgs e) { Console.WriteLine(e.FeedbackInfo); };
            db.Initialize(
                arguments.DatabaseConnectionString,
                true,
                true,
                true,
                arguments.OutputDirectory,
                true);
            string assemblyName = db.GetOrmAssembly().AssemblyFileName;
            if (!arguments.OutputDirectory.Equals(Environment.CurrentDirectory) && File.Exists(assemblyName))
            {
                File.Delete(assemblyName);
            }
            if (!File.Exists(db.GetOrmAssembly().AssemblyFilePath))
            {
                throw new FileNotFoundException(string.Format(
                    "ORM assembly created successfully, but could not be saved to {0}.",
                    db.GetOrmAssembly().AssemblyFilePath));
            }
            Console.WriteLine("ORM DLL created: {0}", db.GetOrmAssembly().AssemblyFilePath);
        }

        #endregion //Methods
    }
}
