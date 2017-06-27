namespace Figlut.Server.Toolkit.Utilities.RAS
{
    #region Using Directives

    using DotRas;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    /// <summary>
    /// A wrapper class for managing RAS connections e.g. VPN or dial up connections.
    /// </summary>
    public class RasConnectionManager
    {
        #region Constructors

        /// <summary>
        /// Utility class for managing windows connections e.g. VPN or dial up connections.
        /// </summary>
        public RasConnectionManager() : this(null)
        {
        }

        /// <summary>
        /// Utility class for managing windows connections e.g. VPN or dial up connections.
        /// </summary>
        /// <param name="tag">Any object to be used as a state bag.</param>
        public RasConnectionManager(object tag)
        {
            _tag = tag;
            _rasDialer = new RasDialer();
            _rasDialer.StateChanged += _rasDialer_StateChanged;
            _rasDialer.DialCompleted += _rasDialer_DialCompleted;
            _rasPhoneBook = new RasPhoneBook();
        }

        #endregion //Constructors

        #region Events

        /// <summary>
        /// Event fired when the state of the a windows connection changes.
        /// </summary>
        public event OnConnectionStateChangedHandler OnConnectionStateChanged;

        /// <summary>
        /// Event fired after a dial up connection has been completed.
        /// </summary>
        public event OnDialCompletedHandler OnDialCompleted;

        #endregion //Events

        #region Fields

        private RasDialer _rasDialer;
        private RasPhoneBook _rasPhoneBook;
        private object _tag;

        #endregion //Fields

        #region Properties

        public RasDialer RasDialer
        {
            get { return _rasDialer; }
        }

        public RasEntryCollection RasConnections
        {
            get { return _rasPhoneBook.Entries; }
        }

        public object Tag
        {
            get { return _tag; }
        }

        #endregion //Properties

        #region Event Handlers

        private void _rasDialer_StateChanged(object sender, StateChangedEventArgs e)
        {
            if (OnConnectionStateChanged != null)
            {
                OnConnectionStateChanged(this, e);
            }
        }

        private void _rasDialer_DialCompleted(object sender, DialCompletedEventArgs e)
        {
            if (OnDialCompleted != null)
            {
                OnDialCompleted(this, e);
            }
        }

        #endregion //Event Handlers

        #region Methods

        /// <summary>
        /// Gets the path to the address book containing information about all the windows connections.
        /// </summary>
        /// <param name="phoneBookType">The type of phone book e.g. current user or all users.</param>
        /// <returns></returns>
        public string GetRasConnectionsAddressBookPath(RasPhoneBookType phoneBookType)
        {
            return RasPhoneBook.GetPhoneBookPath(phoneBookType);
        }

        /// <summary>
        /// Gets the names of all the available RAS (VPN or dial up) connections.
        /// </summary>
        /// <returns></returns>
        public List<string> GetRasConnectionNames()
        {
            _rasPhoneBook.Open();
            List<string> result = new List<string>();
            for (int i = 0; i < _rasPhoneBook.Entries.Count; i++)
            {
                RasEntry entry = _rasPhoneBook.Entries[i];
                result.Add(entry.Name);
            }
            return result;
        }

        /// <summary>
        /// Creates a VPN connection in the Windows list of connections.
        /// </summary>
        /// <param name="vpnConnectionName">The name of the VPN connection to create.</param>
        /// <param name="strategy">The VPN strategy to use. If null the default strategy is used.</param>
        /// <param name="deviceType">The RAS (Remote Access Serice) device type. If null the default is used i.e.RasDeviceType.Vpn</param>
        /// <param name="deviceName">The name of the RAS device. If null the default is used i.e. "(PPTP)".</param>
        public void CreateVPNConnectionEntry(
            string vpnConnectionName, 
            Nullable<RasVpnStrategy> strategy, 
            Nullable<RasDeviceType> deviceType, 
            string deviceName)
        {
            _rasPhoneBook.Open();
            if (string.IsNullOrEmpty(vpnConnectionName))
            {
                throw new NullReferenceException("vpnConnectionName may not be null or empty when requesting for a VPN connection to be created.");
            }
            if (!strategy.HasValue)
            {
                strategy = RasVpnStrategy.Default;
            }
            if (!deviceType.HasValue)
            {
                deviceType = RasDeviceType.Vpn;
            }
            if (string.IsNullOrEmpty(deviceName))
            {
                deviceName = "(PPTP)";
            }
            RasEntry entry = RasEntry.CreateVpnEntry(
                vpnConnectionName,
                IPAddress.Loopback.ToString(),
                strategy.Value,
                RasDevice.GetDeviceByName(deviceName, deviceType.Value));
            _rasPhoneBook.Entries.Add(entry);
        }

        /// <summary>
        /// Creates a dial-up connection in the windows list of connections.
        /// </summary>
        /// <param name="dialupConnectionName">The name of the dial-up connection to create.</param>
        /// <param name="phoneNumber">The phone number to be dialed when dialing the dial-up connection.</param>
        /// <param name="deviceType">The RAS (Remote Access Serice) device type. If null the default is used i.e.RasDeviceType.Modem</param>
        /// <param name="deviceName">The name of the RAS device. May not be null.</param>
        public void CreateDialupConnectionEntry(
            string dialupConnectionName, 
            string phoneNumber, 
            Nullable<RasDeviceType> deviceType,
            string deviceName)
        {
            _rasPhoneBook.Open();
            if (string.IsNullOrEmpty(dialupConnectionName))
            {
                throw new NullReferenceException("dialupConnectionName may not be null or empty when requesting for a dial-up connection to be created.");
            }
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new NullReferenceException("phoneNumber may not be null or empty when requesting to for dial-up connection to be created.");
            }
            if (!deviceType.HasValue)
            {
                deviceType = RasDeviceType.Modem;
            }
            RasEntry entry = RasEntry.CreateDialUpEntry(
                dialupConnectionName,
                phoneNumber,
                RasDevice.GetDeviceByName(deviceName, deviceType.Value));
            _rasPhoneBook.Entries.Add(entry);
        }

        /// <summary>
        /// Gets a list of RAS devices available e.g. modems, VPN connections etc.
        /// </summary>
        /// <returns></returns>
        public List<RasDevice> GetDevices()
        {
            List<RasDevice> result = new List<RasDevice>();
            RasDevice.GetDevices().ToList().ForEach(p => result.Add(p));
            return result;
        }

        /// <summary>
        /// Finds the RAS connection (VPN or dial up) and asynchronously dials that connection.
        /// </summary>
        /// <param name="rasConnectionName">The name of the RAS connection e.g. MyCompanyVPN</param>
        /// <param name="userName">The username to specify when dialing.</param>
        /// <param name="password">The password to specify when dialing.</param>
        /// <param name="timeout">Length of time until asynchronous dialing times out..</param>
        /// <param name="phoneBookType">The type of phone book e.g. current user or all users.</param>
        public void ConnectRasConnection(string rasConnectionName, string userName, string password, int timeout, RasPhoneBookType phoneBookType)
        {
            _rasPhoneBook.Open(GetRasConnectionsAddressBookPath(phoneBookType));
            if (!_rasPhoneBook.Entries.Contains(rasConnectionName))
            {
                throw new NullReferenceException(string.Format("No connection exists with the name {0}.", rasConnectionName));
            }
            _rasDialer.EntryName = rasConnectionName;
            _rasDialer.PhoneBookPath = GetRasConnectionsAddressBookPath(phoneBookType);
            NetworkCredential credentials = new NetworkCredential(userName, password);
            _rasDialer.Timeout = timeout;
            _rasDialer.DialAsync();
        }

        /// <summary>
        /// Disconnects a RAS connection.
        /// </summary>
        /// <param name="rasConnectionName">The name of the RAS connection e.g. MyCompanyVPN</param>
        public void DisconnectRasConnection(string rasConnectionName)
        {
            if (_rasDialer.IsBusy)
            {
                //The connection attempt has not been completed, cancel the attempt.
                _rasDialer.DialAsyncCancel();
                return;
            }
            IReadOnlyCollection<RasConnection> activeConnections = RasConnection.GetActiveConnections();
            foreach (RasConnection connection in activeConnections)
            {
                if (connection.EntryName == rasConnectionName)
                {
                    connection.HangUp(3000);
                    return;
                }
            }
            throw new Exception(string.Format("Could not find any active RAS connection with the name {0}.", rasConnectionName));
        }

        #endregion //Methods
    }
}