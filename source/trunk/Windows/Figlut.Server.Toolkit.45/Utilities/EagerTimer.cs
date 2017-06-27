namespace Figlut.Server.Toolkit.Utilities
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Timers;

    #endregion //Using Directives

    /// <summary>
    // EagerTimer is a simple wrapper around System.Timers.Timer that
    // provides "set up and immediately execute" functionality by adding a
    // new AutoStart property, and also provides the ability to manually
    // raise the Elapsed event with RaiseElapsed.
    /// </summary>
    public class EagerTimer : System.Timers.Timer
    {
        #region Constructors

        public EagerTimer()
            : base()
        {
        }

        public EagerTimer(double interval)
            : base(interval)
        {
        }

        #endregion //Constructors

        // Need to hide this so we can use Elapsed.Invoke below
        // (otherwise the compiler complains)
        private event ElapsedEventHandler _elapsedHandler;
        public new event ElapsedEventHandler Elapsed
        {
            add 
            { 
                _elapsedHandler += value; base.Elapsed += value; 
            }
            remove 
            { 
                _elapsedHandler -= value; base.Elapsed -= value; 
            }
        }

        #region Methods

        /// <summary>
        /// Starts the timer. If the autoStart parameter on this method or the AutoStart property on the timer is set to True, then the
        /// elapsed event is fired.
        /// </summary>
        /// <param name="autoStart">If set to true (or if the AutoStart property on this Timer is set to True), it forces the the elapsed event to be fired.</param>
        public new void Start(bool autoStart)
        {
            // Proceed as normal
            base.Start();
            // If AutoStart is enabled, we need to invoke the timer event manually
            if (autoStart || AutoStart)
            {
                this._elapsedHandler.BeginInvoke(this, null, new AsyncCallback(AutoStartCallback), _elapsedHandler); // fire immediately
            }
        }

        private void AutoStartCallback(IAsyncResult result)
        {
            ElapsedEventHandler handler = result.AsyncState as ElapsedEventHandler;
            if (handler != null) handler.EndInvoke(result);
        }

        /// <summary>
        /// Manually raises the Elapsed event of the System.Timers.Timer.
        /// </summary>
        public void RaiseElapsed()
        {
            if (_elapsedHandler != null)
                _elapsedHandler(this, null);
        }

        #endregion //Methods

        #region Properties

        // Summary:
        //     Gets or sets a value indicating whether the EagerTimer should raise
        //     the System.Timers.Timer.Elapsed event immediately when Start() is called,
        //     or only after the first time it elapses. If AutoStart is false, EagerTimer behaves
        //     identically to System.Timers.Timer.
        //
        // Returns:
        //     true if the EagerTimer should raise the System.Timers.Timer.Elapsed
        //     event immediately when Start() is called; false if it should raise the System.Timers.Timer.Elapsed
        //     event only after the first time the interval elapses. The default is true.
        [Category("Behavior")]
        [DefaultValue(true)]
        [TimersDescription("TimerAutoStart")]
        public bool AutoStart { get; set; }

        #endregion //Properties
    }
}