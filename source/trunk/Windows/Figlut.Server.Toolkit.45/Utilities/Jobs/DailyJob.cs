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

    public abstract class DailyJob
    {
        #region Constructors

        public DailyJob(
            DateTime nextExecutionDateTime,
            bool startImmediately)
        {
            _nextExecutionDateTime = nextExecutionDateTime;
            if (startImmediately)
            {
                StartJob();
            }
        }

        #endregion //Constructors

        #region Fields

        private DateTime _nextExecutionDateTime;
        protected TimeSpan _waitTimeBeforeStart;
        private System.Threading.Timer _timer;
        protected readonly object _lockObject = new object();

        protected bool _enabled;
        protected bool _currentlyExecuting;

        #endregion //Fields

        #region Properties

        public DateTime NextExecutionDateTime
        {
            get { return _nextExecutionDateTime; }
        }

        public TimeSpan WaitTimeBeforeStart
        {
            get { return _waitTimeBeforeStart; }
        }

        public bool IsEnabled
        {
            get { return _enabled; }
        }

        public bool CurrentlyExecuting
        {
            get { return _currentlyExecuting; }
        }

        #endregion //Properties

        #region Methods

        #region Helper Methods

        private DateTime GetTodayMidnightDateTime(Nullable<DateTime> currentDateTime)
        {
            if (!currentDateTime.HasValue)
            {
                currentDateTime = DateTime.Now;
            }
            DateTime midnight = new DateTime(
                currentDateTime.Value.Year,
                currentDateTime.Value.Month,
                currentDateTime.Value.Day,
                0,
                0,
                0);
            return midnight.AddDays(1);
        }

        #endregion //Helper Methods

        private void StartJob()
        {
            DateTime currentDateTime = DateTime.Now;
            if (_nextExecutionDateTime < currentDateTime) //The execution date has passed already, therfore set the TimeSpan before start to tomorrow.
            {
                DateTime midnight = GetTodayMidnightDateTime(currentDateTime);
                TimeSpan timeBeforeMidnight = midnight.Subtract(currentDateTime);
                _waitTimeBeforeStart = timeBeforeMidnight.Add(new TimeSpan(
                    0, //Days
                    _nextExecutionDateTime.Hour,
                    _nextExecutionDateTime.Minute,
                    _nextExecutionDateTime.Second,
                    _nextExecutionDateTime.Millisecond));
            }
            else
            {
                _waitTimeBeforeStart = _nextExecutionDateTime.Subtract(currentDateTime);
            }
            _nextExecutionDateTime = currentDateTime.Add(_waitTimeBeforeStart); //The sub class till have to update its settings/database with the next execution time.
            if (_timer == null)
            {
                _timer = new System.Threading.Timer(TimerElapsed, null, _waitTimeBeforeStart, new TimeSpan(0, 0, 0, 0, -1)); //Will run once off.
            }
            else
            {
                _timer.Change(_waitTimeBeforeStart, new TimeSpan(0, 0, 0, 0, -1)); //Will run once off.
            }
            _enabled = true;
        }

        private void StopJob()
        {
            _timer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite); //Stops the timer from ever running.
            _enabled = false;
        }

        public void SetCurrentlyExecutingFlag(bool currentlyExecuting)
        {
            _currentlyExecuting = currentlyExecuting;
        }

        private void TimerElapsed(object state)
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
                if (!this.IsEnabled)
                {
                    StartJob();
                }
            }
        }

        private void BeginExecution(DailyJob scheduledJob)
        {
            if (scheduledJob.CurrentlyExecuting)
            {
                return;
            }
            lock (_lockObject)
            {
                try
                {
                    scheduledJob.SetCurrentlyExecutingFlag(true);
                    ExecuteJob(scheduledJob);
                }
                finally
                {
                    scheduledJob.SetCurrentlyExecutingFlag(false);
                }
            }
        }

        protected abstract void ExecuteJob(DailyJob scheduledJob);

        #endregion //Methods
    }
}