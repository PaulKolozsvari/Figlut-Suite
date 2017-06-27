namespace Figlut.Mobile.Toolkit.Extensions.ExtensionManaged
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Mobile.Toolkit.Data;
    using Figlut.Mobile.Toolkit.Extensions.ExtensionManaged;
    using Figlut.Mobile.Toolkit.Extensions.DataBox.Events.MainMenu;

    #endregion //Using Directives

    public class ExtensionManagedMenuItem : EntityCache<string, ExtensionManagedMenuItem>
    {
        #region Constructors

        public ExtensionManagedMenuItem(string name)
            : base(name)
        {
            _extensionManagedEntityCache = new ExtensionManagedEntityCache();
        }

        #endregion //Constructors

        #region Fields

        protected object _tag;
        protected ExtensionManagedEntityCache _extensionManagedEntityCache;

        #endregion //Fields

        #region Events

        public event OnMenuClick OnMenuClick;

        #endregion //Events

        #region Properties

        public object Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        public ExtensionManagedEntityCache ExtensionManagedEntities
        {
            get { return _extensionManagedEntityCache; }
        }

        #endregion //Properties

        #region Methods

        public void PerformOnClick(MenuClickArgs e)
        {
            if (OnMenuClick != null)
            {
                OnMenuClick(this, e);
            }
        }

        #endregion //Methods
    }
}