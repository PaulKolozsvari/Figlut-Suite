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
    public class MatcherdentificationOutputDTO
    {
        #region Constructors

        public MatcherdentificationOutputDTO(
            DeviceUser user)
        {
            _user = user;
        }

        #endregion //Constructors

        #region Fields

        private DeviceUser _user;

        #endregion //Fields

        #region Properties

        public DeviceUser User
        {
            get { return _user; }
        }

        #endregion //Properties
    }
}
