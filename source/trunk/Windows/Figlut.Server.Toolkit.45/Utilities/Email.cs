namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class Email
    {
        #region Constructors

        public Email(
            string fromAddress,
            List<string> toAddresses,
            string body,
            string subject)
        {
            _fromAddress = fromAddress;
            _toAddresses = toAddresses;
            _body = body;
            _subject = subject;
            _ccAddresses = new List<string>();
            _bccAddresses = new List<string>();
            _attachments = new List<object>();
            _smtpPort = 25;
        }

        #endregion //Constructors

        #region Fields

        private string _body;
        private List<string> _toAddresses;
        private string _fromAddress;
        private string _subject;
        private List<string> _ccAddresses;
        private List<string> _bccAddresses;
        private List<object> _attachments;
        private SmtpClient _smtpClient;
        private bool _htmlBody;
        private string _smtpHost;
        private int _smtpPort;
        private MailPriority _priority;

        #endregion //Fields

        #region Properties

        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        public List<string> ToAddresses
        {
            get { return _toAddresses; }
            set { _toAddresses = value; }
        }

        public string FromAddress
        {
            get { return _fromAddress; }
            set { _fromAddress = value; }
        }

        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        public List<string> CcAddresses
        {
            get { return _ccAddresses; }
            set { _ccAddresses = value; }
        }

        public List<string> BccAddresses
        {
            get { return _bccAddresses; }
            set { _bccAddresses = value; }
        }

        public List<object> Attachments
        {
            get { return _attachments; }
            set { _attachments = value; }
        }

        public SmtpClient SmtpClient
        {
            get { return _smtpClient; }
        }

        public bool HtmlBody
        {
            get { return _htmlBody; }
            set { _htmlBody = value; }
        }

        public string SmtpHost
        {
            get { return _smtpHost; }
            set { _smtpHost = value; }
        }

        public int SmtpPort
        {
            get { return _smtpPort; }
            set { _smtpPort = value; }
        }

        public MailPriority Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        
        #endregion //Properties

        #region Methods

        public void Send()
        {
            MailAddressCollection recipients = new MailAddressCollection();
            MailMessage mailMessage = new MailMessage()
            {
                Subject = _subject,
                Body = _body + "\r\n",
                Priority = _priority,
                IsBodyHtml = _htmlBody,
                From = new MailAddress(_fromAddress),
            };
            _toAddresses.ForEach(p => mailMessage.To.Add(p));
            _ccAddresses.ForEach(p => mailMessage.CC.Add(p));
            _bccAddresses.ForEach(p => mailMessage.Bcc.Add(p));
            using (SmtpClient smtpClient = new SmtpClient(_smtpHost, _smtpPort))
            {
                smtpClient.Send(mailMessage);
            }
        }

        #endregion //Methods
    }
}
