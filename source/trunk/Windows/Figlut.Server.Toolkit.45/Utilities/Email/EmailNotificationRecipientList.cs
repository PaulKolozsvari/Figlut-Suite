namespace Figlut.Server.Toolkit.Utilities.Email
{
    #region Using Directives

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class EmailNotificationRecipientList : IEnumerable<EmailNotificationRecipient>
    {
        #region Constructors

        public EmailNotificationRecipientList()
        {
            _emailNotificationRecipients = new List<EmailNotificationRecipient>();
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

        public void Add(EmailNotificationRecipient recipient)
        {
            _emailNotificationRecipients.Add(recipient);
        }

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

        #endregion //Methods
    }
}
