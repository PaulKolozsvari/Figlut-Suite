namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using Renci.SshNet;
    using Renci.SshNet.Sftp;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class SSHFtpClient
    {
        #region Constructors

        public SSHFtpClient()
            : this(22, null, null, null)
        {
        }

        public SSHFtpClient(int port, string host, string userName, string password)
        {
            _port = port;
            _host = host;
            _userName = userName;
            _password = password;
        }

        #endregion //Constructors

        #region Fields

        private int _port;
        private string _host;
        private string _userName;
        private string _password;

        #endregion //Fields

        #region Properties

        public int Port { get; set; }

        public string Host { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        #endregion //Properties

        #region Methods

        //http://blog.deltacode.be/2012/01/05/uploading-a-file-using-sftp-in-c-sharp/
        public void UploadFile(string sourceFilePath, string sftpWorkingDirectory)
        {
            using (SftpClient client = new SftpClient(_host, _port, _userName, _password))
            {
                Console.WriteLine("Connecting to {0}:{1}", _host, _port);
                client.Connect();
                Console.WriteLine("Changing directory to {0} ...", sftpWorkingDirectory);
                client.ChangeDirectory(sftpWorkingDirectory);
                using (FileStream fileStream = new FileStream(sourceFilePath, FileMode.Open))
                {
                    string fileName = Path.GetFileName(sourceFilePath);
                    Console.WriteLine("Uploading {0} ...", sourceFilePath);
                    client.BufferSize = 4 * 1024; // bypass Payload error large files
                    client.UploadFile(fileStream, fileName);
                }
                Console.WriteLine("Disconnecting from {0} ...", _host, _port);
                client.Disconnect();
            }
        }

        //https://sshnet.codeplex.com/discussions/403235
        public void DownloadFile(string sftpWorkingDirectory, string sftpFileName, string localDestinationFilePath)
        {
            if (File.Exists(localDestinationFilePath))
            {
                File.Delete(localDestinationFilePath);
            }
            using (SftpClient client = new SftpClient(_host, _port, _userName, _password))
            {
                Console.WriteLine("Connecting to {0}:{1}", _host, _port);
                client.Connect();
                Console.WriteLine("Changing directory to {0} ...", sftpWorkingDirectory);
                client.ChangeDirectory(sftpWorkingDirectory);
                if (client.Exists(sftpFileName))
                {
                    using (FileStream fileStream = File.OpenWrite(localDestinationFilePath))
                    {
                        Console.WriteLine("Downloading {0} ...", sftpFileName);
                        client.DownloadFile(sftpFileName, fileStream);
                        fileStream.Close();
                    }
                }
                else
                {
                    throw new FileNotFoundException(string.Format("Could not find SFTP file {0}{1} on host {2}:{3}.", sftpWorkingDirectory, sftpFileName, _host, _port));
                }
                Console.WriteLine("Disconnecting from {0} ...", _host, _port);
                client.Disconnect();
            }
        }

        public List<string> GetDirectoryListing(string sftpWorkingDirectory, string fileExtension)
        {
            List<string> result = new List<string>();
            string fileExtensionLower = fileExtension.ToLower();
            using (SftpClient client = new SftpClient(_host, _port, _userName, _password))
            {
                Console.WriteLine("Connecting to {0}:{1}", _host, _port);
                client.Connect();
                Console.WriteLine("Changing directory to {0} ...", sftpWorkingDirectory);
                client.ChangeDirectory(sftpWorkingDirectory);
                IEnumerable<SftpFile> files = client.ListDirectory(".");
                foreach (SftpFile f in files)
                {
                    string currentFileExtension = Path.GetExtension(f.Name).ToLower();
                    if (string.IsNullOrEmpty(fileExtension) || currentFileExtension == fileExtensionLower)
                    {
                        result.Add(f.Name);
                    }
                }
                Console.WriteLine("Disconnecting from {0} ...", _host, _port);
                client.Disconnect();
            }
            return result;
        }

        public void DeleteFile(string sftpWorkingDirectory, string sftpFileName)
        {
            using (SftpClient client = new SftpClient(_host, _port, _userName, _password))
            {
                Console.WriteLine("Connecting to {0}:{1}", _host, _port);
                client.Connect();
                Console.WriteLine("Changing directory to {0} ...", sftpWorkingDirectory);
                client.ChangeDirectory(sftpWorkingDirectory);
                if (client.Exists(sftpFileName))
                {
                    Console.WriteLine("Deleting {0} ...", sftpFileName);
                    client.DeleteFile(sftpFileName);
                }
                else
                {
                    throw new FileNotFoundException(string.Format("Could not find SFTP file {0}{1} on host {2}:{3}.", sftpWorkingDirectory, sftpFileName, _host, _port));
                }
                Console.WriteLine("Disconnecting from {0} ...", _host, _port);
                client.Disconnect();
            }
        }

        #endregion //Methods
    }
}