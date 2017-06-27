namespace Figlut.Server.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Web.Client.FiglutWebService;

    #endregion //Using Directives

    public class FiglutEntityCacheUnique : EntityCacheUnique
    {
        #region Constructors

        public FiglutEntityCacheUnique(Type entityType) : 
            base(entityType)
        {
        }

        public FiglutEntityCacheUnique(string name, string defaultFilePath, Type entityType)
            : base(name, defaultFilePath, entityType)
        {
        }

        #endregion //Constructors

        #region Methods

        public override bool SaveToServer()
        {
            string result = null;
            return SaveToServer(out result);
        }

        public bool SaveToServer(out string messageResult)
        {
            StringBuilder result = new StringBuilder();
            string message = null;
            message = GOC.Instance.FiglutWebServiceClient.Delete(
                new FiglutWebServiceFilterString(base._entityType.Name, null),
                null,
                _deletedEntities.Values.ToList());
            result.AppendLine(message);

            message = GOC.Instance.FiglutWebServiceClient.Insert(
                new FiglutWebServiceFilterString(base._entityType.Name, null),
                _addedEntities.Values.ToList());
            result.AppendLine(message);

            message = GOC.Instance.FiglutWebServiceClient.Update(
                new FiglutWebServiceFilterString(base._entityType.Name, null),
                null,
                _entities.Values.ToList());

            result.AppendLine(message);
            messageResult = result.ToString();
            return true;
        }

        #endregion //Methods
    }
}
