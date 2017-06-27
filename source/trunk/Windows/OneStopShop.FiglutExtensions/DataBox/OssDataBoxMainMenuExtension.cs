namespace OneStopShop.FiglutExtensions.DataBox
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Extensions.DataBox;
    using Figlut.Server.Toolkit.Extensions.DataBox.Events.MainMenu;
    using Figlut.Server.Toolkit.Extensions.ExtensionManaged;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Web.Client.FiglutWebService;
    using OneStopShop.FiglutExtensions.Biometric;
    using OneStopShop.FiglutExtensions.DTO;
    using OneStopShop.FiglutExtensions.WebService;

    #endregion //Using Directives

    public class OssDataBoxMainMenuExtension : DataBoxMainMenuExtension
    {
        #region Constructors

        public OssDataBoxMainMenuExtension()
        {
        }

        #endregion //Constructors

        #region Fields

        private ExtensionManagedMenuItem _optionsMenu;
        private ExtensionManagedMenuItem _identifyMenu;

        #endregion //Fields

        #region Methods

        public override void AddExtensionManagedMenuItems()
        {
            _optionsMenu = new ExtensionManagedMenuItem("Options");
            _optionsMenu.ExtensionManagedEntities.AddExtensionManagedEntity(typeof(DeviceUser).FullName);
            _identifyMenu = new ExtensionManagedMenuItem("Identify User");
            _identifyMenu.ExtensionManagedEntities.AddExtensionManagedEntity(typeof(DeviceUser).FullName);
            _optionsMenu.Add(_identifyMenu);
            ExtensionManagedMainMenu.Add(_optionsMenu);

        }

        public override void SubscribeToAddedMenuItemsEvents()
        {
            _identifyMenu.OnMenuClick += _identifyMenu_OnMenuClick;
        }

        private void PutUserInUpdateMode(DeviceUser user)
        {
            Dictionary<string, object> filterProperties = new Dictionary<string, object>();
            filterProperties.Add(EntityReader<DeviceUser>.GetPropertyName(p => p.DeviceUserId, false), user.DeviceUserId);
            Dictionary<Guid, object> entities = DataBoxManager.DataBoxProperties.CurrentEntityCache.GetEntitiesByProperties(filterProperties, true);
            if (entities.Count < 1)
            {
                throw new NullReferenceException(string.Format(
                    "Could not find {0} with {1} of {2}.",
                    typeof(DeviceUser).FullName,
                    EntityReader<DeviceUser>.GetPropertyName(p => p.DeviceUserId, false),
                    user.DeviceUserId));
            }
            if (DataBoxManager.DataBoxProperties.InUpdateMode)
            {
                DataBoxManager.DataBox.CancelEntityUpdate();
            }
            KeyValuePair<Guid, object> dataBoxUser = entities.First();
            DataBoxManager.DataBox.SelectEntity(dataBoxUser.Key);
            DataBoxManager.DataBox.PrepareForEntityUpdate();
        }

        #endregion //Methods

        #region Event Handlers

        private void _identifyMenu_OnMenuClick(object sender, MenuClickArgs e)
        {
            using (WaitProcess w = new WaitProcess())
            {
                MatcherIdentificationInputDTO input = null;
                using (FingerScanForm f = new FingerScanForm(false, FingerId.RightThumb, "Identify Fingerprint"))
                {
                    if (f.ShowDialog() == DialogResult.Cancel)
                    {
                        return;
                    }
                    input = new MatcherIdentificationInputDTO(
                        f.TemplateResult,
                        f.FingerIdResult,
                        f.AcquisitionImageBytesResult);
                }
                string rawOutput = null;
                HttpStatusCode statusCode = HttpStatusCode.NotFound;
                string statusDescription = null;
                MatcherdentificationOutputDTO output = GOC.Instance.FiglutWebServiceClient.CallExtension<MatcherdentificationOutputDTO>(
                    typeof(UserHandler).Name,
                    FingerOperationType.Identification.ToString(),
                    input,
                    true,
                    true,
                    out rawOutput,
                    out statusCode,
                    out statusDescription,
                    true);
                switch (statusCode)
                {
                    case HttpStatusCode.OK:
                        PutUserInUpdateMode(output.User);
                        break;
                    case HttpStatusCode.NotFound:
                        UIHelper.DisplayError("No user with a matching fingerprint found.");
                        break;
                    default:
                        throw new UserThrownException(string.Format(
                            "Unexpected HTTP status code {0} : {1} on fingerprint identification.",
                            statusCode.ToString(),
                            (int)statusCode));
                }
            }
        }



        #endregion //Event Handlers
    }
}