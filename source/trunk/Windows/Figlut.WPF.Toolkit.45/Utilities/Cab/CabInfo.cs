namespace Figlut.Server.Toolkit.Utilities.Cab
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections;
    using System.IO;

    #endregion //Using Directives

    /// <summary>
    /// Contains information about a CAB file in order to create a INF file to be passed to cabwiz to build a cab.
    /// INF format: http://msdn.microsoft.com/en-us/library/aa924478.aspx
    /// Creating an INF File: http://msdn.microsoft.com/en-us/library/3h8ff753(v=vs.80).aspx
    /// </summary>
    public class CabInfo : IEnumerable<CabResourceInfo>
    {
        #region Constructors

        /// <summary>
        /// Contains information about a CAB file in order to create a INF file to be passed to cabwiz to build a cab.
        /// </summary>
        /// <param name="providerName">May not be passed as null or empty.</param>
        /// <param name="appName">May not be passed as null or empty.</param>
        /// <param name="manufacturer">May not be passed as null or empty.</param>
        /// <param name="versionMin">A null defaults the value to 4.0</param>
        /// <param name="versionMax">A null defaults the value to 6.99</param>
        public CabInfo(
            string provider,
            string appName,
            string manufacturer,
            Nullable<double> versionMin,
            Nullable<double> versionMax)
        {
            _provider = provider;
            _appName = appName;
            _manufacturer = manufacturer;
            _versionMin = versionMin.HasValue ? versionMin.Value : DEFAULT_VERSION_MIN;
            _versionMax = versionMax.HasValue ? versionMax.Value : DEFAULT_VERSION_MAX;
            _cabResources = new Dictionary<string, CabResourceInfo>();
        }

        /// <summary>
        /// Contains information about a CAB file in order to create a INF file to be passed to cabwiz to build a cab.
        /// </summary>
        public CabInfo()
        {
            _cabResources = new Dictionary<string, CabResourceInfo>();
        }

        #endregion //Constructors

        #region Constants

        private const double DEFAULT_VERSION_MIN = 4.0;
        private const double DEFAULT_VERSION_MAX = 6.99;

        #endregion //Constants

        #region Fields

        private string _provider;
        private string _appName;
        private string _manufacturer;
        private double _versionMin;
        private double _versionMax;
        private string _ceSetupDll;
        private Dictionary<string, CabResourceInfo> _cabResources;

        #endregion //Fields

        #region Properties

        public string Provider
        {
            get { return _provider; }
            set { _provider = value; }
        }

        public string AppName
        {
            get { return _appName; }
            set { _appName = value; }
        }

        public string Manufacturer
        {
            get { return _manufacturer; }
            set { _manufacturer = value; }
        }

        public double VersionMin
        {
            get { return _versionMin; }
            set { _versionMin = value; }
        }

        public double VersionMax
        {
            get { return _versionMax; }
            set { _versionMax = value; }
        }

        public int CabResourcesCount
        {
            get { return _cabResources.Count; }
        }

        #endregion //Properties

        #region Indexers

        /// <summary>
        /// Get a CAB resource file with the specified file name (not the full path).
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public CabResourceInfo this[string fileName]
        {
            get
            {
                return _cabResources.ContainsKey(fileName) ? _cabResources[fileName] : null;
            }
        }

        #endregion //Indexers

        #region Methods

        /// <summary>
        /// Set the file name (not the path) of the CESetupDLL for the CAB file. The resource file must have already been added to this CAB
        /// and must have a dll file extension.
        /// </summary>
        /// <param name="ceSetupDll"></param>
        public void SetCESetupDll(string ceSetupDllFileName)
        {
            if (!_cabResources.ContainsKey(ceSetupDllFileName))
            {
                throw new ArgumentException(string.Format(
                    "A resource with the file name of {0} has not been added to the CAB yet, therefore it cannot be set as the CESetupDLL.",
                    ceSetupDllFileName));
            }
            if (Path.GetExtension(ceSetupDllFileName).ToLower() != ".dll")
            {
                throw new ArgumentException("CESetupDLL must be a dll file. {0} is not a dll.", ceSetupDllFileName);
            }
            _ceSetupDll = ceSetupDllFileName;
        }

        /// <summary>
        /// Pass in just the file name (not the path) to determine if the resource 
        /// file has already been added to be embedded into this CAB file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public bool ContainsCabResource(string fileName)
        {
            return _cabResources.ContainsKey(fileName);
        }

        /// <summary>
        /// Pass in CabResource to determine if the resource file has already been added to be embedded into this CAB file.
        /// The match is done against the file name (not th path).
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        public bool ContainsCabResource(CabResourceInfo resource)
        {
            return _cabResources.ContainsKey(resource.FileName);
        }

        /// <summary>
        /// Adds a file as a resource to be embedded in the CAB file. The file name (not the path) has to be unique to this CAB file.
        /// </summary>
        /// <param name="resource"></param>
        public CabResourceInfo AddCabResource(string filePath)
        {
            CabResourceInfo resource = new CabResourceInfo(filePath, _cabResources.Count + 1, true);
            if (ContainsCabResource(resource))
            {
                throw new ArgumentException(string.Format(
                    "Resource file {0} already added to this CAB file. Resource file names have to be unique to this CAB file.", 
                    resource.FileName));
            }
            _cabResources.Add(resource.FileName, resource);
            return resource;
        }

        /// <summary>
        /// Removes a resource file from this CAB file.
        /// </summary>
        /// <param name="resource"></param>
        public void RemoveCabResource(CabResourceInfo resource)
        {
            if (!ContainsCabResource(resource))
            {
                throw new ArgumentException(string.Format(
                    "Resource file {0} has not been addded to this CAB file to be removed.",
                    resource.FileName));
            }
            _cabResources.Remove(resource.FileName);
            RefreshCabResourceIndexes();
        }

        private void RefreshCabResourceIndexes()
        {
            int i = 1;
            foreach (CabResourceInfo resource in (from r in this
                                                  orderby r.Index ascending
                                                  select r))
            {
                resource.Index = i;
                i++;
            }
        }

        /// <summary>
        /// Validates that all the files resources added to the CAB actually exist.
        /// </summary>
        public void ValidateCabResourcesExist()
        {
            foreach (CabResourceInfo resource in this)
            {
                FileSystemHelper.ValidateFileExists(resource.AbsoluteFilePath);
            }
        }

        public IEnumerator<CabResourceInfo> GetEnumerator()
        {
            return _cabResources.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region INF

        /// <summary>
        /// INF Order: 1
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFVersionBlock(StringBuilder INF)
        {
            INF.AppendLine("[Version]");
            INF.AppendLine("Signature=\"$Windows NT$\"");
            INF.AppendLine(string.Format("Provider=\"{0}\"", _provider));
            INF.AppendLine("CESignature=\"$Windows CE$\"");
            INF.AppendLine();
        }

        /// <summary>
        /// INF Order: 2
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFCEStringBlock(StringBuilder INF)
        {
            INF.AppendLine("[CEStrings]");
            INF.AppendLine(string.Format("AppName=\"{0}\"", _appName));
            INF.AppendLine(@"InstallDir=%CE1%\%AppName%");
            INF.AppendLine();
        }

        /// <summary>
        /// INF Order: 3
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFStringsBlock(StringBuilder INF)
        {
            INF.AppendLine("[Strings]");
            INF.AppendLine(string.Format("Manufacturer=\"{0}\"", _manufacturer));
            INF.AppendLine();
        }

        /// <summary>
        /// INF Order: 4
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFCEDeviceBlock(StringBuilder INF)
        {
            INF.AppendLine("[CEDevice]");
            INF.AppendLine(string.Format("VersionMin={0}", Math.Round(_versionMin, 2).ToString()));
            INF.AppendLine(string.Format("VersionMax={0}", Math.Round(_versionMax, 2).ToString()));
            INF.AppendLine("BuildMax=0xE0000000");
            INF.AppendLine();
        }

        /// <summary>
        /// INF Order: 5
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFDefaultInstallBlock(StringBuilder INF)
        {
            INF.AppendLine("[DefaultInstall]");
            INF.AppendLine("CEShortcuts=Shortcuts");
            INF.AppendLine("AddReg=RegKeys");
            AppendINFCopyFilesDeclarationBlock(INF);
            if (!string.IsNullOrEmpty(_ceSetupDll))
            {
                INF.AppendLine(string.Format("CESetupDLL=\"{0}\"", _ceSetupDll));
            }
            INF.AppendLine();
        }

        private void AppendINFCopyFilesDeclarationBlock(StringBuilder INF)
        {
            INF.Append("CopyFiles=");
            foreach(CabResourceInfo resource in (from r in this
                                                 orderby r.Index ascending
                                                 select r))
            {
                INF.Append(resource.GetINFCopyFileOperationName());
                if(resource.Index < CabResourcesCount)
                {
                    INF.Append(",");
                }
                else if(resource.Index > CabResourcesCount)
                {
                    throw new IndexOutOfRangeException(string.Format(
                        "There are {0} resource files, but resource {1} has an index of {2} i.e. over the number of resource files.",
                        CabResourcesCount,
                        resource.FileName,
                        resource.Index));
                }
                else
                {
                    /*Do nothing*/
                }
            }
            INF.AppendLine();
        }

        /// <summary>
        /// INF Order: 7
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFSourceDisksNamesBlock(StringBuilder INF)
        {
            INF.AppendLine("[SourceDisksNames]");
            (from r in this
             orderby r.Index ascending
             select r).ToList().ForEach(p => p.AppendINFSourceDiskNameBlock(INF));//Directories
            INF.AppendLine();
        }

        /// <summary>
        /// INF Order: 8
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFSourceDisksFilesBlock(StringBuilder INF)
        {
            INF.AppendLine("[SourceDisksFiles]");
            (from r in this
             orderby r.Index ascending
             select r).ToList().ForEach(p => p.AppendINFSourceDiskFileBlock(INF));
            INF.AppendLine();
        }

        /// <summary>
        /// INF Order: 9
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFDestinationDirsBlock(StringBuilder INF)
        {
            INF.AppendLine("[DestinationDirs]");
            INF.AppendLine(@"Shortcuts=0,%CE2%\Start Menu");
            (from r in this
             orderby r.Index ascending
             select r).ToList().ForEach(p => p.AppendINFDestinationDirBlock(INF));
            INF.AppendLine();
        }

        /// <summary>
        /// INF Order: 10
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFOperationsBlock(StringBuilder INF)
        {
            (from r in this
             orderby r.Index ascending
             select r).ToList().ForEach(p => p.AppendINFCopyFileOperationBlock(INF));
            INF.AppendLine();
        }

        /// <summary>
        /// INF Order: 11
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFShortcuts(StringBuilder INF)
        {
            INF.AppendLine("[Shortcuts]");
            INF.AppendLine();
        }

        /// <summary>
        /// INF Order: 12
        /// </summary>
        /// <param name="INF"></param>
        private void AppendINFRegKeys(StringBuilder INF)
        {
            INF.AppendLine("[RegKeys]");
            INF.AppendLine();
        }

        #endregion //INF

        public string GetINFFileContents()
        {
            if (string.IsNullOrEmpty(_provider))
            {
                throw new NullReferenceException("Provider not specified.");
            }
            if (string.IsNullOrEmpty(_appName))
            {
                throw new NullReferenceException("Application Name not specified.");
            }
            if (string.IsNullOrEmpty(_manufacturer))
            {
                throw new NullReferenceException("Manufacturer not specified.");
            }
            StringBuilder result = new StringBuilder();
            AppendINFVersionBlock(result);//1
            AppendINFCEStringBlock(result);//2
            AppendINFStringsBlock(result);//3
            AppendINFCEDeviceBlock(result);//4
            AppendINFDefaultInstallBlock(result);//5
            AppendINFSourceDisksNamesBlock(result);//6
            AppendINFSourceDisksFilesBlock(result);//7
            AppendINFDestinationDirsBlock(result);//8
            AppendINFOperationsBlock(result);//9
            AppendINFShortcuts(result);//10
            AppendINFRegKeys(result);//11
            return result.ToString();
        }

        public override string ToString()
        {
            return GetINFFileContents();
        }

        public void SaveINFFile(string filePath)
        {
            if (Path.GetExtension(filePath).ToLower() != ".inf")
            {
                throw new ArgumentException(string.Format("{0} must have an INF extension.", Path.GetFileName(filePath)));
            }
            File.WriteAllText(filePath, GetINFFileContents(), Encoding.UTF8);
        }

        #endregion //Methods
    }
}