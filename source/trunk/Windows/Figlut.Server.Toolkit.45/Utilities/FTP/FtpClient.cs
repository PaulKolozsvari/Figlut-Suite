namespace Figlut.Server.Toolkit.Utilities.FTP
{
    using Renci.SshNet;
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class FtpClient
    {
        #region Constructors

        public FtpClient()
            : this(null, null, null, false, DEFAULT_TIMEOUT, DEFAULT_TIMEOUT)
        {
        }

        public FtpClient(string username, string password, string domain, bool keepAlive) : 
            this(username, password, domain, keepAlive, DEFAULT_TIMEOUT, DEFAULT_TIMEOUT)
        {
        }

        public FtpClient(string username, string password, string domain, bool keepAlive, int timeout, int readWriteTimeout)
        {
            if (!string.IsNullOrEmpty(username))
            {
                _credentials = new NetworkCredential(username, password);
                if (!string.IsNullOrEmpty(domain))
                {
                    _credentials.Domain = domain;
                }
            }
            _keepAlive = keepAlive;
            _timeout = timeout;
            _readWriteTimeout = readWriteTimeout;
        }

        #endregion //Constructors

        #region Constants

        public const int DEFAULT_TIMEOUT = 10000000;

        #endregion //Constants

        #region Fields

        private NetworkCredential _credentials;
        public bool _keepAlive;
        public int _timeout;
        private int _readWriteTimeout;

        #endregion //Fields

        #region Properties

        public NetworkCredential Credentials
        {
            get { return _credentials; }
            set { _credentials = value; }
        }

        public bool KeepAlive
        {
            get { return _keepAlive; }
            set { _keepAlive = value; }
        }

        public int Timeout
        {
            get { return _timeout; }
            set { _timeout = value; }
        }

        public int ReadWriteTimeout
        {
            get { return _readWriteTimeout; }
            set { _readWriteTimeout = value; }
        }

        #endregion //Properties

        #region Methods

        public void UploadFileSftp(string sourceFilePath, string outputDirectory, string ftpUrl, bool enableSsl)
        {
            var port = 22;
            var fileOutputPath = $"{outputDirectory}{Path.GetFileName(sourceFilePath)}";

            using (var client = new SftpClient(ftpUrl, port, _credentials.UserName, Credentials.Password))
            {
                client.Connect();
                if (client.IsConnected)
                {
                    byte[] sourceFileBytes = null;
                    using (StreamReader reader = new StreamReader(sourceFilePath))
                    {
                        sourceFileBytes = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                        reader.Close();
                    }

                    using (var ms = new MemoryStream(sourceFileBytes))
                    {
                        client.BufferSize = (uint)ms.Length;
                        client.UploadFile(ms, fileOutputPath);
                    }
                }
            }
        }

        public void UploadFile(string sourceFilePath, string ftpDestinationFileUrl, bool enableSsl)
        {
            if (!File.Exists(sourceFilePath))
            {
                throw new FileNotFoundException(string.Format("Could not find file {0} to upload via FTP.", sourceFilePath));
            }
            FtpWebRequest request = WebRequest.Create(ftpDestinationFileUrl) as FtpWebRequest;
            if (request == null)
            {
                throw new InvalidCastException(string.Format("Could not get {0} for {1}.", typeof(FtpWebRequest).FullName, ftpDestinationFileUrl));
            }
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.KeepAlive = _keepAlive;
            request.Timeout = _timeout;
            request.ReadWriteTimeout = _readWriteTimeout;
            request.Credentials = _credentials;
            request.EnableSsl = enableSsl;
            byte[] sourceFileBytes = null;
            using (StreamReader reader = new StreamReader(sourceFilePath))
            {
                sourceFileBytes = Encoding.UTF8.GetBytes(reader.ReadToEnd());
                reader.Close();
            }
            using (Stream requestStream = request.GetRequestStream())
            {
                requestStream.Write(sourceFileBytes, 0, sourceFileBytes.Length);
                requestStream.Close();
            }
        }

        public void DownloadFile(string ftpSourceFileUrl, string localDestinationFilePath, bool enableSsl)
        {
            FtpWebRequest request = WebRequest.Create(ftpSourceFileUrl) as FtpWebRequest;
            if (request == null)
            {
                throw new InvalidCastException(string.Format("Could not get {0} for {1}.", typeof(FtpWebRequest).FullName, ftpSourceFileUrl));
            }
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.KeepAlive = _keepAlive;
            request.Timeout = _timeout;
            request.ReadWriteTimeout = _readWriteTimeout;
            request.Credentials = _credentials;
            request.EnableSsl = enableSsl;
            if (File.Exists(localDestinationFilePath))
            {
                File.Delete(localDestinationFilePath);
            }
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (FileStream fileStream = File.Create(localDestinationFilePath))
                    {
                        byte[] buffer = new byte[32 * 1024];
                        int readLength;
                        while((readLength = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, readLength);
                        }
                        fileStream.Close();
                    }
                    responseStream.Close();
                }
                response.Close();
            }
        }

        public List<string> GetDirectoryListing(string ftpDirectoryUrl, string fileExtension)
        {
            List<string> result = new List<string>();
            FtpWebRequest request = WebRequest.Create(ftpDirectoryUrl) as FtpWebRequest;
            if (request == null)
            {
                throw new InvalidCastException(string.Format("Could not get {0} for {1}.", typeof(FtpWebRequest).FullName, ftpDirectoryUrl));
            }
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.KeepAlive = _keepAlive;
            request.Timeout = _timeout;
            request.ReadWriteTimeout = _readWriteTimeout;
            request.Credentials = _credentials;
            string fileExtensionLower = fileExtension.ToLower();
            using(FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responseStream))
                    {
                        while (!reader.EndOfStream)
                        {
                            string line = reader.ReadLine();
                            string fileName = line.Split(new[] { ' ', '\t' }).Last();
                            string currentFileExtension = Path.GetExtension(fileName).ToLower();
                            if (string.IsNullOrEmpty(fileExtension) || currentFileExtension == fileExtensionLower)
                            {
                                result.Add(fileName);
                            }
                        }
                        reader.Close();
                    }
                    responseStream.Close();
                }
                response.Close();
            }
            return result;
        }

        public string DeleteFile(string ftpFileUrl)
        {
            string statusDescription = null;
            FtpWebRequest request = WebRequest.Create(ftpFileUrl) as FtpWebRequest;
            if (request == null)
            {
                throw new InvalidCastException(string.Format("Could not get {0} for {1}.", typeof(FtpWebRequest).FullName, ftpFileUrl));
            }
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.KeepAlive = _keepAlive;
            request.Timeout = _timeout;
            request.ReadWriteTimeout = _readWriteTimeout;
            request.Credentials = _credentials;
            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                statusDescription = response.StatusDescription;
                response.Close();
            }
            return statusDescription;
        }

        #endregion //Methods
    }
}
