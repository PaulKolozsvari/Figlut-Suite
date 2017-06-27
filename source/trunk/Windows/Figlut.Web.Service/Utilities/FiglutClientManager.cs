namespace Figlut.Web.Service.Utilities
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
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Web.Service.Configuration;

    #endregion //Using Directives

    public class FiglutClientManager
    {
        #region Singleton Setup

        private static FiglutClientManager _instance;

        public static FiglutClientManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FiglutClientManager();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private FiglutClientManager()
        {
            _settings = GOC.Instance.GetSettings<FiglutWebServiceSettings>();
            string executingDirectory = Information.GetExecutingDirectory();
            _figlutDesktopDataBoxClients = new EntityCache<Guid, FiglutClientInfo>("Figlut Desktop DataBox Clients", Path.Combine(executingDirectory, "FiglutDesktopDataBoxClients.txt"));
            _figlutMobileDataBoxClients = new EntityCache<Guid, FiglutClientInfo>("Figlut Mobile DataBox Clients", Path.Combine(executingDirectory, "FiglutMobileDataBoxClients.txt"));
            _nonFiglutClients = new EntityCache<Guid, FiglutClientInfo>("Non Figlut Clients", Path.Combine(executingDirectory, "NonFiglutClients.txt"));
            _clientTrace = new EntityCache<Guid, FiglutClientInfo>("Figlut Client Trace", Path.Combine(executingDirectory, "FiglutClientTrace.txt"));
            _encryptor = new Encryptor(K, I);
            _saveQueue = new List<SaveFileInfo>();
            _lock = new object();
        }

        #region Inner Types

        internal class SaveFileInfo
        {
            public string FilePath { get; set; }
            public string Contents { get; set; }
            public bool Saved { get; set; }
        }

        #endregion //Inner Types

        #endregion //Constructors

        #region Constants

        private Guid FIGLUT_DESKTOP_DATABOX_APPLICATION_ID = new Guid("438E0151-F39A-4A4A-BD15-4FD2D842E058");
        private Guid FIGLUT_MOBILE_DATABOX_APPLICATION_ID = new Guid("F92655FA-A532-4a51-8EE7-18C84D05E8B6");
        private Guid NON_FIGLUT_APPLICATION_ID = new Guid("0A0A4CA3-6E8C-47CA-A32A-0463D8E6C1C5");

        private const string K = "9650B172-4940-4D81-8411-09447A739633";
        private const string I = "8F177D91";

        #endregion //Constants

        #region Fields

        private FiglutWebServiceSettings _settings;
        private EntityCache<Guid, FiglutClientInfo> _figlutDesktopDataBoxClients;
        private EntityCache<Guid, FiglutClientInfo> _figlutMobileDataBoxClients;
        private EntityCache<Guid, FiglutClientInfo> _nonFiglutClients;
        private EntityCache<Guid, FiglutClientInfo> _clientTrace;
        private Encryptor _encryptor;

        private List<SaveFileInfo> _saveQueue;
        private object _lock;

        #endregion //Fields

        #region Methods

        private void ProcessSaveQueue()
        {
            lock (_lock)
            {
                SaveFileInfo[] toSave = _saveQueue.ToArray();
                foreach (SaveFileInfo s in toSave)
                {
                    if (!s.Saved)
                    {
                        File.WriteAllText(s.FilePath, s.Contents);
                        s.Saved = true;
                        _saveQueue.Remove(s);
                    }
                }
            }
        }

        internal void ManageClient(string userAgent, string requestUri)
        {
            EntityCache<Guid, FiglutClientInfo> clientInfoCache = null;
            bool saveClientInfo = false;
            FiglutClientInfo figlutClientInfo = FiglutClientInfo.CreateFromHash(userAgent);
            if (figlutClientInfo == null) //Not a Figlut client.
            {
                figlutClientInfo = new FiglutClientInfo() { ApplicationId = NON_FIGLUT_APPLICATION_ID, ExecutableName = userAgent, ClientId = "N/A", Platform = "N/A", Version = "N/A" };
                clientInfoCache = _nonFiglutClients;
            }
            else //Figlut client.
            {
                if (figlutClientInfo.ApplicationId == FIGLUT_DESKTOP_DATABOX_APPLICATION_ID)
                {
                    clientInfoCache = _figlutDesktopDataBoxClients;
                }
                else if (figlutClientInfo.ApplicationId == FIGLUT_MOBILE_DATABOX_APPLICATION_ID)
                {
                    clientInfoCache = _figlutMobileDataBoxClients;
                }
                else
                {
                    throw new Exception(string.Format("Invalid {0} of {1}.", EntityReader<FiglutClientInfo>.GetPropertyName(p => p.ApplicationId, false), figlutClientInfo.ApplicationId));
                }
            }
            figlutClientInfo.ConnectionDate = DateTime.Now;
            figlutClientInfo.RequestUri = requestUri;
            if (!clientInfoCache.Exists(figlutClientInfo.ApplicationId))
            {
                if (clientInfoCache.Count >= 1)
                {
                    throw new UserThrownException(
                        "CAL (Client Access License) count has been exceeded on this CTP (Community Technology Preview) version. Restart the Figlut Web Service and retry connecting a different device.",
                        LoggingLevel.Minimum);
                }
                clientInfoCache.Add(figlutClientInfo.ApplicationId, figlutClientInfo);
                saveClientInfo = true;
            }
            else if (_settings.EnableClientTrace || clientInfoCache.Exists(figlutClientInfo.ApplicationId)) //Update the latest connection date and request URI for this specific client.
            {
                FiglutClientInfo existingClientInfo = clientInfoCache[figlutClientInfo.ApplicationId];
                existingClientInfo.ConnectionDate = figlutClientInfo.ConnectionDate;
                existingClientInfo.RequestUri = figlutClientInfo.RequestUri;
                saveClientInfo = true;
            }
            if (_settings.EnableClientTrace) //Add it to the trace.
            {
                FiglutClientInfo traceFiglutClientInfo = GetTraceFiglutClientInfo(figlutClientInfo);
                _clientTrace.Add(traceFiglutClientInfo.ApplicationId, traceFiglutClientInfo);
                string traceXml = GOC.Instance.GetSerializer(SerializerType.XML).SerializeToText(_clientTrace);
                _saveQueue.Add(new SaveFileInfo() { FilePath = _clientTrace.DefaultFilePath, Contents = _encryptor.Encrypt(traceXml) });
                saveClientInfo = true;
                GOC.Instance.Logger.LogMessage(new LogMessage(
                    string.Format("Added {0} to save queue.", _clientTrace.DefaultFilePath),
                    LogMessageType.Information,
                    LoggingLevel.Maximum));
            }
            if (saveClientInfo)
            {
                string xml = GOC.Instance.GetSerializer(SerializerType.XML).SerializeToText(clientInfoCache);
                _saveQueue.Add(new SaveFileInfo() { FilePath = clientInfoCache.DefaultFilePath, Contents = _encryptor.Encrypt(xml) });
                GOC.Instance.Logger.LogMessage(new LogMessage(
                    string.Format("Added {0} to save queue.", clientInfoCache.DefaultFilePath),
                    LogMessageType.Information,
                    LoggingLevel.Maximum));
                Thread thread = new Thread(new ThreadStart(ProcessSaveQueue));
                thread.Start();
            }
        }

        private FiglutClientInfo GetTraceFiglutClientInfo(FiglutClientInfo figlutClientInfo)
        {
            FiglutClientInfo result = new FiglutClientInfo();
            EntityReader.CopyProperties(figlutClientInfo, result, false);
            result.ApplicationId = Guid.NewGuid();
            return result;
        }

        internal void Stop()
        {
            if (File.Exists(_figlutDesktopDataBoxClients.DefaultFilePath))
            {
                File.Delete(_figlutDesktopDataBoxClients.DefaultFilePath);
            }
            if (File.Exists(_figlutMobileDataBoxClients.DefaultFilePath))
            {
                File.Delete(_figlutMobileDataBoxClients.DefaultFilePath);
            }
            if (File.Exists(_nonFiglutClients.DefaultFilePath))
            {
                File.Delete(_nonFiglutClients.DefaultFilePath);
            }
            if (File.Exists(_clientTrace.DefaultFilePath))
            {
                File.Delete(_clientTrace.DefaultFilePath);
            }
        }

        #endregion //Methods
    }
}