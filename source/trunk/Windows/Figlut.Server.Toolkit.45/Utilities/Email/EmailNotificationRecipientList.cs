namespace Figlut.Server.Toolkit.Utilities.Email
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;

    #endregion //Using Directives

    public class EmailNotificationRecipientList : IEnumerable<EmailNotificationRecipient>
    {
        #region Constructors

        public EmailNotificationRecipientList()
        {
            _emailNotificationRecipients = new List<EmailNotificationRecipient>();
        }

        public EmailNotificationRecipientList(List<EmailNotificationRecipient> emailNotificationRecipientList)
        {
            _emailNotificationRecipients = emailNotificationRecipientList;
        }

        #endregion //Constructors

        #region Fields

        private List<EmailNotificationRecipient> _emailNotificationRecipients;

        #endregion //Fields

        #region Properties

        public List<EmailNotificationRecipient> EmailNotificationRecipients
        {
            get { return _emailNotificationRecipients; }
        }

        #endregion //Properties

        #region Methods

        public IEnumerator<EmailNotificationRecipient> GetEnumerator()
        {
            return _emailNotificationRecipients.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < _emailNotificationRecipients.Count; i++)
            {
                EmailNotificationRecipient r = _emailNotificationRecipients[i];
                result.Append(r.ToString());
                if (i < (_emailNotificationRecipients.Count - 1))
                {
                    result.Append(",");
                }
            }
            return result.ToString();
        }

        public void Add(EmailNotificationRecipient recipient)
        {
            DataValidator.ValidateObjectNotNull(recipient, nameof(recipient), parentName: null);
            DataValidator.ValidateStringNotEmptyOrNull(recipient.EmailAddress, recipient.EmailAddress, parentName: null);
            DataValidator.ValidateStringNotEmptyOrNull(recipient.DisplayName, nameof(recipient.DisplayName), parentName: null);
            if (EmailAddressExists(recipient.EmailAddress))
            {
                throw new ArgumentException($"Recipient with Email Address '{recipient.EmailAddress}' already exists.");
            }
            _emailNotificationRecipients.Add(recipient);
        }

        public bool EmailAddressExists(string emailAddress)
        {
            return GetByEmailAddress(emailAddress, throwExceptionOnNotFound: false) != null;
        }

        public bool RemoveByEmailAddress(string emailAddress, bool throwExceptionOnNotFound)
        {
            EmailNotificationRecipient e = GetByEmailAddress(emailAddress, throwExceptionOnNotFound);
            if (e == null)
            {
                return false;
            }
            return _emailNotificationRecipients.Remove(e);
        }

        public EmailNotificationRecipient GetByEmailAddress(string emailAddress, bool throwExceptionOnNotFound)
        {
            EmailNotificationRecipient result = _emailNotificationRecipients.Where(p => p.EmailAddress.ToLower().Equals(emailAddress, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
            if (result == null && throwExceptionOnNotFound)
            {
                throw new NullReferenceException($"No {nameof(EmailNotificationRecipient)} with {nameof(EmailNotificationRecipient.EmailAddress)} of {emailAddress}.");
            }
            return result;
        }

        public EmailNotificationRecipientList Clone()
        {
            EmailNotificationRecipientList result = new EmailNotificationRecipientList();
            foreach (EmailNotificationRecipient e in _emailNotificationRecipients)
            {
                result.Add(new EmailNotificationRecipient()
                {
                    EmailAddress = e.EmailAddress,
                    DisplayName = e.DisplayName
                });
            }
            return result;
        }

        #endregion //Methods
    }
}
