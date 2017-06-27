namespace Figlut.ReModem.Configuration
{
    #region Using Directives

    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Utilities.SettingsFile;
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class FiglutReModemSettings : Settings
    {
        #region Constructors

        public FiglutReModemSettings()
            : base()
        {
        }

        public FiglutReModemSettings(string filePath)
            : base(filePath)
        {
        }

        public FiglutReModemSettings(string name, string filePath)
            : base(name, filePath)
        {
        }

        #endregion //Constructors

        #region Fields

        #region ReModem

        private string _modemSerialPortName;
        private int _modemSerialPortBaudRate;
        private Parity _modemSerialPortParity;
        private int _modemSerialPortDataBits;
        private StopBits _modemSerialPortStopBits;
        private Handshake _modemSerialPortHandshake;
        private bool _modemSerialPortDtrEnable;
        private bool _modemSerialPortRtsEnable;
        private bool _ignoreTimeZoneOnSmsDateReceived;
        private bool _logAllSmsReceived;
        private bool _logAllSmsSent;
        private bool _logModemSerialPortCommunication;

        private string _defaultDialupConnectionName;
        private string _defaultDialupConnectionUserName;
        private string _defaultDialupConnectionPassword;
        private int _dialupConnectionAttemptTimeout;
        private bool _logAllDialupStateChangedEvents;

        private string _runCmdAfterDial;

        #endregion //ReModem

        #region Logging

        private bool _logToFile;
        private bool _logToWindowsEventLog;
        private bool _logToConsole;
        private string _logFileName;
        private string _eventSourceName;
        private string _eventLogName;
        private LoggingLevel _loggingLevel;

        #endregion //Logging

        #endregion //Fields

        #region Properties

        #region ReModem

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "The name of the COM serial port to connect to the 3G modem.", CategorySequenceId = 0)]
        public string ModemSerialPortName
        {
            get { return _modemSerialPortName; }
            set { _modemSerialPortName = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "The baud rate to use when connecting to the 3G modem.", CategorySequenceId = 1)]
        public int ModemSerialPortBaudRate
        {
            get { return _modemSerialPortBaudRate; }
            set { _modemSerialPortBaudRate = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "The parity to use when connection to the 3G modem i.e. None, Odd, Even, Mark or Space.", CategorySequenceId = 2)]
        public Parity ModemSerialPortParity
        {
            get { return _modemSerialPortParity; }
            set { _modemSerialPortParity = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "The data bits to use when connecting to the 3G modem e.g. 8.", CategorySequenceId = 3)]
        public int ModemSerialPortDatabits
        {
            get { return _modemSerialPortDataBits; }
            set { _modemSerialPortDataBits = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "The number of bits to use when connecting to the 3G modem i.e. None, One, Two or OnePointFive.", CategorySequenceId = 4)]
        public StopBits ModemSerialPortStopBits
        {
            get { return _modemSerialPortStopBits; }
            set { _modemSerialPortStopBits = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "The handshake to use when connecting to the 3G modemd i.e. None, XOnXOff, RequestToSend or RequestToSendXOnXOff.", CategorySequenceId = 5)]
        public Handshake ModemSerialPortHandshake
        {
            get { return _modemSerialPortHandshake; }
            set { _modemSerialPortHandshake = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "Whether to enable the Data Terminal Ready (DTR) signal during communication with the modem.", CategorySequenceId = 6)]
        public bool ModemSerialPortDtrEnable
        {
            get { return _modemSerialPortDtrEnable; }
            set { _modemSerialPortDtrEnable = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "Whether to enable the Request To Send (RTS) signal during communication with the 3G modem.", CategorySequenceId = 7)]
        public bool ModemSerialPortRtsEnable
        {
            get { return _modemSerialPortRtsEnable; }
            set { _modemSerialPortRtsEnable = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "Whether to strip the timezone from the given date time from the modem and thus taking it as local time.", CategorySequenceId = 8)]
        public bool IgnoreTimeZoneOnSmsDateReceived
        {
            get { return _ignoreTimeZoneOnSmsDateReceived; }
            set { _ignoreTimeZoneOnSmsDateReceived = value; }
        }

        [SettingInfo("ReModem", DisplayName = "Log all SMS received", Description = "Whether or not to log all SMS messages received by the 3G modem.", CategorySequenceId = 9)]
        public bool LogAllSmsReceived
        {
            get { return _logAllSmsReceived; }
            set { _logAllSmsReceived = value; }
        }

        [SettingInfo("ReModem", DisplayName = "Log all SMS sent", Description = "Whether or not to log all SMS messages sent by the ReModem application via the 3G modem.", CategorySequenceId = 10)]
        public bool LogAllSmsSent
        {
            get { return _logAllSmsSent; }
            set { _logAllSmsSent = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "Whether to log all communication between ReModem and the 3G modem via the serial port. This should only be enabled for testing and troubleshooting.", CategorySequenceId = 11)]
        public bool LogModemSerialPortCommunication
        {
            get { return _logModemSerialPortCommunication; }
            set { _logModemSerialPortCommunication = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName =  true, Description = "The name of the dial-up connection to control via SMS commands.", CategorySequenceId = 12)]
        public string DefaultDialupConnectionName
        {
            get { return _defaultDialupConnectionName; }
            set { _defaultDialupConnectionName = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "The user name to use when starting the dial-up connection.", CategorySequenceId = 13)]
        public string DefaultDialupConnectionUserName
        {
            get { return _defaultDialupConnectionUserName; }
            set { _defaultDialupConnectionUserName = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "The password to use when starting the dial-up connection.", CategorySequenceId = 14)]
        public string DefaultDialupConnectionPassword
        {
            get { return _defaultDialupConnectionPassword; }
            set { _defaultDialupConnectionPassword = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "Length of time in milliseconds until dialing times out if connection cannot be made.", CategorySequenceId = 15)]
        public int DialupConnectionAttemptTimeout
        {
            get { return _dialupConnectionAttemptTimeout; }
            set { _dialupConnectionAttemptTimeout = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "Whether or not to log all dial-up connection status messages as the dial up connection is attempting to connect.", CategorySequenceId = 16)]
        public bool LogAllDialupStateChangedEvents
        {
            get { return _logAllDialupStateChangedEvents; }
            set { _logAllDialupStateChangedEvents = value; }
        }

        [SettingInfo("ReModem", AutoFormatDisplayName = true, Description = "The windows Run command after a dial up connection is started.", CategorySequenceId = 17)]
        public string RunCmdAfterDial
        {
            get { return _runCmdAfterDial; }
            set { _runCmdAfterDial = value; }
        }

        #endregion //ReModem

        #region Logging

        [SettingInfo("Logging", DisplayName = "To File", Description = "Whether or not to log to a text log file in the executing directory.", CategorySequenceId = 0)]
        public bool LogToFile
        {
            get { return _logToFile; }
            set { _logToFile = value; }
        }

        [SettingInfo("Logging", DisplayName = "To Windows Event Log", Description = "Whether or not to log to the Windows Event Log.", CategorySequenceId = 1)]
        public bool LogToWindowsEventLog
        {
            get { return _logToWindowsEventLog; }
            set { _logToWindowsEventLog = value; }
        }

        [SettingInfo("Logging", DisplayName = "To Console", Description = "Whether or not to write all log messages to the console. Useful when running the service as a console application i.e. running the executable with the /test_mode switch.", CategorySequenceId = 2)]
        public bool LogToConsole
        {
            get { return _logToConsole; }
            set { _logToConsole = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The name of the text log file to log to. The log file is written in the executing directory.", CategorySequenceId = 3)]
        public string LogFileName
        {
            get { return _logFileName; }
            set { _logFileName = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The name of the event source to use when logging to the Windows Event Log.", CategorySequenceId = 4)]
        public string EventSourceName
        {
            get { return _eventSourceName; }
            set { _eventSourceName = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The name of the Windows Event Log to log to.", CategorySequenceId = 5)]
        public string EventLogName
        {
            get { return _eventLogName; }
            set { _eventLogName = value; }
        }

        [SettingInfo("Logging", AutoFormatDisplayName = true, Description = "The extent of messages being logged: None = logging is disabled, Minimum = logs server start/stop and exceptions, Normal = logs additional information messages, Maximum = logs all requests and responses to the server.", CategorySequenceId = 6)]
        public LoggingLevel LoggingLevel
        {
            get { return _loggingLevel; }
            set { _loggingLevel = value; }
        }

        #endregion //Logging

        #endregion //Properties
    }
}