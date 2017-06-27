namespace Psion.Server.Toolkit.Data.DB.SQLQuery
{
    #region Using Directives

    using System;

    #endregion //Using Directives

    public enum ComparisonOperator
    {
        /// <summary>
        /// =
        /// </summary>
        EQUALS,
        /// <summary>
        /// >
        /// </summary>
        GREATER_THAN,
        /// <summary>
        /// &lt
        /// </summary>
        LESS_THAN,
        /// <summary>
        /// >=
        /// </summary>
        GREATER_THAN_OR_EQUAL_TO,
        /// <summary>
        /// &lt=
        /// </summary>
        LESS_THAN_OR_EQUAL_TO,
        /// <summary>
        /// &lt>
        /// </summary>
        NOT_EQUAL_TO_ANGLE_BRACKETS,
        /// <summary>
        /// !=
        /// </summary>
        NOT_EQUAL_TO,
        /// <summary>
        /// !&lt
        /// </summary>
        NOT_LESS_THAN,
        /// <summary>
        /// !>
        /// </summary>
        NOT_GREATER_THAN,
        /// <summary>
        /// IS
        /// </summary>
        IS,
        /// <summary>
        /// IS NOT
        /// </summary>
        IS_NOT
    }

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/ms173290(v=sql.90).aspx
    /// </summary>
    public class WhereClauseComparisonOperator
    {
        #region Constructors

        public WhereClauseComparisonOperator(ComparisonOperator comparisonOperator)
        {
            switch (comparisonOperator)
            {
                case ComparisonOperator.EQUALS:
                    _value = "=";
                    break;
                case ComparisonOperator.GREATER_THAN:
                    _value = ">";
                    break;
                case ComparisonOperator.LESS_THAN:
                    _value = "<";
                    break;
                case ComparisonOperator.GREATER_THAN_OR_EQUAL_TO:
                    _value = ">=";
                    break;
                case ComparisonOperator.LESS_THAN_OR_EQUAL_TO:
                    _value = "<=";
                    break;
                case ComparisonOperator.NOT_EQUAL_TO_ANGLE_BRACKETS:
                    _value = "<>";
                    break;
                case ComparisonOperator.NOT_EQUAL_TO:
                    _value = "!=";
                    break;
                case ComparisonOperator.NOT_LESS_THAN:
                    _value = "!<";
                    break;
                case ComparisonOperator.NOT_GREATER_THAN:
                    _value = "!>";
                    break;
                case ComparisonOperator.IS:
                    _value = "IS";
                    break;
                case ComparisonOperator.IS_NOT:
                    _value = "IS NOT";
                    break;
                default:
                    throw new ArgumentException(string.Format(
                        "Invalid {0} of {1}",
                        typeof(ComparisonOperator).FullName,
                        comparisonOperator.ToString()));
            }
        }

        #endregion //Constructors

        #region Fields

        private string _value;

        #endregion //Fields

        #region Properties

        public string Value
        {
            get { return _value; }
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