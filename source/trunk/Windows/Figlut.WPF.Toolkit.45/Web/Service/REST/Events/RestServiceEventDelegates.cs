namespace Figlut.Server.Toolkit.Web.Service.REST.Events
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    #region Delegates

    public delegate void OnBeforeGetEntitiesHandler(object sender, RestServiceGetEntitiesEventArgs e);
    public delegate void OnAfterGetEntitiesHandler(object sender, RestServiceGetEntitiesEventArgs e);

    public delegate void OnBeforeGetEntityByIdHandler(object sender, RestServiceGetEntityByIdEventArgs e);
    public delegate void OnAfterGetEntityByIdHandler(object sender, RestServiceGetEntityByIdEventArgs e);

    public delegate void OnBeforeGetEntitiesByFieldHandler(object sender, RestServiceGetEntitiesByFieldEventArgs e);
    public delegate void OnAfterGetEntitiesByFieldHandler(object sender, RestServiceGetEntitiesByFieldEventArgs e);

    public delegate void OnBeforePutEntityHandler(object sender, RestServicePutEntityEventArgs e);
    public delegate void OnAfterPutEntityHandler(object sender, RestServicePutEntityEventArgs e);

    public delegate void OnBeforeDeleteEntityHandler(object sender, RestServiceDeleteEntityEventArgs e);
    public delegate void OnAfterDeleteEntityHandler(object sender, RestServiceDeleteEntityEventArgs e);

    #endregion //Delegates
}
