namespace Figlut.Server.Toolkit.Data.QueryRunners
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Figlut.Server.Toolkit.Utilities;

    #endregion //Using Directives

    [Serializable]
    public class SqlQueryRunnerConfig
    {
        #region Constructors

        public SqlQueryRunnerConfig(
            string sqlQueryRunnerAssemblyPath,
            string sqlQueryRunnerFullTypeName)
        {
            SqlQueryRunnerAssemblyPath = sqlQueryRunnerAssemblyPath;
            SqlQueryRunnerFullTypeName = sqlQueryRunnerFullTypeName;
            QueryRunnerAssemblyBytes = FileSystemHelper.GetFileBytes(sqlQueryRunnerAssemblyPath);
        }

        public SqlQueryRunnerConfig(
            string sqlQueryRunnerAssemblyPath,
            string sqlQueryRunnerFullTypeName,
            byte[] queryRunnerAssemblyBytes)
        {
            SqlQueryRunnerAssemblyPath = sqlQueryRunnerAssemblyPath;
            SqlQueryRunnerFullTypeName = sqlQueryRunnerFullTypeName;
            QueryRunnerAssemblyBytes = queryRunnerAssemblyBytes;
        }

        #endregion //Constructors

        #region Fields

        protected string _sqlQueryRunnerAssemblyPath;
        protected string _sqlQueryRunnerFullTypeName;
        protected byte[] _queryRunnerAssemblyBytes;

        #endregion //Fields

        #region Properties

        public string SqlQueryRunnerAssemblyPath
        {
            get { return _sqlQueryRunnerAssemblyPath; }
            set
            {
                if(string.IsNullOrEmpty(value))
                {
                    throw new NullReferenceException(string.Format(
                        "{0} may not be null or empty on {1}.",
                        EntityReader<SqlQueryRunnerConfig>.GetPropertyName(p => p.SqlQueryRunnerAssemblyPath, false),
                        this.GetType().FullName));
                }
                _sqlQueryRunnerAssemblyPath = value;
            }
        }

        public string SqlQueryRunnerFullTypeName
        {
            get { return _sqlQueryRunnerFullTypeName; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new NullReferenceException(string.Format(
                        "{0} may not be null or empty on {1}.",
                        EntityReader<SqlQueryRunnerConfig>.GetPropertyName(p => p.SqlQueryRunnerFullTypeName, false),
                        this.GetType().FullName));
                }
                _sqlQueryRunnerFullTypeName = value;
            }
        }

        public byte[] QueryRunnerAssemblyBytes
        {
            get { return _queryRunnerAssemblyBytes; }
            set 
            {
                if (value == null || value.Length < 1)
                {
                    throw new NullReferenceException(string.Format(
                        "{0} may not be null or have a 0 length on {1}.",
                        EntityReader<SqlQueryRunnerConfig>.GetPropertyName(p => p.QueryRunnerAssemblyBytes, false),
                        this.GetType().FullName));
                }
                _queryRunnerAssemblyBytes = value; 
            }
        }

        #endregion //Properties
    }
}