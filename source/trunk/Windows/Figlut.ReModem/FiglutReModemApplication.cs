namespace Figlut.ReModem
{
    #region Using Directives

    using Figlut.ReModem.Configuration;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.RAS;
    using Figlut.Server.Toolkit.Utilities.SMS;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    #endregion //Using Directives

    public class FiglutReModemApplication
    {
        #region Singleton Setup

        private static FiglutReModemApplication _instance;

        public static FiglutReModemApplication Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FiglutReModemApplication();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private FiglutReModemApplication()
        {
            _rasConnectionManagerQueue = new List<RasConnectionManager>();
        }

        #endregion //Constructors

        #region Constants

        public const string SMS_COMMAND_START_DEFAULT_DIALUP = "dial default";
        public const string SMS_COMMAND_STOP_DEFAULT_DIALUP = "hangup default";

        #endregion //Constants

        #region Fields

        private SmsGateway _smsGateway;
        private FiglutReModemSettings _settings;
        private List<RasConnectionManager> _rasConnectionManagerQueue;

        #endregion //Fields

        #region Methods

        public void Start(FiglutReModemSettings settings)
        {
            _settings = settings;
            GOC.Instance.Logger = new Logger(
                settings.LogToFile,
                settings.LogToWindowsEventLog,
                settings.LogToConsole,
                settings.LoggingLevel,
                settings.LogFileName,
                settings.EventSourceName,
                settings.EventLogName);
            GOC.Instance.Logger.LogMessage(new LogMessage("Initializing SMS Gateway ...", LogMessageType.Information, LoggingLevel.Maximum));
            _smsGateway = new SmsGateway(
                settings.ModemSerialPortName,
                settings.ModemSerialPortBaudRate,
                settings.ModemSerialPortParity,
                settings.ModemSerialPortDatabits,
                settings.ModemSerialPortStopBits,
                settings.ModemSerialPortHandshake,
                settings.ModemSerialPortDtrEnable,
                settings.ModemSerialPortRtsEnable,
                Environment.NewLine,
                settings.IgnoreTimeZoneOnSmsDateReceived,
                true);
            _smsGateway.OnSmsReceived += _smsGateway_OnSmsReceived;
            _smsGateway.OpenConnectionToModem();
            _smsGateway.ConfigureModemForNotificationResponseToPC();
        }

        public void Stop()
        {
            _smsGateway.CloseConnectionToModem();
        }

        #endregion //Methods

        #region Event Handlers

        private void _smsGateway_OnSmsReceived(object sender, SmsReceivedEventArgs e)
        {
            if (_settings.LogAllSmsReceived)
            {
                GOC.Instance.Logger.LogMessage(new LogMessage(e.SmsReceivedInfo.ToString(), LogMessageType.Information, LoggingLevel.Minimum));
            }
            RasConnectionManager rasConnectionManager = new RasConnectionManager(e.SmsReceivedInfo.CellPhoneNumber);
            rasConnectionManager.OnConnectionStateChanged += _rasConnectionManager_OnConnectionStateChanged;
            rasConnectionManager.OnDialCompleted += _rasConnectionManager_OnDialCompleted;
            try
            {
                string receivedSmsMessage = e.SmsReceivedInfo.SmsMessage.Trim().ToLower();
                switch (receivedSmsMessage)
                {
                    case SMS_COMMAND_START_DEFAULT_DIALUP:
                        _rasConnectionManagerQueue.Add(rasConnectionManager);
                        rasConnectionManager.ConnectRasConnection(
                            _settings.DefaultDialupConnectionName,
                            _settings.DefaultDialupConnectionUserName,
                            _settings.DefaultDialupConnectionPassword,
                            _settings.DialupConnectionAttemptTimeout,
                            DotRas.RasPhoneBookType.AllUsers);
                        break;
                    case SMS_COMMAND_STOP_DEFAULT_DIALUP:
                        rasConnectionManager.DisconnectRasConnection(_settings.DefaultDialupConnectionName);
                        rasConnectionManager.OnConnectionStateChanged -= _rasConnectionManager_OnConnectionStateChanged;
                        rasConnectionManager.OnDialCompleted -= _rasConnectionManager_OnDialCompleted;

                        SmsInfo smsToSend = new SmsInfo(e.SmsReceivedInfo.CellPhoneNumber, string.Format("{0} disconnected.", _settings.DefaultDialupConnectionName));
                        GOC.Instance.Logger.LogMessage(new LogMessage(smsToSend.SmsMessage, LogMessageType.Information, LoggingLevel.Normal));
                        _smsGateway.SendSms(smsToSend);
                        LogSmsSent(smsToSend);
                        break;
                    default:
                        throw new Exception(string.Format("Invalid command '{0}'.", e.SmsReceivedInfo.SmsMessage));
                }
            }
            catch (Exception ex)
            {
                GOC.Instance.Logger.LogMessage(new LogError(ex, LoggingLevel.Normal));
                SmsInfo smsToSend = new SmsInfo(e.SmsReceivedInfo.CellPhoneNumber, ex.Message);
                _smsGateway.SendSms(smsToSend);
                LogSmsSent(smsToSend);
                GOC.Instance.Logger.LogMessage(new LogError(ex, LoggingLevel.Normal));
            }
        }

        private void LogSmsSent(SmsInfo smsInfo)
        {
            if (_settings.LogAllSmsSent)
            {
                StringBuilder smsLogMessage = new StringBuilder();
                smsLogMessage.AppendLine(string.Format("SMS Sent to {0}:", smsInfo.CellPhoneNumber));
                smsLogMessage.AppendLine(string.Format("\"{0}\"", smsInfo.SmsMessage));
                GOC.Instance.Logger.LogMessage(new LogMessage(smsLogMessage.ToString(), LogMessageType.Information, LoggingLevel.Minimum));
            }
        }

        private void _rasConnectionManager_OnDialCompleted(object sender, DotRas.DialCompletedEventArgs e)
        {
            RasConnectionManager rasConnectionManager = (RasConnectionManager)sender;
            StringBuilder summary = new StringBuilder();
            summary.AppendLine(string.Format("Dial Connected: {0}", e.Connected.ToString()));
            if (!e.Connected)
            {
                summary.AppendLine(string.Format("Dial Error: {0}", e.Error.Message));
            }
            summary.AppendLine(string.Format("Dial Timed Out: {0}", e.TimedOut));
            summary.AppendLine(string.Format("Dial Cancelled: {0}", e.Cancelled));
            string summaryMessage = summary.ToString();
            GOC.Instance.Logger.LogMessage(new LogMessage(summaryMessage, LogMessageType.Information, LoggingLevel.Normal));

            string cellPhoneNumberToRespondTo = rasConnectionManager.Tag.ToString();
            //Clean up and get rid of the RAS connection manager.
            rasConnectionManager.OnConnectionStateChanged -= _rasConnectionManager_OnConnectionStateChanged;
            rasConnectionManager.OnDialCompleted -= _rasConnectionManager_OnDialCompleted;
            _rasConnectionManagerQueue.Remove(rasConnectionManager);
            if (!string.IsNullOrEmpty(_settings.RunCmdAfterDial))
            {
                //Process.Start(_settings.RunCmdAfterDial);
                ProcessStartInfo startInfo = new ProcessStartInfo(_settings.RunCmdAfterDial);
                startInfo.UseShellExecute = true;
                Process.Start(startInfo);
            }
            SmsInfo smsToSend = new SmsInfo(cellPhoneNumberToRespondTo, summaryMessage);
            _smsGateway.SendSms(smsToSend);
            LogSmsSent(smsToSend);
        }

        private void _rasConnectionManager_OnConnectionStateChanged(object sender, DotRas.StateChangedEventArgs e)
        {
            if (_settings.LogAllDialupStateChangedEvents)
            {
                GOC.Instance.Logger.LogMessage(new LogMessage(
                    string.Format("Dial {0}", e.State.ToString()), 
                    LogMessageType.Information, 
                    LoggingLevel.Minimum));
            }
        }

        #endregion //Event Handlers
    }
}