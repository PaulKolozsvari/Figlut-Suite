namespace OneStopShop.FiglutExtensions.WebService
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Utilities;
    using OneStopShop.FiglutExtensions.Biometric;

    #endregion //Using Directives

    public class OssWebServiceApplication
    {
        #region Singleton Setup

        private static OssWebServiceApplication _instance;

        public static OssWebServiceApplication Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new OssWebServiceApplication();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private OssWebServiceApplication()
        {
        }

        #endregion //Constructors

        #region Fields

        private MatcherHelper _matcherHelper;

        #endregion //Fields

        #region Properties

        public MatcherHelper MatcherHelper
        {
            get { return _matcherHelper; }
        }

        #endregion //Properties

        #region Methods

        internal void InitializeFingerMatcher()
        {
            _matcherHelper = new MatcherHelper();
            SqlDatabase db = GOC.Instance.GetDatabase<SqlDatabase>();
            string tableName = typeof(DeviceUser).Name;
            SqlQuery query = new SqlQuery(SqlQueryKeyword.SELECT);
            query.AppendSelectColumn<DeviceUser>(true);
            List<DeviceUser> users = db.Query<DeviceUser>(query);
            users.ForEach(p => _matcherHelper.InsertRecord(
                p.DeviceUserId.ToString(),
                p.Finger1Template,
                (FingerId)p.Finger1Id,
                p.Finger2Template,
                (FingerId)p.Finger2Id));
        }

        #endregion //Methods
    }
}
