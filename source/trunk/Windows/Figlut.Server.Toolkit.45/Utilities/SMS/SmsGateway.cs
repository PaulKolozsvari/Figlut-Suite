namespace Figlut.Server.Toolkit.Utilities.SMS
{
    #region Using Directives

    using Figlut.Server.Toolkit.Utilities.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class SmsGateway
    {
        #region Constructors

        public SmsGateway(
            string modemSerialPortName,
            int modemSerialPortBaudRate,
            Parity modemSerialPortParity,
            int modemSerialPortDataBits,
            StopBits modemSerialPortStopBits,
            Handshake modemSerialPortHandshake,
            bool modemSerialPortDtrEnable,
            bool modemSerialPortRtsEnable,
            string modemSerialPortNewLine,
            bool ignoreTimeZoneOnSmsDateReceived,
            bool logModemCommunication)
        {
            _modemSerialPortName = modemSerialPortName;
            _modemSerialPortBaudRate = modemSerialPortBaudRate;
            _modemSerialPortParity = modemSerialPortParity;
            _modemSerialPortDataBits = modemSerialPortDataBits;
            _modemSerialPortStopBits = modemSerialPortStopBits;
            _modemSerialPortHandshake = modemSerialPortHandshake;
            _modemSerialPortDtrEnable = modemSerialPortDtrEnable;
            _modemSerialPortRtsEnable = modemSerialPortRtsEnable;
            _modemSerialPortNewLine = modemSerialPortNewLine;
            _ignoreTimeZoneOnSmsDateReceived = ignoreTimeZoneOnSmsDateReceived;
            _logModemCommunication = logModemCommunication;
            _memoryBanksToClearQueue = new List<int>();
            InitializeSerialPort();
        }

        #endregion //Constructors

        #region Events

        /// <summary>
        /// Event fired when an SMS is received by the SMS Gateway.
        /// </summary>
        public event OnSmsReceivedHandler OnSmsReceived;

        #endregion //Events

        #region Fields

        private SerialPort _serialPort;

        private string _modemSerialPortName;
        private int _modemSerialPortBaudRate;
        private Parity _modemSerialPortParity;
        private int _modemSerialPortDataBits;
        private StopBits _modemSerialPortStopBits;
        private Handshake _modemSerialPortHandshake;
        private bool _modemSerialPortDtrEnable;
        private bool _modemSerialPortRtsEnable;
        private string _modemSerialPortNewLine;
        private bool _ignoreTimeZoneOnSmsDateReceived;
        private bool _logModemCommunication;

        private List<int> _memoryBanksToClearQueue;

        #endregion //Fields

        #region Constants

        public const char CMD_ENTER = (char)13;
        public const string CMD_AT = "AT";
        public const string CMD_OK = "OK";
        public const string CMD_FORMAT_AS_TEXT_MESSAGE_AT = "AT+CMGF=1";
        public const string CMD_CONFIGURE_NOTIFICATION_RESPONSE_AT = "AT+CNMI=1,1,0,0,0";
        public const string CMD_SEND_SMS = "AT+CMGS="; //Followed by the cell phone number e.g. AT+CMGS="0763777515"
        public const char CMD_END_SEND_SMS = (char)26;

        public const string CMD_RECEIVED_SMS_NOTIFICATION_PREFIX = "+CMTI:"; //This is a notification from the modem that an SMS has arrived an is sitting in a specific memory bank on the SIM card.
        public const string CMD_READ_SMS_FROM_MEMORY_BANK = "AT+CMGR="; //Followed by the memory bank number and an enter command.
        public const string CMD_RECEIVED_SMS_DETAILS_PREFIX = "+CMGR:"; //This is a notification containing the SMS that we requested to be retrieved from the SIM card when sending AT+CMGR= command.
        public const string CMD_CLEAR_SIM_MEMORY_BANK = "AT+CMGD="; //Followed by the memory bank number of the SIM card to clear.

        #endregion //Constants

        #region Methods

        private void LogMessage(string message)
        {
            if (_logModemCommunication)
            {
                GOC.Instance.Logger.LogMessage(new LogMessage(message, LogMessageType.Information, LoggingLevel.Minimum));
            }
        }

        private void InitializeSerialPort()
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = _modemSerialPortName;
            _serialPort.BaudRate = _modemSerialPortBaudRate;
            _serialPort.Parity = _modemSerialPortParity;
            _serialPort.DataBits = _modemSerialPortDataBits;
            _serialPort.StopBits = _modemSerialPortStopBits;
            _serialPort.Handshake = _modemSerialPortHandshake;
            _serialPort.DtrEnable = _modemSerialPortDtrEnable;
            _serialPort.RtsEnable = _modemSerialPortRtsEnable;
            _serialPort.NewLine = _modemSerialPortNewLine;

            _serialPort.DataReceived += _serialPort_DataReceived;
        }

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort serialPort = sender as SerialPort;
            if (serialPort == null)
            {
                throw new InvalidCastException(
                    string.Format("Data received from serial port but sender object is not of type {0}.", typeof(SerialPort).Name));
            }
            string allDataReceived = serialPort.ReadExisting();
            LogMessage(allDataReceived);
            string[] allDataLinesReceived = allDataReceived.Split((char)13);
            for(int i = 0; i < allDataLinesReceived.Length; i++)
            {
                string line = allDataLinesReceived[i];
                if (line.Contains(CMD_RECEIVED_SMS_NOTIFICATION_PREFIX)) //This is a notification from the modem that an SMS has arrived and is sitting in a specific memory bank on the SIM card.
                {
                    ParseSmsReceivedNotificationMessageLine(line);
                    continue;
                }
                else if (line.Contains(CMD_RECEIVED_SMS_DETAILS_PREFIX)) //i.e. contains +CMGR:
                {
                    ParseSmsDetailsMessageLines(line, allDataLinesReceived, i);
                    continue;
                }
            }
        }

        /// <summary>
        /// This method handles what happens when the modem notifies the PC when an SMS has arrived and is sitting in a 
        /// specific memory bank on the SIM card. After parsing the line containing memory bank details it then requests
        /// the modem to retrieve and write out the SMS details and message to the serial port, which will then be handled
        /// by the HandleSmsDetailsMessageLines method.
        /// </summary>
        /// <param name="notificationMessageLine">The line containing the infor on which memory bank the SMS is on.</param>
        private void ParseSmsReceivedNotificationMessageLine(string notificationMessageLine)
        {
            int simMemoryBank = Convert.ToInt32(notificationMessageLine.Split(',')[1].Trim());
            string cmdReadSmsFromMemoryBank = string.Format("{0}{1}{2}", CMD_READ_SMS_FROM_MEMORY_BANK, simMemoryBank.ToString(), CMD_ENTER);
            _serialPort.WriteLine(cmdReadSmsFromMemoryBank); //Tell the modem that we want to retrieve the message from the SIM card in a specific memory bank.
            Thread.Sleep(3000);
            _memoryBanksToClearQueue.Add(simMemoryBank);
        }

        /// <summary>
        /// This method handles the parsing of the actual SMS that was retrieved from a specific memory bank on the SIM card.
        /// </summary>
        /// <param name="smsDetailsMessageLine"></param>
        /// <param name="allDataLinesReceived"></param>
        /// <param name="allDataLinesIndex"></param>
        private void ParseSmsDetailsMessageLines(string smsDetailsMessageLine, string[] allDataLinesReceived, int allDataLinesIndex)
        {
            /*Sample SMS data read off the bank.
            * 
            * +CMGR: "REC UNREAD","+27763777515",,"13/11/01,20:18:59+08"
            Hello, how are you?
            Great to hear from you
                        
            OK
            */
            SmsReceivedInfo smsReceivedInfo = new SmsReceivedInfo();
            string[] smsMessageDetails = smsDetailsMessageLine.Split(','); //i.e. this is the first line e.g. +CMGR: "REC UNREAD","+27763777515",,"13/11/01,20:18:59+08"
            smsReceivedInfo.CellPhoneNumber = smsMessageDetails[1].Replace("\"", string.Empty);
            smsReceivedInfo.ReceivedDateTimeRaw = string.Format("{0},{1}", smsMessageDetails[3], smsMessageDetails[4]).Replace("\"", string.Empty);
            smsReceivedInfo.ReceivedDate = GetDateTimeFromSmsDateTime(smsReceivedInfo.ReceivedDateTimeRaw, _ignoreTimeZoneOnSmsDateReceived);
            StringBuilder smsMessage = new StringBuilder();
            int messageLineIndex = allDataLinesIndex + 1; //The next line contains the first line of the SMS message.
            while (true) //Go through the next lines containing the SMS message.
            {
                string smsMessageLine = allDataLinesReceived[messageLineIndex].Replace("\n", string.Empty);
                if (smsMessageLine.Contains(CMD_OK))
                {
                    break; //End of SMS message.
                }
                smsMessage.AppendLine(smsMessageLine);
                messageLineIndex++;
            }
            smsReceivedInfo.SmsMessage = smsMessage.ToString();
            if (OnSmsReceived != null)
            {
                OnSmsReceived(this, new SmsReceivedEventArgs(smsReceivedInfo));
            }
            if (_memoryBanksToClearQueue.Count > 0)
            {
                int simMemoryBankToClear = _memoryBanksToClearQueue.First();
                ClearSimMemoryBank(simMemoryBankToClear);
                _memoryBanksToClearQueue.RemoveAt(0);
            }
        }

        /// <summary>
        /// Clears a memory bank of the SIM card. This should be done after an SMS has been read off a memory bank and has been handled.
        /// </summary>
        /// <param name="simMemoryBankToClear"></param>
        public void ClearSimMemoryBank(int simMemoryBankToClear)
        {
            if (!_serialPort.IsOpen)
            {
                return;
            }
            LogMessage(string.Format("Clearing SIM memory bank {0} ...", simMemoryBankToClear));
            string cmdClearSimMemoryBank = string.Format("{0}{1}{2}", CMD_CLEAR_SIM_MEMORY_BANK, simMemoryBankToClear, CMD_ENTER);
            _serialPort.WriteLine(cmdClearSimMemoryBank);
            Thread.Sleep(3000);
        }

        /// <summary>
        /// Parses a date sent by the modem to a .NET DateTime.
        /// </summary>
        /// <param name="receivedDateTimeRaw">The received date of the SMS given by the modem expected in this format: "13/11/01,20:18:59+08"</param>
        /// <param name="ignoreTimeZone">Whether to strip the timezone from the given date time i.e. not compare the provided time zone in the date time with the time zone of this machine where the code is running on.</param>
        /// <returns></returns>
        public static Nullable<DateTime> GetDateTimeFromSmsDateTime(string receivedDateTimeRaw, bool ignoreTimeZone)
        {
            string d = receivedDateTimeRaw.Replace('/', '-');
            Nullable<DateTime> result = null;
            try
            {
                if (ignoreTimeZone)
                {
                    d = d.Remove(d.Length - 3, 3);
                    result = DateTime.ParseExact(d, "yy-MM-dd,H:mm:ss", null);
                }
                else
                {
                    result = DateTime.ParseExact(d, "yy-MM-dd,H:mm:sszz", null);
                }
            }
            catch (Exception ex)
            {
                GOC.Instance.Logger.LogMessage(new LogError(ex, LoggingLevel.Normal));
            }
            return result;
        }

        /// <summary>
        /// Configures the modem to send notifications to the PC when an SMS is received.
        /// Notifications containing info on the memory bank of the SIM card where the SMS is stored.
        /// These notifications will be read by this SmsGateway from a specific memory bank of the SIM card.
        /// </summary>
        public void ConfigureModemForNotificationResponseToPC()
        {
            ValidateSerialPortIsOpen();
            LogMessage("Configuring modem to notify when an SMS received ...");
            string cmdFormatAsTextMessage = string.Format("{0}{1}", CMD_FORMAT_AS_TEXT_MESSAGE_AT, CMD_ENTER);
            string cmdNotificationResponseByModem = string.Format("{0}{1}", CMD_CONFIGURE_NOTIFICATION_RESPONSE_AT, CMD_ENTER);
            _serialPort.WriteLine(cmdFormatAsTextMessage);
            Thread.Sleep(2000);
            _serialPort.WriteLine(cmdNotificationResponseByModem);
            Thread.Sleep(2000);
        }

        /// <summary>
        /// Sends a command to the modem informing it to send an SMS to a specific cell phone number.
        /// </summary>
        /// <param name="smsInfo">The details of the SMS to send.</param>
        public void SendSms(SmsInfo smsInfo)
        {
            ValidateSerialPortIsOpen();
            LogMessage(string.Format("Sending SMS to {0} ...", smsInfo.CellPhoneNumber));
            string cmdAt = string.Format("{0}{1}", CMD_AT, CMD_ENTER);
            string cmdFormatAsTextMessage = string.Format("{0}{1}", CMD_FORMAT_AS_TEXT_MESSAGE_AT, CMD_ENTER);
            string cmdSendSmsToCellPhoneNumber = string.Format("{0}\"{1}\"", CMD_SEND_SMS, smsInfo.CellPhoneNumber);
            string cmdSmsMessage = string.Format(">{0}{1}", smsInfo.SmsMessage, CMD_END_SEND_SMS);
            _serialPort.WriteLine(cmdAt);
            Thread.Sleep(2000);
            _serialPort.WriteLine(cmdFormatAsTextMessage);
            Thread.Sleep(3000);
            _serialPort.WriteLine(cmdSendSmsToCellPhoneNumber);
            Thread.Sleep(5000);
            _serialPort.WriteLine(cmdSmsMessage);
            Thread.Sleep(5000);
        }

        private void ValidateSerialPortIsOpen()
        {
            if (!_serialPort.IsOpen)
            {
                throw new Exception(string.Format("Serial port is not open on port {0}. Open serial port first.", _modemSerialPortName));
            }
        }

        public void OpenConnectionToModem()
        {
            try
            {
                if (!_serialPort.IsOpen)
                {
                    LogMessage(string.Format("Opening serial port to modem on {0} ...", _serialPort.PortName));
                    _serialPort.Open();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Could not open connnection to modem on port {0}. {1}.", 
                    _modemSerialPortName, 
                    ex.Message));
            }
        }

        public void CloseConnectionToModem()
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    LogMessage(string.Format("Closing serial port to modem on {0} ...", _serialPort.PortName));
                    _serialPort.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(
                    string.Format("Could not close connection to modem on port {0}.", _modemSerialPortName));
            }
        }

        #endregion //Methods
    }
}