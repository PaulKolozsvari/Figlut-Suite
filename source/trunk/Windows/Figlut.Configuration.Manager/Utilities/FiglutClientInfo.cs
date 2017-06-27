namespace Figlut.Configuration.Manager.Utilities
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Utilities.Serialization;

    #endregion //Using Directives

    [Serializable]
    public class FiglutClientInfo
    {
        #region Constructors

        public FiglutClientInfo()
        {
        }

        public FiglutClientInfo(
            Guid applicationId,
            string executableName,
            string version,
            string platform,
            string clientId)
        {
            _applicationId = applicationId;
            _executableName = executableName;
            _version = version;
            _platform = platform;
            _clientId = clientId;
        }

        #endregion //Constructors

        #region Constants

        private const string K = "B3C6D191-1DB1-4F60-BFD4-F6615ABD02A5";
        private const string I = "D50727D2";
        private const string PREFIX = "Figlut";

        #endregion //Constants

        #region Fields

        private Guid _applicationId;
        private Nullable<DateTime> _connectionDate;
        private string _requestUri;
        private string _clientId;
        private string _executableName;
        private string _version;
        private string _platform;

        #endregion //Fields

        #region Properties

        public Guid ApplicationId
        {
            get { return _applicationId; }
            set { _applicationId = value; }
        }

        public Nullable<DateTime> ConnectionDate
        {
            get { return _connectionDate; }
            set { _connectionDate = value; }
        }

        public string RequestUri
        {
            get { return _requestUri; }
            set { _requestUri = value; }
        }

        public string ClientId
        {
            get { return _clientId; }
            set { _clientId = value; }
        }

        public string ExecutableName
        {
            get { return _executableName; }
            set { _executableName = value; }
        }

        public string Version
        {
            get { return _version; ;}
            set { _version = value; }
        }

        public string Platform
        {
            get { return _platform; }
            set { _platform = value; }
        }

        #endregion //Properties

        #region Methods

        private void ValidateClientInfo()
        {
            if (ApplicationId == Guid.Empty)
            {
                throw new UserThrownException(
                    string.Format("{0} not specified for {1}.",
                    EntityReader<FiglutClientInfo>.GetPropertyName(p => p.ApplicationId, false),
                    typeof(FiglutClientInfo).FullName),
                    true);
            }
            if (string.IsNullOrEmpty(ExecutableName))
            {
                throw new UserThrownException(
                    string.Format("{0} not specified for {1}.",
                    EntityReader<FiglutClientInfo>.GetPropertyName(p => p.ExecutableName, false),
                    typeof(FiglutClientInfo).FullName),
                    true);
            }
            if (string.IsNullOrEmpty(Version))
            {
                throw new UserThrownException(
                    string.Format("{0} not specified for {1}.",
                    EntityReader<FiglutClientInfo>.GetPropertyName(p => p.Version, false),
                    typeof(FiglutClientInfo).FullName),
                    true);
            }
            if (string.IsNullOrEmpty(Platform))
            {
                throw new UserThrownException(
                    string.Format("{0} not specified for {1}.",
                    EntityReader<FiglutClientInfo>.GetPropertyName(p => p.Platform, false),
                    typeof(FiglutClientInfo).FullName),
                    true);
            }
            if (string.IsNullOrEmpty(ClientId))
            {
                throw new UserThrownException(
                    string.Format("{0} not specified for {1}.",
                    EntityReader<FiglutClientInfo>.GetPropertyName(p => p.ClientId, false),
                    typeof(FiglutClientInfo).FullName),
                    true);
            }
        }

        internal static FiglutClientInfo CreateFromHash(string s)
        {
            if (!s.StartsWith(PREFIX))
            {
                return null;
            }
            else
            {
                s = s.Remove(0, PREFIX.Length);
            }
            return (FiglutClientInfo)GOC.Instance.GetSerializer(SerializerType.XML).DeserializeFromText(
                typeof(FiglutClientInfo),
                (new Encryptor(K, I)).Decrypt(s));
        }

        internal string GetHash()
        {
            ValidateClientInfo();
            return string.Concat(
                PREFIX,
                (new Encryptor(K, I)).Encrypt(GOC.Instance.GetSerializer(SerializerType.XML).SerializeToText(this)));
        }

        #endregion //Methods
    }
}
