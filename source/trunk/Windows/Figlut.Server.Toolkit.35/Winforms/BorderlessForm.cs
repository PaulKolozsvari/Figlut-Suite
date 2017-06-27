namespace Figlut.Server.Toolkit.Winforms
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
    using System.Drawing.Drawing2D;
    using System.Threading;
    using System.Runtime.InteropServices;

    #endregion //Using Directives

    public partial class BorderlessForm : Form
    {
        //Articles used:
        //http://msdn.microsoft.com/en-us/library/Aa984331
        //http://thedailyreviewer.com/dotnet/view/how-to-move-a-form-that-formborderstyle-is-none-102339004
        //http://www.dreamincode.net/forums/topic/109668-animating-a-windows-form/

        #region Inner Types

        private delegate void UpdateFormText();

        #endregion //Inner Types

        #region Constructors

        public BorderlessForm()
        {
            InitializeComponent();
            _originalFormTitle = FormTitle;
            this.DoubleBuffered = true;
            _animate = true;
        }

        public BorderlessForm(bool animate)
        {
            InitializeComponent();
            _originalFormTitle = FormTitle;
            this.DoubleBuffered = true;
            _animate = animate;
        }

        #endregion //Constructors

        #region PInvokes

        [DllImport("user32.dll")]
        static  extern bool AnimateWindow(IntPtr hWnd, int time, AnimateWindowFlags flags);

        [Flags]
        enum AnimateWindowFlags
        {
            AW_HOR_POSITIVE = 0x00000001,
            AW_HOR_NEGATIVE = 0x00000002,
            AW_VER_POSITIVE = 0x00000004,
            AW_VER_NEGATIVE = 0x00000008,
            AW_CENTER = 0x00000010,
            AW_HIDE = 0x00010000,
            AW_ACTIVATE = 0x00020000,
            AW_SLIDE = 0x00040000,
            AW_BLEND = 0x00080000
        }

        #endregion //PInvokes

        #region Constants

        private int TITLE_ANIMATOR_INTERVAL = 50;
        private int STATUS_ANIMATOR_INTERVAL = 10;

        #endregion //Constants

        #region Fields

        private bool _animate;

        private Point _mouseOffset;
        private bool _isMouseDown = false;

        private string _currentFormTitle;
        private string _originalFormTitle;
        private UpdateFormText _updateFormTitle;
        private System.Timers.Timer _timerTitleAnimator; //Runs tick (elapsed) event handlers on a separate thread, but only on one new thread.
        private object _titleAnimatorLock = new object();

        private string _currentStatus;
        private string _originalStatus;
        private UpdateFormText _updateStatus;
        private System.Timers.Timer _timerStatusAnimator;
        private object _statusAnimatorLock = new object();
        
        private bool _closing = false;
        private bool _disposing;
        private bool _disposed;

        #endregion //Fields

        #region Properties

        public string FormTitle
        {
            get { return lblFormTitle.Text.Trim(); }
            set 
            {
                lblFormTitle.Text = string.Concat(value, " ");
            }
        }

        public string Status
        {
            get { return _originalStatus; }
            set
            {
                _originalStatus = value;
                if (DesignMode)
                {
                    statusMain.Text = value;
                    return;
                }
                statusMain.Text = string.Empty;
                StartStatusAnimation();
            }
        }

        #endregion //Properties

        #region Event Handlers

        private void StartStatusAnimation()
        {
            _currentStatus = string.Empty;
            _updateStatus = new UpdateFormText(UpdateStatusHandler);
            _timerStatusAnimator = new System.Timers.Timer(STATUS_ANIMATOR_INTERVAL);
            _timerStatusAnimator.AutoReset = false;
            _timerStatusAnimator.Elapsed += new System.Timers.ElapsedEventHandler(_timerStatusAnimator_Elapsed);
            _timerStatusAnimator.Start();
        }

        private void _timerStatusAnimator_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_updateStatus == null || _closing || _disposing || this.IsDisposed)
            {
                ((System.Timers.Timer)sender).Stop();
                return;
            }
            lock (_titleAnimatorLock)
            {
                if (_currentStatus == null || _originalStatus == null || (_currentStatus.Length == _originalStatus.Length))
                {
                    return;
                }
                if (_originalStatus.Substring(0, _currentStatus.Length) != _currentStatus)
                {
                    throw new Exception("Could not animate text.");
                }
                _currentStatus += _originalStatus[_currentStatus.Length].ToString();
                if (_updateStatus != null && !this.Disposing)
                {
                    Invoke(_updateStatus);
                    ((System.Timers.Timer)sender).Start();
                }
            }
        }

        public void AnimateShowForm()
        {
            if (_animate)
            {
                AnimateWindow(this.Handle, 400, AnimateWindowFlags.AW_CENTER);
            }
        }

        public void AnimateHideForm()
        {
            if (_animate)
            {
                AnimateWindow(this.Handle, 400, AnimateWindowFlags.AW_CENTER | AnimateWindowFlags.AW_HIDE);
            }
        }

        private void BorderlessForm_Load(object sender, EventArgs e)
        {
            AnimateShowForm();
            AnimateFormTitle();
        }

        public void AnimateFormTitle()
        {
            _currentFormTitle = string.Empty;
            _originalFormTitle = FormTitle;
            FormTitle = string.Empty;

            _updateFormTitle = new UpdateFormText(UpdateTitleHandler);
            _timerTitleAnimator = new System.Timers.Timer(TITLE_ANIMATOR_INTERVAL);
            _timerTitleAnimator.AutoReset = false; //Only fires the tick event once. The timer needs to then be restarted in the tick event handler.
            _timerTitleAnimator.Elapsed += new System.Timers.ElapsedEventHandler(_timerTextAnimator_Elapsed);
            _timerTitleAnimator.Start();
        }

        private void BorderlessForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Name != "MainForm")
            {
                AnimateHideForm();
            }
            _closing = true;
            if (_timerTitleAnimator != null)
            {
                _timerTitleAnimator.Stop();
            }
            if (_timerStatusAnimator != null)
            {
                _timerStatusAnimator.Stop();
            }
        }

        private void _timerTextAnimator_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (_closing)
            {
                return;
            }
            lock (_titleAnimatorLock)
            {
                if (_currentFormTitle.Length == _originalFormTitle.Length)
                {
                    return;
                }
                if (_originalFormTitle.Substring(0, _currentFormTitle.Length) != _currentFormTitle)
                {
                    throw new Exception("Could not animate text.");
                }
                _currentFormTitle += _originalFormTitle[_currentFormTitle.Length].ToString();
                if (_updateFormTitle != null && !this.Disposing)
                {
                    Invoke(_updateFormTitle);
                    ((System.Timers.Timer)sender).Start();
                }
            }
        }

        private void UpdateTitleHandler()
        {
            FormTitle = _currentFormTitle;
        }

        private void UpdateStatusHandler()
        {
            statusMain.Text = _currentStatus;
            Application.DoEvents();
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            _disposing = disposing;
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            _updateFormTitle = null;
            _updateStatus = null;
            if (_timerTitleAnimator != null)
            {
                _timerTitleAnimator.Stop();
            }
            if (_timerStatusAnimator != null)
            {
                _timerStatusAnimator.Stop();
            }
            base.Dispose(disposing);
            _disposed = true;
        }

        protected void BorderlessForm_MouseDown(object sender, MouseEventArgs e)
        {
            int xOffset;
            int yOffset;

            if (e.Button == MouseButtons.Left)
            {
                xOffset = -e.X - SystemInformation.FrameBorderSize.Width;
                yOffset = -e.Y - SystemInformation.CaptionHeight - SystemInformation.FrameBorderSize.Height;
                _mouseOffset = new Point(xOffset, yOffset);
                this.Cursor = Cursors.Hand;
                _isMouseDown = true;
                //this.Opacity = 0.5;
                Application.DoEvents();
            }
        }

        protected void BorderlessForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isMouseDown)
            {
                Point mousePos = Control.MousePosition;
                mousePos.Offset(_mouseOffset.X, _mouseOffset.Y);
                Location = mousePos;
            }
        }

        protected void BorderlessForm_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Cursor = Cursors.Default;
                _isMouseDown = false;
                //this.Opacity = 1;
                Application.DoEvents();
            }
        }

        #endregion //Event Handlers

        #region Methods

        protected void BorderLessFormResize(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Size = new Size(this.PointToClient(MousePosition).X, this.PointToClient(MousePosition).Y);
            }
        }

        protected void BorderlessForm_Maximize(object sender, EventArgs e)
        {
            this.WindowState =
                this.WindowState == FormWindowState.Normal ?
                FormWindowState.Maximized :
                FormWindowState.Normal;
        }

        protected void BorderlessForm_Minimize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion //Methods
    }
}