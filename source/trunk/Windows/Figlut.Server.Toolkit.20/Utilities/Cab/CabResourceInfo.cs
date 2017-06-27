namespace Figlut.Server.Toolkit.Utilities.Cab
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    #endregion //Using Directives

    public class CabResourceInfo
    {
        #region Constructors

        public CabResourceInfo(string filePath, int index, bool validateFileExists)
        {
            SetAbsoluteFilePath(filePath);
            _index = index;
            if (validateFileExists)
            {
                ValidateFileExists();
            }
        }

        #endregion //Constructors

        #region Fields

        private string _absolutefilePath;
        private string _fileName;
        private string _directoryPath;
        private int _index;

        #endregion //Fields

        #region Properties

        public string AbsoluteFilePath
        {
            get { return _absolutefilePath; }
            set { SetAbsoluteFilePath(value); }
        }

        public string FileName
        {
            get { return _fileName; }
        }

        public string DirectoryPath
        {
            get { return _directoryPath; }
        }

        public int Index
        {
            get { return _index; }
            internal set { _index = value; }
        }
    
        #endregion //Properties

        #region Methods

        private void SetAbsoluteFilePath(string filePath)
        {
            _absolutefilePath = Path.IsPathRooted(filePath) ? filePath : Path.GetFullPath(filePath);
            _fileName = Path.GetFileName(_absolutefilePath);
            _directoryPath = Path.GetDirectoryName(_absolutefilePath);
        }

        public void ValidateFileExists()
        {
            FileSystemHelper.ValidateFileExists(_absolutefilePath);
        }

        #region INF

        /// <summary>
        /// Gets just the name of the operation in the INF file responsible 
        /// for copying the file to the correct location on the device.
        /// e.g. (where 1 is the index of the file)
        /// 
        /// Files.Common1
        /// </summary>
        /// <returns></returns>
        public string GetINFCopyFileOperationName()
        {
            return string.Format("Files.Common{0}", _index);
        }

        /// <summary>
        /// Gets the directory path formatted as needed to be listed the INF file.
        /// e.g. (where 1 is the index and C:\Program Files\Microsoft.NET\SDK\CompactFramework\v3.5\WindowsCE\ is the directory of the file)
        /// 
        /// 1=,"Common1",,"C:\Program Files\Microsoft.NET\SDK\CompactFramework\v3.5\WindowsCE\"
        /// </summary>
        /// <returns></returns>
        public void AppendINFSourceDiskNameBlock(StringBuilder INF)
        {
            INF.AppendLine(string.Format("{0}=,\"Common{0}\",,\"{1}\"", _index, _directoryPath));
        }

        /// <summary>
        /// Gets the file name formatted as needed to be listed in the INF file.
        /// e.g. (where 1 is the index and NETCFv35.wm.armv4i.cab is the file name.
        /// 
        /// "NETCFv35.wm.armv4i.cab"=1
        /// </summary>
        /// <returns></returns>
        public void AppendINFSourceDiskFileBlock(StringBuilder INF)
        {
            INF.AppendLine(string.Format("\"{1}\"={0}", _index, _fileName));
        }

        /// <summary>
        /// Get the INF formatted line indicating the destination directory on the device where
        /// the file will be copied to.
        /// e.g. (where 1 is the index)
        /// 
        /// Files.Common1=0,"%InstallDir%"
        /// </summary>
        /// <returns></returns>
        public void AppendINFDestinationDirBlock(StringBuilder INF)
        {
            INF.AppendLine(string.Format("{0}=0,\"%InstallDir%\"", GetINFCopyFileOperationName()));
        }

        /// <summary>
        /// Get the actual operation in the INF file responsible
        /// for copying the file to the correct location on the device.
        /// e.g. (where 1 is the index and NETCFv35.wm.armv4i.cab is the file name)
        /// 
        /// [Files.Common1]
        /// "NETCFv35.wm.armv4i.cab","NETCFv35.wm.armv4i.cab",,0
        /// </summary>
        /// <returns></returns>
        public void AppendINFCopyFileOperationBlock(StringBuilder INF)
        {
            INF.AppendLine(string.Format("[{0}]", GetINFCopyFileOperationName()));
            INF.AppendLine(string.Format("\"{0}\",\"{0}\",,0", _fileName));
            INF.AppendLine();
        }

        #endregion //INF

        #endregion //Methods
    }
}