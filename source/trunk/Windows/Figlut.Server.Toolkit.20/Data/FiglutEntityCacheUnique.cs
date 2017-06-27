namespace Figlut.Server.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
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
            object[] deletedEntities = new object[_deletedEntities.Count];
            _deletedEntities.Values.CopyTo(deletedEntities, 0);
            message = GOC.Instance.FiglutWebServiceClient.Delete(
                new FiglutWebServiceFilterString(base._entityType.Name, null),
                null,
                deletedEntities);
            result.AppendLine(message);

            object[] addedEntities = new object[_addedEntities.Count];
            _addedEntities.Values.CopyTo(addedEntities, 0);
            message = GOC.Instance.FiglutWebServiceClient.Insert(
                new FiglutWebServiceFilterString(base._entityType.Name, null),
                addedEntities);
            result.AppendLine(message);

            object[] entities = new object[_entities.Count];
            _entities.Values.CopyTo(entities, 0);
            message = GOC.Instance.FiglutWebServiceClient.Update(
                new FiglutWebServiceFilterString(base._entityType.Name, null),
                null,
                entities);

            result.AppendLine(message);
            messageResult = result.ToString();
            return true;
        }

        #endregion //Methods
    }
}
