namespace Figlut.Server.Toolkit.Data.DB.SQLQuery
{
    public enum LogicalOperator
    {
        ALL,
        AND,
        ANY,
        BETWEEN,
        EXISTS,
        IN,
        LIKE,
        NOT,
        OR,
        SOME
    }

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/ms173290(v=sql.90).aspx
    /// </summary>
    public class WhereClauseLogicalOperator
    {
        #region Constructors

        public WhereClauseLogicalOperator(LogicalOperator logicalOperator)
        {
            _value = logicalOperator.ToString();
        }

        #endregion //Constructors

        #region Fields

        private string _value;

        #endregion //Fields

        #region Properties

        public string Value
        {
            get { return _value.ToString(); }
        }

        #endregion //Properties

        #region Methods

        public override string ToString()
        {
            return _value;
        }

        #endregion //Methods
    }
}