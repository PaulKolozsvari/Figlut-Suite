namespace OneStopShop.FiglutExtensions.DataBox
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Server.Toolkit.Data;
    using Figlut.Server.Toolkit.Data.DB.SQLServer;
    using Figlut.Server.Toolkit.Extensions;
    using Figlut.Server.Toolkit.Extensions.ExtensionManaged;
    using Figlut.Server.Toolkit.Extensions.DataBox;
    using Figlut.Server.Toolkit.Extensions.DataBox.Events;
    using Figlut.Server.Toolkit.Extensions.DataBox.Events.Crud;
    using Figlut.Server.Toolkit.Utilities;
    using Figlut.Server.Toolkit.Winforms;
    using OneStopShop.FiglutExtensions.Biometric;

    #endregion //Using Directives

    public class OssDataBoxCrudInterceptor : DataBoxCrudInterceptor
    {
        #region Constructors

        public OssDataBoxCrudInterceptor()
        {
        }

        #endregion //Constructors

        #region Event Handlers

        private void OneStopShopCrudExtension_OnBeforeAdd(object sender, BeforeCrudOperationArgs e)
        {
            if (!IsEntityToBeManaged(e.Entity) || !IsEntityOfType<DeviceUser>(e.Entity))
            {
                return;
            }
            DeviceUser user = EntityReader<object>.ConvertTo<DeviceUser>(e.Entity);
            if (!EnrolFingerprint(user))
            {
                e.CancelDefaultBindingBehaviour = true;
                e.Cancel = true;
                return;
            }
            EntityReader.CopyProperties(user, e.Entity, false);
            return;
        }

        private void OneStopShopCrudExtension_OnBeforeUpdate(object sender, BeforeCrudOperationArgs e)
        {
            if (!IsEntityToBeManaged(e.Entity) ||
                !IsEntityOfType<DeviceUser>(e.Entity) ||
                UIHelper.AskQuestion("Re-enrol fingerprint?") == DialogResult.No)
            {
                return;
            }
            DeviceUser user = EntityReader<object>.ConvertTo<DeviceUser>(e.Entity);
            if (!EnrolFingerprint(user))
            {
                e.CancelDefaultBindingBehaviour = true;
                e.Cancel = true;
                return;
            }
            EntityReader.CopyProperties(user, e.Entity, false);
            return;
        }

        #endregion //Event Handlers

        #region Methods

        public override void AddExtensionManagedEntities()
        {
            ExtensionManagedEntity managedEntity = ExtensionManagedEntities.AddExtensionManagedEntity(typeof(DeviceUser).FullName);
            managedEntity.AddExtensionManagedProperty(EntityReader<DeviceUser>.GetPropertyName(p => p.Finger1Template, false));
            managedEntity.AddExtensionManagedProperty(EntityReader<DeviceUser>.GetPropertyName(p => p.Finger1Id, false));
            managedEntity.AddExtensionManagedProperty(EntityReader<DeviceUser>.GetPropertyName(p => p.Finger1Image, false));
            managedEntity.AddExtensionManagedProperty(EntityReader<DeviceUser>.GetPropertyName(p => p.Finger2Template, false));
            managedEntity.AddExtensionManagedProperty(EntityReader<DeviceUser>.GetPropertyName(p => p.Finger2Id, false));
            managedEntity.AddExtensionManagedProperty(EntityReader<DeviceUser>.GetPropertyName(p => p.Finger2Image, false));
            managedEntity.AddExtensionManagedProperty(EntityReader<DeviceUser>.GetPropertyName(p => p.Device1CheckoutDate, false));
            managedEntity.AddExtensionManagedProperty(EntityReader<DeviceUser>.GetPropertyName(p => p.Device2CheckoutDate, false));
            managedEntity.AddExtensionManagedProperty(EntityReader<DeviceUser>.GetPropertyName(p => p.Device3CheckoutDate, false));
        }

        public override void SubscribeToCrudEvents()
        {
            this.OnBeforeUpdate += OneStopShopCrudExtension_OnBeforeUpdate;
            this.OnBeforeAdd += OneStopShopCrudExtension_OnBeforeAdd;
        }

        private bool EnrolFingerprint(DeviceUser user)
        {
            using (FingerScanForm f = new FingerScanForm(true, FingerId.LeftThumb, "Enrol 1st Fingerprint")) //Finger1
            {
                if (f.ShowDialog() == DialogResult.Cancel)
                {
                    return false;
                }
                user.Finger1Template = f.TemplateResult;
                user.Finger1Id = (int)f.FingerIdResult;
                user.Finger1Image = f.AcquisitionImageBytesResult;
            }
            using (FingerScanForm f = new FingerScanForm(true, FingerId.RightThumb, "Enrol 2nd Fingerprint")) //Finger1
            {
                if (f.ShowDialog() == DialogResult.Cancel)
                {
                    user.Finger1Template = null;
                    user.Finger1Id = 0;
                    user.Finger1Image = null;
                    return false;
                }
                user.Finger2Template = f.TemplateResult;
                user.Finger2Id = (int)f.FingerIdResult;
                user.Finger2Image = f.AcquisitionImageBytesResult;
            }
            return true;
        }

        #endregion //Methods
    }
}