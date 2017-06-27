namespace Figlut.Server.Toolkit.Winforms
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    #endregion //Using Directives

    public class NumericTextBox : TextBox
    {
        #region Constructors

        public NumericTextBox()
        {
            //Value = 0;
        }

        #endregion //Constructors

        #region Properties

        public bool AllowDecimal { get; set; }

        public double Value
        {
            get
            {
                try
                {
                    if (string.IsNullOrEmpty(Text))
                    {
                        return 0;
                    }
                    if (AllowDecimal)
                    {
                        return double.Parse(Text);
                    }
                    return int.Parse(Text);
                }
                catch (Exception)
                {
                    return -1;
                }
            }
            set
            {
                if (AllowDecimal)
                {
                    Text = value.ToString();
                }
                else
                {
                    Text = Convert.ToInt32(value).ToString();
                }
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if ((!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.') ||
                (e.KeyChar == '.' && Text.Contains('.')) ||
                (e.KeyChar == '.' && !AllowDecimal))
            {
                e.Handled = true;
            }
            base.OnKeyPress(e);
        }

        #endregion //Properties
    }
}