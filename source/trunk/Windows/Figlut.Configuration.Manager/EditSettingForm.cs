namespace Figlut.Configuration.Manager
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Figlut.Desktop.BaseUI;

    #endregion //Using Directives

    public partial class EditSettingForm : FiglutBaseForm
    {
        #region Constructors

        public EditSettingForm()
        {
            InitializeComponent();
        }

        #endregion //Constructors

        #region Event Handlers

        private void EditSettingForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void EditSettingForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void EditSettingForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        #endregion //Event Handlers
    }
}
