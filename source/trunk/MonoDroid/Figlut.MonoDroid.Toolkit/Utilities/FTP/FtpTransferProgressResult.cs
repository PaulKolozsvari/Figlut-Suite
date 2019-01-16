namespace Figlut.MonoDroid.Toolkit.Utilities.FTP
{
    #region Using Directives

    using System;

    #endregion Using Directives

    public class FtpTransferProgressResult : EventArgs
    {
        public FtpTransferProgressResult(string fileName, long currentFileTransferedBytes, long currentFileTotalBytes)
        {
            _currentFileName = fileName;
            _currentFileTransferedBytes = currentFileTransferedBytes;
            _currentFileTotalBytes = currentFileTotalBytes;
            _currentFilePercentageCompleted = Convert.ToInt32((((double)_currentFileTransferedBytes) / ((double)_currentFileTotalBytes)) * 100);
        }

        public FtpTransferProgressResult(
            string currentFileName,
            long currentFileTransferedBytes,
            long currentFileTotalBytes,
            int fileCount,
            int currentFileIndex,
            long allFilesTransferredBytes,
            long allFilesTotalBytes)
            : this(currentFileName, currentFileTransferedBytes, currentFileTotalBytes)
        {
            _fileCount = fileCount;
            _currentFileIndex = currentFileIndex;
            _allFilesTransferredBytes = allFilesTransferredBytes;
            _allFilesTotalBytes = allFilesTotalBytes;
            _allFilesPercentageCompleted = Convert.ToInt32((((double)_allFilesTransferredBytes) / ((double)_allFilesTotalBytes)) * 100);
        }

        #region Fields

        private string _currentFileName;
        private long _currentFileTransferedBytes;
        private long _currentFileTotalBytes;
        private int _currentFilePercentageCompleted;

        private int _fileCount;
        private int _currentFileIndex;
        private long _allFilesTransferredBytes;
        private long _allFilesTotalBytes;
        public int _allFilesPercentageCompleted;

        #endregion //Fields

        #region Properties

        public string CurrentFileName
        {
            get { return _currentFileName; }
        }

        public long CurrentFileTransferedBytes
        {
            get { return _currentFileTransferedBytes; }
        }

        public long CurrentFileTotalBytes
        {
            get { return _currentFileTotalBytes; }
        }

        public int CurrentFilePercentageCompleted
        {
            get { return _currentFilePercentageCompleted; }
        }

        public int FileCount
        {
            get { return _fileCount; }
        }

        public int CurrentFileIndex
        {
            get { return _currentFileIndex; }
        }

        public long AllFilesTransferredBytes
        {
            get { return _allFilesTransferredBytes; }
        }

        public long AllFilesTotalBytes
        {
            get { return _allFilesTotalBytes; }
        }

        public int AllFilesPercentageCompleted
        {
            get { return _allFilesPercentageCompleted; }
        }

        #endregion //Properties
    }
}