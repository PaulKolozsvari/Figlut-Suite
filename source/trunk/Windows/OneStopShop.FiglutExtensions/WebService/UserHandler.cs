namespace OneStopShop.FiglutExtensions.WebService
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB;
    using Figlut.Server.Toolkit.Data.DB.SQLQuery;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Extensions.WebService.Handlers;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Utilities.Logging;
    using OneStopShop.FiglutExtensions.Biometric;
    using OneStopShop.FiglutExtensions.DTO;
    using Sagem.MorphoKit;

    #endregion //Using Directives

    public class UserHandler : WebRequestToExtensionHandler
    {
        #region Constructors

        public UserHandler()
            : base(typeof(UserHandler).Name)
        {
        }

        #endregion //Constructors

        #region Methods

        public override WebRequestToExtensionOutput HandleWebRequest(WebRequestToExtensionInput input)
        {
            if (string.IsNullOrEmpty(input.CustomParameters))
            {
                throw new NullReferenceException(string.Format(
                    "{0} may not be null or empty on {1} when passed to {2}.",
                    EntityReader<WebRequestToExtensionInput>.GetPropertyName(p => p.CustomParameters, false),
                    typeof(WebRequestToExtensionInput).FullName,
                    this.GetType().FullName));
            }
            FingerOperationType operationType;
            if (!Enum.TryParse<FingerOperationType>(input.CustomParameters, true, out operationType))
            {
                throw new ArgumentException(string.Format(
                    "Could not parse {0} of value {1} to {2}.",
                    EntityReader<WebRequestToExtensionInput>.GetPropertyName(p => p.CustomParameters, false),
                    input.CustomParameters,
                    typeof(FingerOperationType).FullName));
            }
            switch (operationType)
            {
                case FingerOperationType.Identification:
                    return IdentifyUser(input);
                case FingerOperationType.Insert:
                    return InsertRecord(input);
                case FingerOperationType.Update:
                    return UpdateUser(input);
                case FingerOperationType.Delete:
                    return DeleteUser(input);
                default:
                    throw new ArgumentException(string.Format(
                        "Invalid {0} of value {1}.", 
                        typeof(FingerOperationType).FullName, 
                        operationType.ToString()));
            }
        }

        private WebRequestToExtensionOutput IdentifyUser(WebRequestToExtensionInput input)
        {
            MatcherIdentificationInputDTO idInput = (MatcherIdentificationInputDTO)input.InputSerializer.DeserializeFromText(
                typeof(MatcherIdentificationInputDTO),
                input.InputString);
            Candidate candidate = OssWebServiceApplication.Instance.MatcherHelper.MatchRecord(idInput.FingerTemplate);
            if (candidate == null || string.IsNullOrEmpty(candidate.Id))
            {
                return new WebRequestToExtensionOutput(HttpStatusCode.NotFound, "Fingerprint does not match any user.", null);
            }
            Guid userId = Guid.Parse(candidate.Id);
            SqlDatabase db = GOC.Instance.GetDatabase<SqlDatabase>();
            SqlQuery query = new SqlQuery(SqlQueryKeyword.SELECT);
            query.AppendSelectColumn<DeviceUser>(true);
            query.AppendWhereColumns(new List<WhereClauseColumn>()
            {
                new WhereClauseColumn(
                    EntityReader<DeviceUser>.GetPropertyName(p => p.DeviceUserId, false),
                    new WhereClauseComparisonOperator(ComparisonOperator.EQUALS),
                    userId,
                    true,
                    true)
            });
            List<DeviceUser> users = db.Query<DeviceUser>(query);
            DeviceUser resultUser = users.Count < 1 ? null : users[0];
            if (resultUser == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with {1} of {2}.",
                    typeof(DeviceUser).FullName,
                    EntityReader<DeviceUser>.GetPropertyName(p => p.DeviceUserId, false),
                    candidate.Id));
            }
            string outputString = input.OutputSerializer.SerializeToText(new MatcherdentificationOutputDTO(resultUser));
            return new WebRequestToExtensionOutput(
                HttpStatusCode.OK,
                string.Format("Identification of {0} {1} successful.", resultUser.Name, resultUser.Surname),
                outputString);
        }

        private WebRequestToExtensionOutput InsertRecord(WebRequestToExtensionInput input)
        {
            MatcherInsertInputDTO insertInput = (MatcherInsertInputDTO)input.InputSerializer.DeserializeFromText(
                typeof(MatcherInsertInputDTO),
                input.InputString);
            if (OssWebServiceApplication.Instance.MatcherHelper.FindRecord(insertInput.RecordId) != null)
            {
                throw new ArgumentException(string.Format("Record with ID {0} already added to matcher.", insertInput.RecordId));
            }
            OssWebServiceApplication.Instance.MatcherHelper.InsertRecord(
                insertInput.RecordId,
                insertInput.Finger1Template,
                insertInput.Finger1Id,
                insertInput.Finger2Template,
                insertInput.Finger2Id);
            return new WebRequestToExtensionOutput(HttpStatusCode.Created, "Record inserted into matcher successfully.", null);
        }

        private WebRequestToExtensionOutput UpdateUser(WebRequestToExtensionInput input)
        {
            MatcherUpdateInputDTO updateInput = (MatcherUpdateInputDTO)input.InputSerializer.DeserializeFromText(
                typeof(MatcherUpdateInputDTO),
                input.InputString);
            Record record = OssWebServiceApplication.Instance.MatcherHelper.FindRecord(updateInput.RecordId);
            if (record == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find record with ID {0} in matcher to be updated.", 
                    updateInput.RecordId));
            }
            OssWebServiceApplication.Instance.MatcherHelper.DeleteRecord(updateInput.RecordId);
            OssWebServiceApplication.Instance.MatcherHelper.InsertRecord(
                updateInput.RecordId,
                updateInput.Finger1Template,
                updateInput.Finger1Id,
                updateInput.Finger2Template,
                updateInput.Finger2Id);
            return new WebRequestToExtensionOutput(HttpStatusCode.OK, "Record updated in matcher successfully.", null);
        }

        private WebRequestToExtensionOutput DeleteUser(WebRequestToExtensionInput input)
        {
            MatcherDeleteInputDTO deleteInput = (MatcherDeleteInputDTO)input.InputSerializer.DeserializeFromText(
                typeof(MatcherDeleteInputDTO),
                input.InputString);
            Record record = OssWebServiceApplication.Instance.MatcherHelper.FindRecord(deleteInput.RecordId);
            if (record == null)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find record with ID in matcher to be deleted.",
                    deleteInput.RecordId));
            }
            OssWebServiceApplication.Instance.MatcherHelper.DeleteRecord(deleteInput.RecordId);
            return new WebRequestToExtensionOutput(HttpStatusCode.OK, "Record deleted in matcher successfully.", null);
        }

        #endregion //Methods
    }
}
