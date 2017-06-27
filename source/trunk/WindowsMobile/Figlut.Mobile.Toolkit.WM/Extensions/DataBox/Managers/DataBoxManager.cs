namespace Figlut.Mobile.Toolkit.Extensions.DataBox.Managers
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using Figlut.Mobile.Toolkit.Data;

    #endregion //Using Directives

    public class DataBoxManager : IDisposable
    {
        #region Constructors

        public DataBoxManager(IDataBox dataBox)
        {
            _dataBox = dataBox;
            _dataBox.OnDataBoxPropertiesChanged += _dataBox_OnDataBoxPropertiesChanged;
        }

        #endregion //Constructors

        #region Fields

        private IDataBox _dataBox;
        private DataBoxPropertiesChangedArgs _dataBoxPropertiesArgs;

        #endregion //Fields

        #region Properties

        public DataBoxPropertiesChangedArgs DataBoxProperties
        {
            get { return _dataBoxPropertiesArgs; }
        }

        public IDataBox DataBox
        {
            get { return _dataBox; }
        }

        #endregion //Properties

        #region Methods

        public void Dispose()
        {
            _dataBox.OnDataBoxPropertiesChanged -= _dataBox_OnDataBoxPropertiesChanged;
        }

        #endregion //Methods

        #region Event Handlers

        private void _dataBox_OnDataBoxPropertiesChanged(object sender, DataBoxPropertiesChangedArgs e)
        {
            _dataBoxPropertiesArgs = e;
        }

        #endregion //Event Handlers
    }
}