namespace Figlut.Server.Toolkit.Data.ORM
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection.Emit;
    using System.Reflection;
    using System.IO;
    using System.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using System.Data.SqlClient;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;

    #endregion //Using Directives

    /// <summary>
    /// http://olondono.blogspot.com/2008/02/creating-code-at-runtime.html
    /// Collectible Assemblies for Dynamic Type Generation : http://msdn.microsoft.com/en-us/library/dd554932.aspx#restrictions
    /// </summary>
    public class OrmAssembly
    {
        #region Constructors

        public OrmAssembly(string assemblyName, AssemblyBuilderAccess assemblyBuilderAccess)
        {
            Initialize(assemblyName, null, assemblyBuilderAccess);
        }

        public OrmAssembly(string assemblyName, string assemblyFileName, AssemblyBuilderAccess assemblyBuilderAccess)
        {
            Initialize(assemblyName, assemblyFileName, assemblyBuilderAccess);
        }

        #endregion //Constructors

        #region Fields

        protected string _assemblyName;
        protected string _assemblyFileName;
        protected string _assemblyFilePath;
        protected AssemblyBuilderAccess _assemblyBuilderAccess;
        protected EntityCache<string, OrmType> _ormTypes;
        protected AssemblyBuilder _assemblyBuilder;
        protected ModuleBuilder _moduleBuilder;

        #endregion //Fields

        #region Properties

        public string AssemblyName
        {
            get { return _assemblyName; }
        }

        public string AssemblyFileName
        {
            get { return _assemblyFileName; }
        }

        public string AssemblyFilePath
        {
            get { return _assemblyFilePath; }
        }

        public AssemblyBuilderAccess AssemblyBuilderAccess
        {
            get { return _assemblyBuilderAccess; }
        }

        public AssemblyBuilder AssemblyBuilder
        {
            get { return _assemblyBuilder; }
        }

        public ModuleBuilder ModuleBuilder
        {
            get { return _moduleBuilder; }
        }

        public EntityCache<string, OrmType> OrmTypes
        {
            get { return _ormTypes; }
        }

        #endregion //Properties

        #region Methods

        public void Clean()
        {
            _ormTypes.ToList().ForEach(p => p.Clean());
            _ormTypes.Clear();
            _ormTypes = null;
            _moduleBuilder = null;
            _assemblyBuilder = null;
        }

        protected void Initialize(
            string assemblyName, 
            string assemblyFileName, 
            AssemblyBuilderAccess assemblyBuilderAccess)
        {
            if (assemblyName != null)
            {
                assemblyName = assemblyName.Trim();
            }
            if (string.IsNullOrEmpty(assemblyName))
            {
                throw new NullReferenceException(string.Format(
                    "{0} may not be null when constructing a {1}.",
                    EntityReader<OrmAssembly>.GetPropertyName(p => p.AssemblyName, false),
                    this.GetType().FullName));
            }
            _assemblyName = assemblyName;
            _assemblyFileName = assemblyFileName;
            _assemblyBuilderAccess = assemblyBuilderAccess;
            _assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(_assemblyName), _assemblyBuilderAccess);
            string domainName = AppDomain.CurrentDomain.FriendlyName;
            if (string.IsNullOrEmpty(_assemblyFileName))
            {
                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assemblyName);
            }
            else
            {
                _moduleBuilder = _assemblyBuilder.DefineDynamicModule(_assemblyName, _assemblyFileName);
            }
            _ormTypes = new EntityCache<string, OrmType>();
        }

        public OrmType CreateOrmType(string typeName, bool prefixWithAssemblyNamespace)
        {
            if (_ormTypes.Exists(typeName))
            {
                throw new ArgumentException(string.Format(
                    "{0} with {1} {2} already created on {3}.",
                    typeof(OrmType).FullName,
                    EntityReader<OrmType>.GetPropertyName(p => p.TypeName, false),
                    typeName,
                    this.GetType().FullName));
            }
            if (prefixWithAssemblyNamespace)
            {
                typeName = string.Format("{0}.{1}", _assemblyName, typeName);
            }
            TypeBuilder typeBuilder = _moduleBuilder.DefineType(typeName, TypeAttributes.Class | TypeAttributes.Public);
            ConstructorBuilder constructorBuilder = typeBuilder.DefineDefaultConstructor(MethodAttributes.Public);
            OrmType result = new OrmType(typeName, typeBuilder);
            _ormTypes.Add(result.TypeName, result);
            return result;
        }

        /// <summary>
        /// Gets the default assembly file path which is the assembly file name in the current executing directory.
        /// </summary>
        /// <returns></returns>
        private string GetAssemblyFilePathToExecutingDirectory()
        {
            return Path.Combine(Information.GetExecutingDirectory(), _assemblyFileName);
        }

        private void DeleteAssemblyFromCurrentDirectory()
        {
            if (File.Exists(_assemblyFileName))
            {
                File.Delete(_assemblyFileName);
            }
        }

        /// <summary>
        /// We can only save the assembly in the current directory when we call save i.e. we're not allowed to set a path.
        /// However, the current directory may not always be equal to current directory. 
        /// If that's the case we need to save it to the current directory, then copy it to the executing directory and thereafter delete it from the current directory.
        /// </summary>
        private void SaveOrmAssemblyToExecutingDirectory()
        {
            DeleteAssemblyFromCurrentDirectory(); //Delete the old assembly from the current directory.
            _assemblyBuilder.Save(_assemblyFileName); //Saves it to the current directory.
            if (Environment.CurrentDirectory == Information.GetExecutingDirectory())
            {
                return; //If the current and executing directories are the same, then we can leave the assembly where it is.
            }
            string executingDirectoryAssemblyFilePath = GetAssemblyFilePathToExecutingDirectory(); //Path to the executing directory where we need to actually save it to.
            if (File.Exists(executingDirectoryAssemblyFilePath))
            {
                File.Delete(executingDirectoryAssemblyFilePath); //Delete the old assembly from the executing directory.
            }
            File.Copy(_assemblyFileName, executingDirectoryAssemblyFilePath); //Copy from the current directory to the executing directory.
            DeleteAssemblyFromCurrentDirectory(); //Delete the assembly from the current directory.
            _assemblyFilePath = executingDirectoryAssemblyFilePath;
        }

        /// <summary>
        /// Saves the assembly in the executing directory and then copies it to the specified output directory if the output directory is different from the executing directory.
        /// Throws an exception of the specified output directory does not exist.
        /// </summary>
        public void Save(string ormAssemblyOutputDirectory)
        {
            SaveOrmAssemblyToExecutingDirectory();
            if (string.IsNullOrEmpty(ormAssemblyOutputDirectory) || (ormAssemblyOutputDirectory.ToLower().Trim() == Information.GetExecutingDirectory().ToLower().Trim()))
            {
                return; //Output directory not specified or the specified output directory is in fact the executing directory.
            }
            if (!Directory.Exists(ormAssemblyOutputDirectory))
            {
                throw new UserThrownException(
                    string.Format("Could not find ORM output directory {0}.", ormAssemblyOutputDirectory),
                    LoggingLevel.Minimum);
            }
            string newOutputAssemblyFilePath = Path.Combine(ormAssemblyOutputDirectory, _assemblyFileName);
            if (File.Exists(newOutputAssemblyFilePath))
            {
                File.Delete(newOutputAssemblyFilePath); //Delete the old assembly from the output directory.
            }
            File.Copy(_assemblyFilePath, newOutputAssemblyFilePath); //Copy from the executing directory to the output directory specified by the user in settings file.
            _assemblyFilePath = newOutputAssemblyFilePath;
        }

        /// <summary>
        /// Gets the assembly file name.
        /// </summary>
        public override string ToString()
        {
            return _assemblyName;
        }

        #endregion //Methods

        #region Destructors

        //~OrmAssembly()
        //{
        //    int test = 0;
        //}

        #endregion //Destructors
    }
}