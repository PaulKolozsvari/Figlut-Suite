namespace OneStopShop.FiglutExtensions.WebService
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Extensions.ExtensionManaged;
    using Figlut.Server.Toolkit.Extensions.WebService;
    using Figlut.Server.Toolkit.Extensions.WebService.Events.Crud;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using Figlut.Server.Toolkit.Web.Client;
    using OneStopShop.FiglutExtensions.Biometric;
    using Sagem.MorphoKit;

    #endregion //Using Directives

    public class OssWebServiceCrudInterceptor : WebServiceCrudInterceptor
    {
        #region Event Handlers

        private void OssWebServiceCrudInterceptor_OnBeforeWebInvokeSqlTable(object sender, BeforeWebInvokeSqlTableArgs e)
        {
            if (!base.IsEntityToBeManaged(e.InputEntityType.FullName) || !IsEntityOfType<DeviceUser>(e.InputEntityType.FullName))
            {
                return;
            }
            switch (e.Method)
            {
                case HttpVerb.POST:
                    HandleInterceptOnPost(e);
                    break;
                case HttpVerb.PUT:
                    HandleInterceptOnPut(e);
                    break;
                case HttpVerb.DELETE:
                    HandleInterceptOnDelete(e);
                    break;
                default:
                    throw new UserThrownException(
                        string.Format("Unexpected web request method {0}.", e.Method.ToString()),
                        LoggingLevel.Minimum,
                        false);
            }
        }

        #endregion //Event Handlers

        #region Methods

        public override void AddExtensionManagedEntities()
        {
            ExtensionManagedEntity managedEntity = ExtensionManagedEntities.AddExtensionManagedEntity(typeof(DeviceUser).FullName); //DeviceUser class is from the generated ORM dll.
        }

        public override void SubscribeToCrudEvents()
        {
            OnBeforeWebInvokeSqlTable += OssWebServiceCrudInterceptor_OnBeforeWebInvokeSqlTable;
        }

        private void HandleInterceptOnPost(BeforeWebInvokeSqlTableArgs e)
        {
            foreach (object entity in e.InputEntities)
            {
                DeviceUser user = EntityReader<object>.ConvertTo<DeviceUser>(entity);
                string recordId = user.DeviceUserId.ToString();
                FingerId finger1Id = (FingerId)user.Finger1Id;
                FingerId finger2Id = (FingerId)user.Finger2Id;
                ValidateUser(recordId, finger1Id, finger2Id);
                OssWebServiceApplication.Instance.MatcherHelper.InsertRecord(
                    recordId,
                    user.Finger1Template,
                    finger1Id,
                    user.Finger2Template,
                    finger2Id);
            }
        }

        private void HandleInterceptOnPut(BeforeWebInvokeSqlTableArgs e)
        {
            foreach (object entity in e.InputEntities)
            {
                DeviceUser user = EntityReader<object>.ConvertTo<DeviceUser>(entity);
                string recordId = user.DeviceUserId.ToString();
                FingerId finger1Id = (FingerId)user.Finger1Id;
                FingerId finger2Id = (FingerId)user.Finger2Id;
                ValidateUser(recordId, finger1Id, finger2Id);
                Record record = OssWebServiceApplication.Instance.MatcherHelper.FindRecord(recordId);
                if (record == null)
                {
                    throw new NullReferenceException(string.Format(
                        "Could not find record with ID {0} in {1} to be updated.",
                        recordId,
                        typeof(MatcherHelper).FullName));
                }
                OssWebServiceApplication.Instance.MatcherHelper.DeleteRecord(recordId);
                OssWebServiceApplication.Instance.MatcherHelper.InsertRecord(
                    recordId,
                    user.Finger1Template,
                    finger1Id,
                    user.Finger2Template,
                    finger2Id);
            }
        }

        private void HandleInterceptOnDelete(BeforeWebInvokeSqlTableArgs e)
        {
            foreach (object entity in e.InputEntities)
            {
                DeviceUser user = EntityReader<object>.ConvertTo<DeviceUser>(entity);
                string recordId = user.DeviceUserId.ToString();
                FingerId finger1Id = (FingerId)user.Finger1Id;
                FingerId finger2Id = (FingerId)user.Finger2Id;
                ValidateUser(recordId, finger1Id, finger2Id);
                Record record = OssWebServiceApplication.Instance.MatcherHelper.FindRecord(recordId);
                if (record == null)
                {
                    throw new NullReferenceException(string.Format(
                        "Could not find record with ID {0} in {1} to be deleted.",
                        recordId,
                        typeof(MatcherHelper).FullName));
                }
                OssWebServiceApplication.Instance.MatcherHelper.DeleteRecord(recordId);
            }
        }

        private void ValidateUser(string recordId, FingerId finger1Id, FingerId finger2Id)
        {
            if (string.IsNullOrEmpty(recordId))
            {
                throw new UserThrownException(
                    string.Format("{0} of {1} may not be null or empty when using it in {2}.",
                    EntityReader<DeviceUser>.GetPropertyName(p => p.DeviceUserId, false),
                    typeof(DeviceUser).FullName,
                    typeof(MatcherHelper).FullName));
            }
            if (finger1Id == FingerId.None)
            {
                throw new UserThrownException(
                    string.Format("{0} of {1} may not be none when when using it in {2}.",
                    EntityReader<DeviceUser>.GetPropertyName(p => p.Finger1Id, false),
                    typeof(DeviceUser).FullName,
                    typeof(MatcherHelper).FullName));
            }
            if (finger2Id == FingerId.None)
            {
                throw new UserThrownException(
                    string.Format("{0} of {1} may not be none when when using it in {2}.",
                    EntityReader<DeviceUser>.GetPropertyName(p => p.Finger2Id, false),
                    typeof(DeviceUser).FullName,
                    typeof(MatcherHelper).FullName));
            }
        }

        #endregion //Methods
    }
}
