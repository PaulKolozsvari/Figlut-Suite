namespace Figlut.Server.Toolkit.Extensions.WebService.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Utilities.Serialization;
    using Figlut.Server.Toolkit.Web.Client;

    #endregion //Using Directives

    public class AfterWebInvokeSqlTableArgs : AfterWebServiceRequestArgs
    {
        #region Constructors

        public AfterWebInvokeSqlTableArgs(
            string uri,
            HttpVerb method,
            string contentType,
            string accept,
            string userAgent,
            string inputString,
            ISerializer inputSerializer,
            ISerializer outputSerializer,
            string tableName,
            List<object> inputEntities)
            : base(
                uri,
                method,
                contentType,
                accept,
                userAgent,
                inputString,
                inputSerializer,
                outputSerializer)
        {
            _tableName = tableName;
            _inputEntities = inputEntities;
        }

        #endregion //Constructors

        #region Fields

        protected string _tableName;
        protected List<object> _inputEntities;

        #endregion //Fields

        #region Properties

        public string TableName
        {
            get { return _tableName; }
        }

        public List<object> InputEntities
        {
            get { return _inputEntities; }
        }

        #endregion //Properties
    }
}
