namespace Figlut.Server.Toolkit.Utilities.Jobs
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Figlut.Server.Toolkit.Data;

    #endregion //Using Directives

    public abstract class IntervalJob
    {
        #region Constructors

        public IntervalJob(int executionInterval, bool startImmediately)
        {
            if (executionInterval < 0)
            {
                throw new ArgumentOutOfRangeException(string.Format("{0} may not be less than 0 when constructing a {1}.",
                    EntityReader<IntervalJob>.GetPropertyName(p => p.ExecutionInterval, false),
                    this.GetType().Name));
            }
            _executionInterval = executionInterval;
            _timer = new System.Timers.Timer();
            _timer.Elapsed += _timer_Elapsed;
            ChangeExecutionInterval(_executionInterval);
            if (startImmediately)
            {
                StartJob();
            }
        }

        #endregion //Constructors

        #region Fields

        protected int _executionInterval;
        protected bool _currentlyExecuting;
        protected System.Timers.Timer _timer;
        protected readonly object _lockObject = new object();

        #endregion //Fields

        #region Properties

        public int ExecutionInterval
        {
            get { return _executionInterval; }
        }

        public bool CurrentlyExecuting
        {
            get { return _currentlyExecuting; }
        }

        #endregion //Properties

        #region Methods

        public void SetCurrentlyExecutingFlag(bool currentlyExecuting)
        {
            _currentlyExecuting = currentlyExecuting;
        }

        public void StartJob()
        {
            _timer.Start();
        }

        internal void StopJob()
        {
            _timer.Stop();
        }

        internal bool IsEnabled()
        {
            return _timer.Enabled;
        }

        public bool ChangeExecutionInterval(int executionInterval)
        {
            if (_currentlyExecuting)
            {
                return false;
            }
            _executionInterval = executionInterval;
            _timer.Interval = Convert.ToDouble(_executionInterval);
            return true;
        }

        private void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            StopJob();
            try
            {
                BeginExecution(this);
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex);
            }
            finally
            {
                if (!this.IsEnabled())
                {
                    this.StartJob();
                }
            }
        }

        private void BeginExecution(IntervalJob intervalJob)
        {
            if (intervalJob.CurrentlyExecuting)
            {
                return;
            }
            lock (_lockObject)
            {
                try
                {
                    ExecuteJob(intervalJob);
                }
                finally
                {
                    intervalJob.SetCurrentlyExecutingFlag(false);
                }
            }
        }

        protected abstract void ExecuteJob(IntervalJob intervalJob);

        #endregion //Methods
    }
}