namespace Figlut.SMS.TestDriver
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO.Ports;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Utilities.SMS;

    #endregion //Using Directives

    class Program
    {
        #region Fields

        private static SmsGateway _smsGateway;

        #endregion //Fields

        #region Methods

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Starting SMS Gateway ...");
                _smsGateway = new SmsGateway(
                    "COM17",
                    2400,
                    Parity.None,
                    8,
                    StopBits.One,
                    Handshake.RequestToSend,
                    true,
                    true,
                    Environment.NewLine,
                    true,
                    true);
                _smsGateway.OnSmsReceived += _smsGateway_OnSmsReceived;
                _smsGateway.OpenConnectionToModem();
                _smsGateway.ConfigureModemForNotificationResponseToPC();
                Console.WriteLine("SMS Gateway running. Press any key to stop gateway ...");
                //_smsGateway.SendSms(new SmsInfo("0833958283", "Hello, testing from code."));
                Console.ReadLine();
                Console.WriteLine("Closing SMS Gateway ...");
                _smsGateway.CloseConnectionToModem();
                Console.WriteLine("SMS Gateway closed. Press any key to exit.");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        static void _smsGateway_OnSmsReceived(object sender, SmsReceivedEventArgs e)
        {
            Console.WriteLine(string.Format("Cell Phone: {0}", e.SmsReceivedInfo.CellPhoneNumber));
            Console.WriteLine(string.Format("Date: {0}", e.SmsReceivedInfo.ReceivedDate.ToString()));
            Console.WriteLine("Message:");
            Console.WriteLine(e.SmsReceivedInfo.SmsMessage);
            Console.WriteLine();
            string response = string.Format("Hello from SMS Gateway. You said: '{0}'", e.SmsReceivedInfo.SmsMessage);
            Console.WriteLine(string.Format("Responding with: '{0}'", response));
            _smsGateway.SendSms(new SmsInfo(e.SmsReceivedInfo.CellPhoneNumber, response));
        }

        #endregion //Methods
    }
}
