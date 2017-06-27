namespace Figlut.Configuration.Manager.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities;

    #endregion //Using Directives

    public class FiglutWebServiceClientManager : IDisposable
    {
        #region Inner Types

        public delegate void FiglutWebServiceClientsInfoChangedHandler(EventArgs e);

        #endregion //Inner Types

        #region Constants

        private const string K = "9650B172-4940-4D81-8411-09447A739633";
        private const string I = "8F177D91";

        #endregion //Constants

        #region Constructors

        internal FiglutWebServiceClientManager()
        {
            _figlutWebServiceInstallationDirectory = Path.GetDirectoryName(ComponentsSettings.Instance[ComponentId.FiglutWebService].FilePath);
            if (!Directory.Exists(_figlutWebServiceInstallationDirectory))
            {
                throw new DirectoryNotFoundException(string.Format("Could not find {0}.", _figlutWebServiceInstallationDirectory));
            }
            _encryptor = new Encryptor(K, I);
            _figlutDesktopDataBoxClients = new ClientView(
                ClientViewType.FiglutDesktopDataBox, 
                new EntityCache<Guid, FiglutClientInfo>("Figlut Desktop DataBox Clients", Path.Combine(_figlutWebServiceInstallationDirectory, "FiglutDesktopDataBoxClients.txt")));
            _figlutMobileDataBoxClients = new ClientView(
                ClientViewType.FiglutMobileDataBox, 
                new EntityCache<Guid, FiglutClientInfo>("Figlut Mobile DataBox Clients", Path.Combine(_figlutWebServiceInstallationDirectory, "FiglutMobileDataBoxClients.txt")));
            _nonFiglutClients = new ClientView(
                ClientViewType.NonFiglutClients, 
                new EntityCache<Guid, FiglutClientInfo>("Non Figlut Clients", Path.Combine(_figlutWebServiceInstallationDirectory, "NonFiglutClients.txt")));
            ConsolidateAllClientsInfo();

            _clientTrace = new ClientView(
                ClientViewType.Trace,
                new EntityCache<Guid, FiglutClientInfo>("Figlut Client Trace", Path.Combine(_figlutWebServiceInstallationDirectory, "FiglutClientTrace.txt")));

            _fileSystemWatcher = new FileSystemWatcher(_figlutWebServiceInstallationDirectory) { IncludeSubdirectories = false };
            _fileSystemWatcher.Changed += _fileSystemWatcher_Changed;
            //_fileSystemWatcher.Created += _fileSystemWatcher_Created;
            _fileSystemWatcher.Deleted += _fileSystemWatcher_Deleted;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        #endregion //Constructors

        #region Fields

        private Encryptor _encryptor;
        private string _figlutWebServiceInstallationDirectory;
        private ClientView _figlutDesktopDataBoxClients;
        private ClientView _figlutMobileDataBoxClients;
        private ClientView _nonFiglutClients;
        private ClientView _allClients;
        private ClientView _clientTrace;
        private FileSystemWatcher _fileSystemWatcher;

        public event FiglutWebServiceClientsInfoChangedHandler OnFiglutWebServiceClientsInfoChanged;

        #endregion //Fields

        #region Properties

        internal string FiglutWebServiceInstallationDirectory
        {
            get { return _figlutWebServiceInstallationDirectory; }
        }

        internal ClientView FiglutDesktopDataBoxClients
        {
            get { return _figlutDesktopDataBoxClients; }
        }

        internal ClientView FiglutMobileDataBoxClients
        {
            get { return _figlutMobileDataBoxClients; }
        }

        internal ClientView NonFiglutClients
        {
            get { return _nonFiglutClients; }
        }

        internal ClientView AllClients
        {
            get { return _allClients; }
        }

        internal ClientView ClientTrace
        {
            get { return _clientTrace; }
        }

        #endregion //Properties

        #region Methods

        private void ConsolidateAllClientsInfo()
        {
            if (_allClients == null)
            {
                _allClients = new ClientView(ClientViewType.AllClients, new EntityCache<Guid, FiglutClientInfo>());
            }
            else
            {
                _allClients.Clients.Clear();
            }
            _figlutDesktopDataBoxClients.Clients.ToList().ForEach(p => _allClients.Clients.Add(p));
            _figlutMobileDataBoxClients.Clients.ToList().ForEach(p => _allClients.Clients.Add(p));
            _nonFiglutClients.Clients.ToList().ForEach(p => _allClients.Clients.Add(p));
        }

        internal void RefreshAllClientsInfoFiles()
        {
            if (File.Exists(_figlutDesktopDataBoxClients.Clients.DefaultFilePath))
            {
                _figlutDesktopDataBoxClients.Clients.LoadFromText(_encryptor.Decrypt(File.ReadAllText(_figlutDesktopDataBoxClients.Clients.DefaultFilePath)));
            }
            if (File.Exists(_figlutMobileDataBoxClients.Clients.DefaultFilePath))
            {
                _figlutMobileDataBoxClients.Clients.LoadFromText(_encryptor.Decrypt(File.ReadAllText(_figlutMobileDataBoxClients.Clients.DefaultFilePath)));
            }
            if (File.Exists(_nonFiglutClients.Clients.DefaultFilePath))
            {
                _nonFiglutClients.Clients.LoadFromText(_encryptor.Decrypt(File.ReadAllText(_nonFiglutClients.Clients.DefaultFilePath)));
            }
            if (File.Exists(_clientTrace.Clients.DefaultFilePath))
            {
                _clientTrace.Clients.LoadFromText(_encryptor.Decrypt(File.ReadAllText(_clientTrace.Clients.DefaultFilePath)));
            }
            ConsolidateAllClientsInfo();
        }

        internal void RefreshClientsInfoFile(FileSystemEventArgs e)
        {
            ClientView clientView = null;
            bool consolidate = false;
            if (e.FullPath == _clientTrace.Clients.DefaultFilePath)
            {
                clientView = _clientTrace;
            }
            if (e.FullPath == _figlutDesktopDataBoxClients.Clients.DefaultFilePath)
            {
                clientView = _figlutDesktopDataBoxClients;
                consolidate = true;
            }
            else if (e.FullPath == _figlutMobileDataBoxClients.Clients.DefaultFilePath)
            {
                clientView = _figlutMobileDataBoxClients;
                consolidate = true;
            }
            else if (e.FullPath == _nonFiglutClients.Clients.DefaultFilePath)
            {
                clientView = _nonFiglutClients;
                consolidate = true;
            }

            if (clientView == null)
            {
                return;
            }
            if (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Changed)
            {
                LoadClientViewFromFile(clientView);
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted || e.ChangeType == WatcherChangeTypes.Renamed)
            {
                clientView.Clients.Clear();
            }

            if (consolidate)
            {
                ConsolidateAllClientsInfo();
            }
            if (OnFiglutWebServiceClientsInfoChanged != null)
            {
                OnFiglutWebServiceClientsInfoChanged(new EventArgs());
            }
        }

        private void LoadClientViewFromFile(ClientView clientView)
        {
            Thread.Sleep(1000);
            for(int i = 0; i < 10; i++) //Try read the file 10 times i.e. 10 x 6000 miliseconds = 1 minute.
            {
                try
                {
                    clientView.Clients.LoadFromText(_encryptor.Decrypt(File.ReadAllText(clientView.Clients.DefaultFilePath)));
                    break;
                }
                catch (IOException iex) //In case it's being used by another process i.e. if the figlut web service is still writing to it.
                {
                    if (FileSystemHelper.IsFileLocked(iex))
                    {
                        Thread.Sleep(6000);
                    }
                }
            }
        }

        public void Dispose()
        {
            _fileSystemWatcher.Changed -= _fileSystemWatcher_Changed;
            //_fileSystemWatcher.Created -= _fileSystemWatcher_Created;
            _fileSystemWatcher.Deleted -= _fileSystemWatcher_Deleted;
        }

        #endregion //Methods

        #region Event Handlers

        void _fileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            RefreshClientsInfoFile(e);
        }

        //private void _fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        //{
        //    RefreshClientsInfoFile(e);
        //}

        private void _fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            RefreshClientsInfoFile(e);
        }

        #endregion //Event Handlers
    }
}
