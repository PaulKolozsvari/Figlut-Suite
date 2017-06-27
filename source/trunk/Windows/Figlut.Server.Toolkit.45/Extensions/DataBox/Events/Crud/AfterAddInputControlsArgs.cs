namespace Figlut.Server.Toolkit.Extensions.DataBox.Events.Crud
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    #endregion //Using Directives

    public class AfterAddInputControlsArgs : AfterDataBoxArgs
    {
        #region Constructors

        public AfterAddInputControlsArgs(
            Dictionary<string, Control> inputControls,
            bool inputControlLabelsIncluded,
            Control firstInputControl)
        {
            _inputControls = inputControls;
            _inputControlLabelsIncluded = inputControlLabelsIncluded;
            _firstInputControl = firstInputControl;
        }

        #endregion //Constructors

        #region Fields

        protected Dictionary<string, Control> _inputControls;
        protected bool _inputControlLabelsIncluded;
        protected Control _firstInputControl;

        #endregion //Fields

        #region Properties

        public Dictionary<string, Control> InputControls
        {
            get { return _inputControls; }
        }

        public bool InputControlLabelsIncluded
        {
            get { return _inputControlLabelsIncluded; }
        }

        public Control FirstInputControl
        {
            get { return _firstInputControl; }
        }

        #endregion //Properties
    }
}
