namespace Figlut.Server.Toolkit.Utilities.SettingsFile.Default
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class DatabaseAppSettings : AppSettings
    {
        #region Constructors

        public DatabaseAppSettings() : base()
        {
        }

        public DatabaseAppSettings(string filePath) : base(filePath)
        {
        }

        public DatabaseAppSettings(string name, string filePath) : base(name, filePath)
        {
        }

        #endregion //Constructors

        #region Properties

        #region Database

        /// <summary>
        /// The connection string to the server database.
        /// </summary>
        [SettingInfo("Database", AutoFormatDisplayName = true, Description = "The connection string to the server database.", CategorySequenceId = 0)]
        public string DatabaseConnectionString { get; set; }

        /// <summary>
        /// The timeout in milliseconds of the commands sent to the server database.
        /// </summary>
        [SettingInfo("Database", AutoFormatDisplayName = true, Description = "The timeout in milliseconds of the commands sent to the server database.", CategorySequenceId = 1)]
        public int DatabaseCommandTimeout { get; set; }

        /// <summary>
        /// The name of the assembly containing the Linq To SQL classes.
        /// </summary>
        [SettingInfo("Database", AutoFormatDisplayName = true, Description = "The name of the assembly containing the Linq To SQL classes.", CategorySequenceId = 2)]
        public string LinqToSQLClassesAssemblyFileName { get; set; }

        /// <summary>
        /// The namespace where the Linq To SQL classes are located in the assembly.
        /// </summary>
        [SettingInfo("Database", AutoFormatDisplayName = true, Description = "The namespace where the Linq To SQL classes are located in the assembly.", CategorySequenceId = 3)]
        public string LinqToSQLClassesNamespace { get; set; }

        #endregion //Database

        #endregion //Properties
    }
}
