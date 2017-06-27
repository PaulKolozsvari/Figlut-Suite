namespace Figlut.Desktop.DataBox
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Windows.Forms;
    using System.Threading;
    using System.Reflection;
    using Figlut.Server.Toolkit.Winforms;
    using Figlut.Desktop.DataBox.Controls;
    using Figlut.Desktop.BaseUI;
    using Figlut.Desktop.DataBox.Utilities;

    #endregion //Using Directives

    public partial class SplashForm : FiglutBaseForm
    {
        #region Inner Types

        private delegate void UpdateFormText();

        #endregion //Inner Types

        #region Constructors

        public SplashForm()
        {
            InitializeComponent();
            if (!string.IsNullOrEmpty(FiglutDataBoxApplication.Instance.ApplicationTitle))
            {
                this.FormTitle = lblApplicationTitle.Text = FiglutDataBoxApplication.Instance.ApplicationTitle;
            }
            else
            {
                lblApplicationTitle.Text = AssemblyTitle;
            }
            if(!string.IsNullOrEmpty(FiglutDataBoxApplication.Instance.ApplicationVersion))
            {
                lblVersion.Text = FiglutDataBoxApplication.Instance.ApplicationVersion;
            }
            else
            {
                lblVersion.Text = string.Format("Version {0}", AssemblyVersion);
            }
            if(FiglutDataBoxApplication.Instance.SplashScreenImage != null)
            {
                picLogo.Image = FiglutDataBoxApplication.Instance.SplashScreenImage;
            }
            progressMain.ProgressStartColor = FiglutDataBoxApplication.Instance.ThemeStartColor;
            progressMain.ProgressEndColor = FiglutDataBoxApplication.Instance.ThemeEndColor;
        }

        #endregion //Constructors

        #region Constants

        private int TIMER_TEXT_ANIMATOR_INTERVAL = 50;

        #endregion //Constants

        #region Fields

        private System.Timers.Timer _timerTextAnimator; //Runs tick (elapsed) event handlers on a separate thread, but only on one new thread.
        private string _currentText;
        private UpdateFormText _updateFormText;
        private object _lock = new object();
        private int _currentLabelToAnimateIndex;
        private List<Label> _labelsToAnimate = new List<Label>();
        private List<string> _originalLabelTexts = new List<string>();

        #endregion //Fields

        #region Properties

        public string AssemblyTitle
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyTitleAttribute), false);
                if (attributes.Length > 0)
                {
                    AssemblyTitleAttribute titleAttribute = (AssemblyTitleAttribute)attributes[0];
                    if (titleAttribute.Title != "")
                    {
                        return titleAttribute.Title;
                    }
                }
                return System.IO.Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().CodeBase);
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            _updateFormText = null;
            Cursor.Current = Cursors.Default;
            base.Dispose(disposing);
        }

        #endregion //Methods

        #region Event Handlers

        private void SplashForm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Application.DoEvents();
            tmrMain.Enabled = true;

            Status = "Please wait ...";

            _currentLabelToAnimateIndex = 0;
            _currentText = string.Empty;
            _labelsToAnimate.Add(lblApplicationTitle);
            _labelsToAnimate.Add(lblVersion);
            foreach (Label l in _labelsToAnimate)
            {
                _originalLabelTexts.Add(l.Text);
                l.Text = string.Empty;
            }

            _updateFormText = new UpdateFormText(UpdateFormTextHandler);
            _timerTextAnimator = new System.Timers.Timer(TIMER_TEXT_ANIMATOR_INTERVAL);
            _timerTextAnimator.AutoReset = false; //Only fires the tick event once. The timer needs to then be restarted in the tick event handler.
            _timerTextAnimator.Elapsed += new System.Timers.ElapsedEventHandler(_timerTextAnimator_Elapsed);
            _timerTextAnimator.Start();
        }

        private bool _allAnimated = false;
        private void _timerTextAnimator_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (_lock)
            {
                string nextChar = _originalLabelTexts[_currentLabelToAnimateIndex][_currentText.Length].ToString();
                _currentText += nextChar;
                Invoke(_updateFormText);
                if (_originalLabelTexts[_currentLabelToAnimateIndex] == _currentText)
                {
                    _currentText = string.Empty;
                    _currentLabelToAnimateIndex++;
                }
                if (_currentLabelToAnimateIndex < _labelsToAnimate.Count)
                {
                    ((System.Timers.Timer)sender).Start();
                }
                else
                {
                    _allAnimated = true;
                }
            }
        }

        private void UpdateFormTextHandler()
        {
            _labelsToAnimate[_currentLabelToAnimateIndex].Text = _currentText;
        }

        private void tmrMain_Tick(object sender, EventArgs e)
        {
            if ((progressMain.Value < progressMain.Maximum))
            {
                progressMain.PerformStep();
                Application.DoEvents();
                return;
            }
            else if (!_allAnimated)
            {
                return;
            }
            tmrMain.Enabled = false;
            Thread.Sleep(2000);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        #endregion //Event Handlers
    }
}