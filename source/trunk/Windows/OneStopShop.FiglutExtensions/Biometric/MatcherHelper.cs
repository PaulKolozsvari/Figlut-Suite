namespace OneStopShop.FiglutExtensions.Biometric
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Sagem.MorphoKit;

    #endregion //Using Directives

    public class MatcherHelper
    {
        #region Constructors

        public MatcherHelper()
        {
            _matcher = new Matcher();
        }

        #endregion //Constructors

        #region Fields

        private Matcher _matcher;

        #endregion //Fields

        #region Methods

        internal void InsertRecord(
            string recordId, 
            byte[] finger1Template, 
            FingerId finger1Id,
            byte[] finger2Template,
            FingerId finger2Id)
        {
            if (finger1Template == null || finger1Template.Length < 1)
            {
                throw new NullReferenceException(string.Format(
                    "Finger 1 Template to be inserted into {0} may not be null or have a length of 0.",
                    typeof(Matcher).FullName));
            }
            if (finger2Template == null || finger1Template.Length < 1)
            {
                throw new NullReferenceException(string.Format(
                    "Finger 2 Template to be inserted into {0} may not be null or have a length of 0.",
                    typeof(Matcher).FullName));
            }
            FingerTemplate template1 = new FingerTemplate() { Buffer = finger1Template, Id = (byte)((int)finger1Id) };
            FingerTemplate template2 = new FingerTemplate() { Buffer = finger2Template, Id = (byte)((int)finger2Id) };
            Record record = new Record() { Id = recordId };
            record.AddTemplate(template1);
            record.AddTemplate(template2);
            _matcher.Insert(record);
        }

        internal Candidate MatchRecord(byte[] fingerTemplate)
        {
            if (fingerTemplate == null || fingerTemplate.Length < 1)
            {
                throw new NullReferenceException("Finger Template to identify may not be null or have a length of 0.");
            }
            return _matcher.IdentifyTemplate(fingerTemplate);
        }

        internal Record FindRecord(string recordId)
        {
            return _matcher.Find(recordId);
        }

        public void DeleteRecord(string recordId)
        {
            _matcher.Delete(recordId);
        }

        #endregion //Methods
    }
}