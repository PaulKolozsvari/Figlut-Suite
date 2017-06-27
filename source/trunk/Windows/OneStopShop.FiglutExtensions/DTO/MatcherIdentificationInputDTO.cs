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
    public class MatcherIdentificationInputDTO
    {
        #region Constructors

        public MatcherIdentificationInputDTO(
            byte[] fingerTemplate,
            FingerId fingerId,
            byte[] fingerImageBytes)
        {
            _fingerTemplate = fingerTemplate;
            _fingerId = fingerId;
            _fingerImageBytes = fingerImageBytes;
        }

        #endregion //Constructors

        #region Fields

        private byte[] _fingerTemplate;
        private FingerId _fingerId;
        private byte[] _fingerImageBytes;

        #endregion //Fields

        #region Properties

        public byte[] FingerTemplate
        {
            get { return _fingerTemplate; }
        }

        public FingerId FingerId
        {
            get { return _fingerId; }
        }

        public byte[] FingerImageBytes
        {
            get { return _fingerImageBytes; }
        }

        #endregion //Properties
    }
}
