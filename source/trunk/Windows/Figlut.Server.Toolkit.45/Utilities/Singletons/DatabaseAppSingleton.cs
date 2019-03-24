namespace Figlut.Server.Toolkit.Utilities.Singletons
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Data.Linq;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data.DB.LINQ;
    using Figlut.Server.Toolkit.Utilities.SettingsFile.Default;

    #endregion //Using Directives

    public class DatabaseAppSingleton<E> : AppSingleton<E> where E : class
    {
        #region Methods

        protected void InitializeAllDefaultSettings<D>(
            DatabaseAppSettings settings,
            Type userLinqToSqlType,
            Type serverActionLinqToSqlType,
            Type serverErrorLinqToSqlType,
            bool logSettings) where D : DataContext
        {
            base.InitializeAllDefaultSettings(settings, logSettings);
            InitializeDatabaseSettings<D>(settings, userLinqToSqlType, serverActionLinqToSqlType, serverErrorLinqToSqlType);
        }

        protected virtual void InitializeDatabaseSettings<D>(
            DatabaseAppSettings settings, 
            Type userLinqToSqlType,
            Type serverActionLinqToSqlType,
            Type serverErrorLinqToSqlType) where D : DataContext
        {
            LinqFunnelSettings linqFunnelSettings = new LinqFunnelSettings(settings.DatabaseConnectionString, settings.DatabaseCommandTimeout);
            GOC.Instance.AddByTypeName(linqFunnelSettings); //Adds an object to Global Object Cache with the key being the name of the type.
            string linqToSqlAssemblyFilePath = Path.Combine(Information.GetExecutingDirectory(), settings.LinqToSQLClassesAssemblyFileName);

            //Grab the LinqToSql context from the specified assembly and load it in the GOC to be used from anywhere in the application.
            //The point of doing this is that you can rebuild the context if the schema changes and reload without having to reinstall the web service. You just stop the service and overwrite the EOH.RainMaker.ORM.dll with the new one.
            //It also allows the Figlut Service Toolkit to be business data agnostic.
            GOC.Instance.LinqToClassesAssembly = Assembly.LoadFrom(linqToSqlAssemblyFilePath);
            GOC.Instance.LinqToSQLClassesNamespace = settings.LinqToSQLClassesNamespace;
            GOC.Instance.SetLinqToSqlDataContextType<D>();
            if (userLinqToSqlType != null)
            {
                GOC.Instance.UserLinqToSqlType = userLinqToSqlType;
            }
            if (serverActionLinqToSqlType != null)
            {
                GOC.Instance.ServerActionLinqToSqlType = serverActionLinqToSqlType;
            }
            if (serverErrorLinqToSqlType != null)
            {
                GOC.Instance.ServerErrorLinqToSqlType = serverErrorLinqToSqlType;
            }
            GOC.Instance.DatabaseTransactionScopeOption = settings.DatabaseTransactionScopeOption;
            GOC.Instance.DatabaseTransactionIsolationLevel = settings.DatabaseTransactionIsolationLevel;
            GOC.Instance.DatabaseTransactionTimeoutSeconds = settings.DatabaseTransactionTimeoutSeconds;
            GOC.Instance.DatabaseTransactionDeadlockRetryAttempts = settings.DatabaseTransactionDeadlockRetryAttempts;
            GOC.Instance.DatabaseTransactionDeadlockRetryWaitPeriod = settings.DatabaseTransactionDeadlockRetryWaitPeriod;
        }

        #endregion //Methods
    }
}
