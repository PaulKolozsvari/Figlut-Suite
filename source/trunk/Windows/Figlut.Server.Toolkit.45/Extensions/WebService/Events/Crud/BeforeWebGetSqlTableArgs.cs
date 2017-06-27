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

    public class BeforeWebGetSqlTableArgs : BeforeWebServiceRequestArgs
    {
        #region Constructors

        public BeforeWebGetSqlTableArgs(
            string uri,
            HttpVerb method,
            string contentType,
            string accept,
            string userAgent,
            ISerializer inputSerializer,
            ISerializer outputSerializer,
            string tableName,
            string filters,
            Type outputEntityType)
            : base(
                uri,
                method,
                contentType,
                accept,
                userAgent,
                null,
                inputSerializer,
                outputSerializer)
        {
            _tableName = tableName;
            _filters = filters;
            _outputEntityType = outputEntityType;
        }

        #endregion //Constructors

        #region Fields

        protected string _tableName;
        protected string _filters;
        public Type _outputEntityType;

        #endregion //Fields

        #region Properties

        public string TableName
        {
            get { return _tableName; }
        }

        public string Filters
        {
            get { return _filters; }
        }

        public Type OutputEntityType
        {
            get { return _outputEntityType; }
        }

        #endregion //Properties
    }
}