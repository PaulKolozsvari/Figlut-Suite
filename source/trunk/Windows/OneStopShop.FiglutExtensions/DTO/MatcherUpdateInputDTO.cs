namespace OneStopShop.FiglutExtensions.DTO
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using OneStopShop.FiglutExtensions.Biometric;

    #endregion //Using Directives

    [Serializable]
    public class MatcherUpdateInputDTO
    {
        #region Constants

        public MatcherUpdateInputDTO(
            string recordId,
            byte[] finger1Template,
            FingerId finger1Id,
            byte[] finger2Template,
            FingerId finger2Id)
        {
            _recordId = recordId;
            _finger1Template = finger1Template;
            _finger1Id = finger1Id;
            _finger2Template = finger2Template;
            _finger2Id = finger2Id;
        }

        #endregion //Constants

        #region Fields

        private string _recordId;
        private byte[] _finger1Template;
        private FingerId _finger1Id;
        private byte[] _finger2Template;
        private FingerId _finger2Id;

        #endregion //Fields

        #region Properties

        public string RecordId
        {
            get { return _recordId; }
        }

        public byte[] Finger1Template
        {
            get { return _finger1Template; }
        }

        public FingerId Finger1Id
        {
            get { return _finger1Id; }
        }

        public byte[] Finger2Template
        {
            get { return _finger2Template; }
        }

        public FingerId Finger2Id
        {
            get { return _finger2Id; }
        }

        #endregion //Properties
    }
}