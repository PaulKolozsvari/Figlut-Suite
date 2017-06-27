namespace Figlut.Mobile.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Forms;

    #endregion //Using Directives

    /// <summary>
    /// A helper class for displaying a wait cursor.
    /// On this creation of an object of this WaiCursor class the current cursor will be set to a wait cursor.
    /// On calling Dispose on this object, the current cursor will set back to its default.
    /// This class should be used in a using clause e.g.
    /// 
    /// using(WaitCursor w = new WaitCursor())
    /// {
    ///     //Some code that will take some time to execute.
    /// }
    /// </summary>
    public class WaitProcess : IDisposable
    {
        #region Constructors

        /// <summary>
        /// A helper class for displaying a wait cursor.
        /// On this calling this constructor the current cursor will be set to a wait cursor.
        /// On calling Dispose on this object, the current cursor will set back to its default.
        /// </summary>
        public WaitProcess()
        {
            Cursor.Current = Cursors.WaitCursor;
        }

        public WaitProcess(MainMenu mainMenuToDisable)
        {
            _mainMenu = mainMenuToDisable;
            UIHelper.MenuEnabled(_mainMenu, false);
            Cursor.Current = Cursors.WaitCursor;
        }

        public WaitProcess(MainMenu mainMenuToDisable, StatusBar statusBar)
        {
            _mainMenu = mainMenuToDisable;
            UIHelper.MenuEnabled(_mainMenu, false);
            _statusBar = statusBar;
            if (_statusBar != null)
            {
                _originalStatus = _statusBar.Text;
            }
            Cursor.Current = Cursors.WaitCursor;
        }

        public WaitProcess(MainMenu mainMenuToDisable, Label statusLabel)
        {
            _mainMenu = mainMenuToDisable;
            UIHelper.MenuEnabled(_mainMenu, false);
            _statusLabel = statusLabel;
            if (_statusLabel != null)
            {
                _originalStatus = _statusLabel.Text;
            }
            Cursor.Current = Cursors.WaitCursor;
        }

        public WaitProcess(MainMenu mainMenuToDisable, StatusBar statusBar, Label statusLabel)
        {
            _mainMenu = mainMenuToDisable;
            UIHelper.MenuEnabled(_mainMenu, false);
            _statusBar = statusBar;
            if(_statusBar != null)
            {
                _originalStatus = _statusBar.Text;
            }
            _statusLabel = statusLabel;
            Cursor.Current = Cursors.WaitCursor;
        }

        public WaitProcess(MainMenu mainMenuToDisable, TextBox statusTextBox)
        {
            _mainMenu = mainMenuToDisable;
            UIHelper.MenuEnabled(_mainMenu, false);
            _statusTextBox = statusTextBox;
            if (_statusTextBox != null)
            {
                _originalStatus = _statusTextBox.Text;
            }
            Cursor.Current = Cursors.WaitCursor;
        }

        public WaitProcess(StatusBar statusBar)
        {
            _statusBar = statusBar;
            if (_statusBar != null)
            {
                _originalStatus = _statusBar.Text;
            }
            Cursor.Current = Cursors.WaitCursor;
        }

        public WaitProcess(Label statusLabel)
        {
            _statusLabel = statusLabel;
            if (_statusBar != null)
            {
                _originalStatus = _statusLabel.Text;
            }
            Cursor.Current = Cursors.WaitCursor;
        }

        public WaitProcess(TextBox statusTextBox)
        {
            _statusTextBox = statusTextBox;
            if (_statusTextBox != null)
            {
                _originalStatus = _statusTextBox.Text;
            }
            Cursor.Current = Cursors.WaitCursor;
        }

        #endregion //Constructors

        #region Fields

        protected MainMenu _mainMenu;
        protected StatusBar _statusBar;
        protected Label _statusLabel;
        protected TextBox _statusTextBox;
        protected string _originalStatus;

        #endregion //Fields

        #region Methods

        public void ChangeStatus(string status)
        {
            if (_statusBar != null)
            {
                _statusBar.Text = status;
            }
            if (_statusLabel != null)
            {
                _statusLabel.Text = status;
            }
            if (_statusTextBox != null)
            {
                _statusTextBox.Text = status;
            }
            Application.DoEvents();
        }

        /// <summary>
        /// The current cursor will set back to a default cursor.
        /// </summary>
        public void Dispose()
        {
            if (_mainMenu != null)
            {
                UIHelper.MenuEnabled(_mainMenu, true);
            }
            if (_statusBar != null && _statusBar.Text != _originalStatus)
            {
                _statusBar.Text = _originalStatus;
            }
            if (_statusLabel != null && _statusLabel.Text != _originalStatus)
            {
                _statusLabel.Text = _originalStatus;
            }
            if (_statusTextBox != null && _statusTextBox.Text != _originalStatus)
            {
                _statusTextBox.Text = _originalStatus;
            }
            Cursor.Current = Cursors.Default;
        }

        #endregion //Methods
    }
}