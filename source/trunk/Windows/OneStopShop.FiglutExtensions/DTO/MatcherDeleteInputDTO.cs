namespace OneStopShop.FiglutExtensions.DTO
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    [Serializable]
    public class MatcherDeleteInputDTO
    {
        #region Constructors

        public MatcherDeleteInputDTO(string recordId)
        {
            _recordId = recordId;
        }

        #endregion //Constructors

        #region Fields

        private string _recordId;

        #endregion //Fields

        #region Properties

        public string RecordId
        {
            get { return _recordId; }
        }

        #endregion //Properties
    }
}
