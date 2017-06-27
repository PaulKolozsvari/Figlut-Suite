namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using Figlut.Server.Toolkit.Utilities;
    using System.Net;
    using System.IO;
    using Figlut.Server.Toolkit.Data;

    #endregion //Using Directives

    public class FtpHelper
    {
        #region Inner Types

        public class FtpFileInfo
        {
            #region Constructors

            public FtpFileInfo(string localFilePath, string ftpFileUri)
            {
                if (string.IsNullOrEmpty(localFilePath))
                {
                    throw new NullReferenceException(string.Format(
                        "{0} may not be null or empty.",
                        EntityReader<FtpFileInfo>.GetPropertyName(p => p.LocalFilePath, false)));
                }
                if (string.IsNullOrEmpty(ftpFileUri))
                {
                    throw new NullReferenceException(string.Format(
                        "{0} may not be null or empty.",
                        EntityReader<FtpFileInfo>.GetPropertyName(p => p.FtpFileUri, false)));
                }
                _localFilePath = localFilePath;
                _ftpFileUri = ftpFileUri;
            }

            #endregion //Constructors

            #region Fields

            private string _localFilePath;
            private string _ftpFileUri;

            #endregion //Fields

            #region Properties

            public string LocalFilePath
            {
                get { return _localFilePath; }
            }

            public string FtpFileUri
            {
                get { return _ftpFileUri; }
            }

            #endregion //Properties
        }

        #endregion //Inner Types

        #region Constructors

        public FtpHelper(string username, string password)
        {
            _username = username;
            _password = password;
        }

        #region Inner Types

        public delegate void FileTransferProgressHandler(FtpTransferProgressResult e);
        public event FileTransferProgressHandler OnFileUploadProgress;

        public delegate void FileTransferCompletedHandler(FtpTransferProgressResult e);
        public event FileTransferCompletedHandler OnFileUploadCompleted;

        #endregion //Inner Types

        #endregion //Constructors

        #region Fields

        private string _username;
        private string _password;

        #endregion //Fields

        #region Properties

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }

        #endregion //Properties

        #region Methods

        public void UploadFiles(List<FtpFileInfo> files, int bufferSize)
        {
            int fileCount = files.Count;
            long allFilesTransferredBytes = 0;
            long allFilesTotalBytes = 0;
            foreach (FtpFileInfo f in files)
            {
                FileSystemHelper.ValidateFileExists(f.LocalFilePath);
                using (FileStream fs = File.OpenRead(f.LocalFilePath))
                {
                    allFilesTotalBytes += fs.Length;
                }
            }
            for (int i = 0; i < files.Count; i++)
            {
                FtpFileInfo f = files[i];
                FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(f.FtpFileUri);
                ftpRequest.Credentials = new NetworkCredential(_username, _password);
                ftpRequest.KeepAlive = true;
                ftpRequest.UseBinary = true;
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                string currentFileName = Path.GetFileName(f.LocalFilePath);
                long currentFileTransferedBytes = 0;
                long currentFileTotalBytes = 0;
                using (FileStream fs = File.OpenRead(f.LocalFilePath))
                {
                    currentFileTotalBytes = fs.Length;
                    byte[] buffer = new byte[bufferSize];
                    int bytesRead = fs.Read(buffer, 0, buffer.Length);
                    currentFileTransferedBytes = bytesRead;
                    allFilesTransferredBytes += bytesRead;
                    using (Stream stream = ftpRequest.GetRequestStream())
                    {
                        while (bytesRead != 0)
                        {
                            stream.Write(buffer, 0, bytesRead);
                            if (OnFileUploadProgress != null)
                            {
                                OnFileUploadProgress(new FtpTransferProgressResult(
                                    currentFileName,
                                    currentFileTransferedBytes,
                                    currentFileTotalBytes,
                                    fileCount,
                                    i,
                                    allFilesTransferredBytes,
                                    allFilesTotalBytes));
                            }
                            bytesRead = fs.Read(buffer, 0, buffer.Length);
                            currentFileTransferedBytes += bytesRead;
                            allFilesTransferredBytes += bytesRead;
                        }
                    }
                }
                if (OnFileUploadCompleted != null)
                {
                    OnFileUploadCompleted(new FtpTransferProgressResult(
                        currentFileName,
                        currentFileTransferedBytes,
                        currentFileTotalBytes,
                        fileCount,
                        i,
                        allFilesTransferredBytes,
                        allFilesTotalBytes));
                }
            }
        }

        public void DownloadFiles(List<FtpFileInfo> files, int bufferSize)
        {
            foreach (FtpFileInfo f in files)
            {
                if (f == null)
                {
                    continue;
                }
                FtpWebRequest ftpRequest = (FtpWebRequest)FtpWebRequest.Create(f.FtpFileUri);
                ftpRequest.Credentials = new NetworkCredential(_username, _password);
                ftpRequest.KeepAlive = true;
                ftpRequest.UseBinary = true;
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                using (FileStream fileStream = File.Create(f.LocalFilePath))
                {
                    using (FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse())
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (BinaryReader reader = new BinaryReader(stream))
                            {
                                byte[] buffer = new byte[bufferSize];
                                int bytesReceived = reader.Read(buffer, 0, buffer.Length);
                                while (bytesReceived > 0)
                                {
                                    fileStream.Write(buffer, 0, buffer.Length);
                                    bytesReceived = reader.Read(buffer, 0, buffer.Length);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion //Methods
    }
}

