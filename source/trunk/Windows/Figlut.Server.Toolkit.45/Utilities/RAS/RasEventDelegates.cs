namespace Figlut.Server.Toolkit.Utilities.RAS
{
    #region Using Directives

    using DotRas;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public delegate void OnConnectionStateChangedHandler(object sender, StateChangedEventArgs e);
    public delegate void OnDialCompletedHandler(object sender, DialCompletedEventArgs e);
}
