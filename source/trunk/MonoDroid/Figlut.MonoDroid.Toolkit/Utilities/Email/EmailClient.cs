﻿namespace Figlut.MonoDroid.Toolkit.Utilities.Email
{
    #region Using Directives

    using Android.App;
    using Figlut.MonoDroid.Toolkit.Data;
    using Figlut.MonoDroid.Toolkit.Utilities;
    using Figlut.MonoDroid.Toolkit.Utilities.Logging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Text;

    #endregion //Using Directives

    public class EmailClient
    {
        #region Constructors

        public EmailClient(
            bool emailNotificationsEnabled,
            bool throwEmailFailExceptions,
            EmailProvider emailProvider,
            string exchangeSmtpServer,
            string exchangeSmtpUserName,
            string exchangeSmtpPassword,
            int exchangeSmtpPort,
            bool exchangeSmtpEnableSsl,
            string gmailSmtpServer,
            string gmailSmtpUserName,
            string gmailSmtpPassword,
            int gmailSmtpPort,
            string senderEmailAddress,
            string senderDisplayName,
            string exceptionEmailSubject,
            bool emailLoggingEnabled,
            bool includeDefaultEmailRecipients,
            List<EmailNotificationRecipient> defaultEmailRecipients)
        {
            DataValidator.ValidateStringNotEmptyOrNull(exchangeSmtpServer, nameof(exchangeSmtpServer), nameof(EmailClient));
            DataValidator.ValidateStringNotEmptyOrNull(exchangeSmtpUserName, nameof(exchangeSmtpUserName), nameof(EmailClient));
            DataValidator.ValidateStringNotEmptyOrNull(exchangeSmtpPassword, nameof(exchangeSmtpPassword), nameof(EmailClient));
            DataValidator.ValidateIntegerNotNegative(exchangeSmtpPort, nameof(exchangeSmtpPort), nameof(EmailClient));

            DataValidator.ValidateStringNotEmptyOrNull(gmailSmtpServer, nameof(gmailSmtpServer), nameof(EmailClient));
            DataValidator.ValidateStringNotEmptyOrNull(gmailSmtpUserName, nameof(gmailSmtpUserName), nameof(EmailClient));
            DataValidator.ValidateStringNotEmptyOrNull(gmailSmtpPassword, nameof(gmailSmtpPassword), nameof(EmailClient));
            DataValidator.ValidateIntegerNotNegative(gmailSmtpPort, nameof(gmailSmtpPort), nameof(EmailClient));

            DataValidator.ValidateStringNotEmptyOrNull(senderEmailAddress, nameof(senderEmailAddress), nameof(EmailClient));
            DataValidator.ValidateStringNotEmptyOrNull(senderDisplayName, nameof(senderDisplayName), nameof(EmailClient));
            DataValidator.ValidateStringNotEmptyOrNull(senderDisplayName, nameof(senderDisplayName), nameof(EmailClient));
            DataValidator.ValidateStringNotEmptyOrNull(exceptionEmailSubject, nameof(exceptionEmailSubject), nameof(EmailClient));

            _emailNotificationsEnabled = emailNotificationsEnabled;
            _throwEmailFailExceptions = throwEmailFailExceptions;

            _emailProvider = emailProvider;

            _exchangeSmtpServer = exchangeSmtpServer;
            _exchangeSmtpUserName = exchangeSmtpUserName;
            _exchangeSmtpPassword = exchangeSmtpPassword;
            _exchangeSmtpPort = exchangeSmtpPort;
            _exchangeSmtpEnableSsl = exchangeSmtpEnableSsl;

            _gmailSmtpServer = gmailSmtpServer;
            _gmailSmtpUserName = gmailSmtpUserName;
            _gmailSmtpPassword = gmailSmtpPassword;
            _gmailSmtpPort = gmailSmtpPort;

            _senderEmailAddress = senderEmailAddress;
            _senderDisplayName = senderDisplayName;
            _emailLoggingEnabled = emailLoggingEnabled;
            _exceptionEmailSubject = exceptionEmailSubject;
            _includeDefaultEmailRecipients = includeDefaultEmailRecipients;
            _defaultEmailRecipients = defaultEmailRecipients;
        }

        #endregion //Constructors

        #region Constants

        public const string HTML_LOGO_FILE_NAME = "image002.png";

        #endregion //Constants

        #region Fields

        private bool _emailNotificationsEnabled;
        private bool _throwEmailFailExceptions;

        private EmailProvider _emailProvider;
        private string _exchangeSmtpServer;
        private string _exchangeSmtpUserName;
        private string _exchangeSmtpPassword;
        private int _exchangeSmtpPort;
        private bool _exchangeSmtpEnableSsl;
        private string _gmailSmtpServer;
        private string _gmailSmtpUserName;
        private string _gmailSmtpPassword;
        private int _gmailSmtpPort;
        private string _senderEmailAddress;
        private string _senderDisplayName;
        private string _exceptionEmailSubject;
        private bool _emailLoggingEnabled;
        private bool _includeDefaultEmailRecipients;
        private List<EmailNotificationRecipient> _defaultEmailRecipients;

        #endregion //Fields

        #region Properties

        public EmailProvider EmailProvider
        {
            get { return _emailProvider; }
        }

        public string ExchangeSmtpServer
        {
            get { return _exchangeSmtpServer; }
        }

        public string ExchangeSmtpUserName
        {
            get { return _exchangeSmtpUserName; }
        }

        public string ExchangeSmtpPassword
        {
            get { return _exchangeSmtpPassword; }
        }

        public int ExchangeSmtpPort
        {
            get { return _exchangeSmtpPort; }
        }

        public bool ExchangeSmtpEnableSsl
        {
            get { return _exchangeSmtpEnableSsl; }
        }

        public string GMailSMTPServer
        {
            get { return _gmailSmtpServer; }
        }

        public string GMailSmtpUserName
        {
            get { return _gmailSmtpUserName; }
        }

        public string GMailSmtpPassword
        {
            get { return _gmailSmtpPassword; }
        }

        public int GMailSmtpPort
        {
            get { return _gmailSmtpPort; }
        }

        public string SenderEmailAddress
        {
            get { return _senderEmailAddress; }
        }

        public string SenderDisplayName
        {
            get { return _senderDisplayName; }
        }

        public string ExceptionEmailSubject
        {
            get { return _exceptionEmailSubject; }
        }

        public bool EmailLoggingEnabled
        {
            get { return _emailLoggingEnabled; }
        }

        public bool IncludeDefaultEmailRecipients
        {
            get { return _includeDefaultEmailRecipients; }
        }

        public List<EmailNotificationRecipient> DefaultEmailRecipients
        {
            get { return _defaultEmailRecipients; }
        }

        #endregion //Properties

        #region Methods

        private SmtpClient GetSmtpClient()
        {
            SmtpClient result = new SmtpClient();
            switch (_emailProvider)
            {
                case EmailProvider.Exchange:
                    result.Host = _exchangeSmtpServer;
                    result.Credentials = new NetworkCredential(_exchangeSmtpUserName, _exchangeSmtpPassword);
                    result.Port = _exchangeSmtpPort;
                    result.EnableSsl = _exchangeSmtpEnableSsl;
                    break;
                case EmailProvider.GMail:
                    result.Host = _gmailSmtpServer;
                    result.Credentials = new NetworkCredential(_gmailSmtpUserName, _gmailSmtpPassword);
                    result.Port = _gmailSmtpPort;
                    result.EnableSsl = true;
                    break;
                default:
                    throw new Exception(string.Format("Invalid {0} of '{1}'.", nameof(EmailProvider), _emailProvider.ToString()));
            }
            return result;
        }

        private MailMessage GetMailMessage(string subject, string body, bool isHtml)
        {
            MailMessage result = new MailMessage()
            {
                Sender = new MailAddress(_senderEmailAddress, _senderDisplayName),
                From = new MailAddress(_senderEmailAddress, _senderDisplayName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isHtml
            };
            return result;
        }

        private void AddAttachments(
            MailMessage email,
            List<string> attachmentFileNames,
            List<MemoryStream> attachmentStreams,
            string logoImageFilePath,
            bool isHtml,
            string bodyOriginal,
            out string bodyModified)
        {
            bodyModified = bodyOriginal;
            foreach (string filePath in attachmentFileNames)
            {
                if (string.IsNullOrEmpty(filePath))
                {
                    continue;
                }
                FileSystemHelper.ValidateFileExists(filePath);
                byte[] fileBytes = File.ReadAllBytes(filePath);
                MemoryStream ms = new MemoryStream(fileBytes);
                attachmentStreams.Add(ms);
                string fileName = Path.GetFileName(filePath);
                if (!string.IsNullOrEmpty(logoImageFilePath) && (filePath == logoImageFilePath) && isHtml)
                {
                    //: http://stackoverflow.com/questions/1212838/c-sharp-sending-mails-with-images-inline-using-smtpclient
                    //: http://blog.devexperience.net/en/12/Send_an_Email_in_CSharp_with_Inline_attachments.aspx
                    //: Try this: http://brutaldev.com/post/sending-html-e-mail-with-embedded-images-(the-correct-way)
                    string parentDirectory = Path.GetFileName(Path.GetDirectoryName(filePath));
                    LinkedResource logo = new LinkedResource(filePath);
                    logo.ContentId = fileName;
                    bodyModified = bodyOriginal.Replace(HTML_LOGO_FILE_NAME, "cid:" + logo.ContentId); //Replace the logo file name in the email body with the Content ID.
                    bodyModified = bodyModified.Replace(string.Format(@"{0}/", parentDirectory), string.Empty);
                    AlternateView view = AlternateView.CreateAlternateViewFromString(bodyModified, null, "text/html");
                    view.LinkedResources.Add(logo);
                    email.AlternateViews.Add(view);
                    continue;
                }
                Attachment attachment = new Attachment(ms, fileName, MediaTypeNames.Text.Plain);
                email.Attachments.Add(attachment);
            }
        }

        private void AddRecipientToEmail(string emailAddress, string displayName, MailMessage email, EmailRecipientType recipientType)
        {
            string emailAddressLower = emailAddress.Trim().ToLower();
            foreach (MailAddress a in email.To.ToList())
            {
                if (a.Address.Trim().ToLower() == emailAddressLower)
                {
                    return; //Email address already exists in the recipient's list of the email.
                }
            }
            switch (recipientType)
            {
                case EmailRecipientType.To:
                    email.To.Add(new MailAddress(emailAddress, displayName));
                    break;
                case EmailRecipientType.CC:
                    email.CC.Add(new MailAddress(emailAddress, displayName));
                    break;
                case EmailRecipientType.BCC:
                    email.Bcc.Add(new MailAddress(emailAddress, displayName));
                    break;
                default:
                    break;
            }
        }

        public bool SendEmail(
            EmailCategory category,
            string subject,
            string body,
            List<string> attachmentFileNames,
            bool isHtml,
            List<EmailNotificationRecipient> emailRecipients,
            string logoImageFilePath,
            string logTag,
            Activity activity)
        {
            if (!_emailNotificationsEnabled)
            {
                return false;
            }
            try
            {
                using (SmtpClient client = GetSmtpClient())
                {
                    using (MailMessage email = GetMailMessage(subject, body, isHtml))
                    {
                        if (_includeDefaultEmailRecipients && (_defaultEmailRecipients != null))
                        {
                            _defaultEmailRecipients.ForEach(p => AddRecipientToEmail(p.EmailAddress, p.DisplayName, email, EmailRecipientType.BCC));
                        }
                        if (emailRecipients != null)
                        {
                            emailRecipients.ForEach(p => AddRecipientToEmail(p.EmailAddress, p.DisplayName, email, EmailRecipientType.To));
                        }
                        List<MemoryStream> attachmentStreams = null;
                        try
                        {
                            if (attachmentFileNames != null)
                            {
                                attachmentStreams = new List<MemoryStream>();
                                AddAttachments(email, attachmentFileNames, attachmentStreams, logoImageFilePath, isHtml, body, out body);
                            }
                            client.Send(email);
                            LogEmailNotification(email, subject);
                        }
                        finally
                        {
                            if (attachmentStreams != null)
                            {
                                attachmentStreams.ForEach(p =>
                                {
                                    p.Close();
                                    p.Dispose();
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (_throwEmailFailExceptions)
                {
                    throw;
                }
                ExceptionHandler.HandleException(ex, logTag, activity);
                return false;
            }
            return true;
        }

        public bool SendExceptionEmailNotification(Exception exception, string logTag, Activity activity)
        {
            StringBuilder message = new StringBuilder();
            message.AppendLine(exception.Message);
            if (exception.InnerException != null)
            {
                message.AppendLine(string.Format("Inner Exception : {0}", exception.InnerException.ToString()));
            }
            message.AppendLine(exception.StackTrace);
            string errorMessage = message.ToString();
            return SendEmail(EmailCategory.Error, _exceptionEmailSubject, errorMessage, null, false, null, null, logTag, activity);
        }

        private void LogEmailNotification(MailMessage email, string subject)
        {
            if (!_emailLoggingEnabled)
            {
                return;
            }
            StringBuilder logMessage = new StringBuilder();
            logMessage.AppendLine("Email notification Sent");
            logMessage.AppendLine();
            logMessage.AppendLine(string.Format("Subject: {0}", subject));
            logMessage.AppendLine();
            logMessage.AppendLine("Body:");
            logMessage.AppendLine();
            logMessage.AppendLine(email.Body);
            logMessage.AppendLine();
            logMessage.AppendLine("To:");
            email.To.ToList().ForEach(p => logMessage.AppendLine(p.Address));
            logMessage.AppendLine();
            logMessage.AppendLine("CC:");
            email.To.ToList().ForEach(p => logMessage.AppendLine(p.Address));
            logMessage.AppendLine();
            logMessage.AppendLine("BCC:");
            email.Bcc.ToList().ForEach(p => logMessage.AppendLine(p.Address));
            string logMessageText = logMessage.ToString();
            GOC.Instance.Logger.LogMessage(new LogMessage(logMessageText, LogMessageType.Information, LoggingLevel.Maximum));
        }

        private string GetEmailRecipientsCsv(MailMessage email)
        {
            StringBuilder result = new StringBuilder();
            foreach (MailAddress a in email.To.ToList())
            {
                result.Append(string.Format("{0},", a.Address));
            }
            if (result.Length > 0)
            {
                result = result.Remove(result.Length - 1, 1);
            }
            return result.ToString();
        }

        public void SendTestEmail(string logTag, Activity activity)
        {
            SendEmail(EmailCategory.Notification, "Test Email", "This is a test email.", null, false, null, null, logTag, activity);
        }

        #endregion //Methods
    }
}
