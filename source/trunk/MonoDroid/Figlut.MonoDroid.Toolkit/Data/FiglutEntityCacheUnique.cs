namespace Figlut.MonoDroid.Toolkit.Data
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.MonoDroid.Toolkit.Utilities;
    using Figlut.MonoDroid.Toolkit.Web.Client.FiglutWebService;

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
            return SaveToServer(out result, true);
        }

        public bool SaveToServer(out string messageResult, bool wrapWebException)
        {
            HttpStatusCode statusCode;
            string statusDescription = null;
            StringBuilder result = new StringBuilder();
            string message = null;
            if (_deletedEntities.Count > 0)
            {
                message = GOC.Instance.FiglutWebServiceClient.Delete(
                    new FiglutWebServiceFilterString(base._entityType.Name, null),
                    null,
                    _deletedEntities.Values.ToList(),
                    out statusCode,
                    out statusDescription,
                    wrapWebException);
                _deletedEntities.Clear();
                result.AppendLine(statusDescription);
            }

            if (_addedEntities.Count > 0)
            {
                message = GOC.Instance.FiglutWebServiceClient.Insert(
                    new FiglutWebServiceFilterString(base._entityType.Name, null),
                    _addedEntities.Values.ToList(),
                    out statusCode,
                    out statusDescription,
                    wrapWebException);
                _addedEntities.Clear();
                result.AppendLine(statusDescription);
            }
            if (_updatedEntities.Count > 0)
            {
                message = GOC.Instance.FiglutWebServiceClient.Update(
                    new FiglutWebServiceFilterString(base._entityType.Name, null),
                    null,
                    _updatedEntities.Values.ToList(),
                    out statusCode,
                    out statusDescription,
                    wrapWebException);
                _updatedEntities.Clear();
                result.AppendLine(statusDescription);
            }
            if (result.Length < 1)
            {
                result.AppendLine("No changes to save.");
                messageResult = result.ToString();
                return false;
            }
            messageResult = result.ToString();
            return true;
        }

        #endregion //Methods
    }
}
