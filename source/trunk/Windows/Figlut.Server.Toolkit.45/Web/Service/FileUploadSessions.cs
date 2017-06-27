namespace Figlut.Server.Toolkit.Web.Service
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class FileUploadSessions
    {
        #region Singleton Setup

        private static FileUploadSessions _instance;

        public static FileUploadSessions Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new FileUploadSessions();
                }
                return _instance;
            }
        }

        #endregion //Singleton Setup

        #region Constructors

        private FileUploadSessions()
        {
            _fileStreams = new Dictionary<string, FileStream>();
        }

        #endregion //Constructors

        #region Fields

        private Dictionary<string, FileStream> _fileStreams;

        #endregion //Fields

        #region Properties

        public Dictionary<string, FileStream> FileStreams
        {
            get { return _fileStreams; }
        }

        #endregion //Properties
    }
}