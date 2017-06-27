namespace Figlut.Mobile.Toolkit.Data.DB.SyncService
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Synchronization.Data;
    using Microsoft.Synchronization;

    #endregion //Using Directives

    public class SyncResult
    {
        #region Constructors

        public SyncResult()
        {
            _syncProgressMessage = new StringBuilder();
        }

        #endregion //Constructors

        #region Fields

        protected SyncStatistics _syncStatistics;
        protected StringBuilder _syncProgressMessage;
        protected StringBuilder _syncStatisticsSummaryMessage;

        #endregion //Fields

        #region Properties

        public SyncStatistics SyncStatistics
        {
            get { return _syncStatistics; }
        }

        public string SyncProgressMessage
        {
            get { return _syncProgressMessage.ToString(); }
        }

        public string SyncStatisticsSummaryMessage
        {
            get { return _syncStatisticsSummaryMessage.ToString(); }
        }

        #endregion //Properties

        #region Methods

        public string SyncStateChanged(SessionStateChangedEventArgs e)
        {
            StringBuilder result = new StringBuilder();
            switch (e.SessionState)
            {
                case SyncSessionState.Ready:
                    result.AppendLine("Sync Agent ready ...".PadRight(50));
                    break;
                case SyncSessionState.Synchronizing:
                    result.AppendLine("Sync Agent synchronizing ...".PadRight(50));
                    break;
                default:
                    throw new ArgumentException(string.Format(
                        "Unexpected SessionStateChangedEventArgs value {0}.",
                        e.SessionState.ToString()));
            }
            result.AppendLine(string.Format("Client Id: {0}", e.SyncSession.ClientId));
            result.AppendLine(string.Format("Originator Id: {0}", e.SyncSession.OriginatorId));
            result.Append(string.Format("Session Id: {0}", e.SyncSession.SessionId));
            _syncProgressMessage.AppendLine(result.ToString());
            return result.ToString();
        }

        public string SyncSessionProgress(SessionProgressEventArgs e, bool showPercentage)
        {
            string percentage = showPercentage ? string.Format("{0} %", e.PercentCompleted) : string.Empty;
            string result = string.Format(
                "{0} ... {1}",
                DataShaper.ShapeCamelCaseString(e.SyncStage.ToString()),
                percentage);
            _syncProgressMessage.AppendLine(result);
            return result;
        }

        public string SyncCompleted(SyncStatistics syncStatistics)
        {
            if (syncStatistics == null)
            {
                throw new NullReferenceException(string.Format(
                    "syncStatistics may not be null when calling SyncCompleted on {0}.",
                    this.GetType().FullName));
            }
            _syncStatistics = syncStatistics;
            _syncStatisticsSummaryMessage = new StringBuilder();
            _syncStatisticsSummaryMessage.AppendLine("*** Sync Statistics ***");
            _syncStatisticsSummaryMessage.AppendLine(string.Format("Start Time: {0}", syncStatistics.SyncStartTime));
            _syncStatisticsSummaryMessage.AppendLine(string.Format("Complete Time: {0}", syncStatistics.SyncCompleteTime));
            _syncStatisticsSummaryMessage.AppendLine(string.Format("Changes downloaded: {0}", syncStatistics.TotalChangesDownloaded));
            _syncStatisticsSummaryMessage.AppendLine(string.Format("Changes uploaded: {0}", syncStatistics.TotalChangesUploaded));
            _syncStatisticsSummaryMessage.AppendLine(string.Format("Downloads Applied: {0}", syncStatistics.DownloadChangesApplied));
            _syncStatisticsSummaryMessage.AppendLine(string.Format("Downloads Failed: {0}", syncStatistics.DownloadChangesFailed));
            _syncStatisticsSummaryMessage.AppendLine(string.Format("Uploads Applied: {0}", syncStatistics.UploadChangesApplied));
            _syncStatisticsSummaryMessage.AppendLine(string.Format("Uploads Failed: {0}", syncStatistics.UploadChangesFailed));
            _syncStatisticsSummaryMessage.AppendLine();

            return _syncStatistics.ToString();
        }

        #endregion //Methods
    }
}