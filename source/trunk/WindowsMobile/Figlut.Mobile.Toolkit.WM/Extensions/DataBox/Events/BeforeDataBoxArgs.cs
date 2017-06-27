namespace Figlut.Mobile.Toolkit.Extensions.DataBox.Events
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    #endregion //Using Directives

    public class BeforeDataBoxArgs : EventArgs
    {
        #region Properties

        public bool Cancel { get; set; }

        #endregion //Properties
    }
}
