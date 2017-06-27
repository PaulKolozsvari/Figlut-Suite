namespace Figlut.Mobile.Toolkit.Data.DB.SyncService
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Drawing;
    using System.Windows.Forms;
    using Microsoft.Synchronization;

    #endregion //Using Directives

    public class SyncFormConfig
    {
        #region Constructors

        public SyncFormConfig(
            MobileDatabase mobileDatabase,
            SyncConfig syncConfig,
            Image logo,
            PictureBoxSizeMode logoSizeMode,
            bool syncOnFormLoad,
            bool displaySyncStatisticsAfterSync,
            bool closeAfterSync)
        {
            _mobileDatabase = mobileDatabase;
            _syncConfig = syncConfig;
            _logo = logo;
            _logoSizeMode = logoSizeMode;
            _syncOnFormLoad = syncOnFormLoad;
            _displaySyncStatisticsAfterSync = displaySyncStatisticsAfterSync;
            _closeAfterSync = closeAfterSync;
        }

        #endregion //Constructors

        #region Fields

        protected MobileDatabase _mobileDatabase;
        protected SyncConfig _syncConfig;
        protected Image _logo;
        protected PictureBoxSizeMode _logoSizeMode;
        protected bool _syncOnFormLoad;
        protected bool _displaySyncStatisticsAfterSync;
        protected bool _closeAfterSync;

        #endregion //Fields

        #region Properties

        public MobileDatabase MobileDatabase
        {
            get { return _mobileDatabase; }
        }

        public SyncConfig SyncConfig
        {
            get { return _syncConfig; }
            set { _syncConfig = value; }
        }

        public Image Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }

        public PictureBoxSizeMode LogoSizeMode
        {
            get { return _logoSizeMode; }
            set { _logoSizeMode = value; }
        }

        public bool SyncOnFormLoad
        {
            get { return _syncOnFormLoad; }
            set { _syncOnFormLoad = value; }
        }

        public bool DisplaySyncStatisticsAfterSync
        {
            get { return _displaySyncStatisticsAfterSync; }
            set { _displaySyncStatisticsAfterSync = value; }
        }

        public bool CloseAfterSync
        {
            get { return _closeAfterSync; }
            set { _closeAfterSync = value; }
        }

        #endregion //Properties
    }
}