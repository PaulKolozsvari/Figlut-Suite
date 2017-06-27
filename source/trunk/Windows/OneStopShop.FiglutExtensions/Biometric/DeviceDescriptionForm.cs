namespace OneStopShop.FiglutExtensions.Biometric
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

    public partial class DeviceDescriptionForm : FiglutBaseForm
    {
        #region Constructors

        public DeviceDescriptionForm(
            string productDescriptor, 
            string sensorDescriptor, 
            string softwareDescriptor)
        {
            InitializeComponent();
            String[] productlines = productDescriptor.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < productlines.Length; i++)
            {
                String[] cols = productlines[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                lblProduct1.Text += cols[0] + "\n";
                for (int j = 1; j < cols.Length; j++)
                {
                    lblProduct2.Text += cols[j] + "   ";
                }
                lblProduct2.Text += "\n";
            }

            string[] sensorlines = sensorDescriptor.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < sensorlines.Length; i++)
            {
                string[] cols = sensorlines[i].Split("\t".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                lblSensor1.Text += cols[0] + "\n";
                lblSensor2.Text += cols[1] + "\n";
            }
            gpbSensor.Location = new Point(gpbProduct.Location.X, gpbProduct.Location.Y + gpbProduct.Size.Height + 6);

            lblSoftware1.Text = softwareDescriptor;
            gpbSoftware.Location = new Point(gpbSensor.Location.X, gpbSensor.Location.Y + gpbSensor.Size.Height + 6);
            int rem = 0;
            long x = Math.DivRem(this.Size.Width, 2, out rem);

            this.Height = 
                gpbProduct.Size.Height + gpbSensor.Size.Height + gpbSoftware.Size.Height + 
                24 + //4 gaps between the group boxes.
                42 + //Sum of heights of title bar and status bar.
                24; //Height of the menu bar.
        }

        private void mnuCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void DeviceDescriptionForm_MouseDown(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseDown(sender, e);
        }

        private void DeviceDescriptionForm_MouseMove(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseMove(sender, e);
        }

        private void DeviceDescriptionForm_MouseUp(object sender, MouseEventArgs e)
        {
            base.BorderlessForm_MouseUp(sender, e);
        }

        private void DeviceDescriptionForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                mnuCancel.PerformClick();
            }
        }

        #endregion //Constructors
    }
}