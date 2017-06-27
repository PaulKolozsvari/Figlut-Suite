namespace Figlut.Server.Toolkit.Web.Service.REST
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    [ServiceContract]
    public interface IRestService
    {
        [OperationContract]
        [WebGet(UriTemplate = "*")]
        Stream AllURIs();

        [OperationContract]
        [WebGet(UriTemplate = "/{entityName}")]
        Stream GetEntities(string entityName);

        [OperationContract]
        [WebGet(UriTemplate = "/{entityName}/{entityId}")]
        Stream GetEntityById(string entityName, string entityId);

        [OperationContract]
        [WebGet(UriTemplate = "/{entityName}?searchBy={fieldName}&searchValueOf={fieldValue}")]
        Stream GetEntitiesByField(string entityName, string fieldName, string fieldValue);

        [OperationContract]
        [WebInvoke(UriTemplate = "/{entityName}", Method = "PUT")]
        Stream PutEntity(string entityName, Stream inputStream);

        [OperationContract]
        [WebInvoke(UriTemplate = "/{entityName}/{entityId}", Method = "DELETE")]
        Stream DeleteEntity(string entityName, string entityId);
    }
}
