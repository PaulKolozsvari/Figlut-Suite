namespace Figlut.Server.Toolkit.Data.DB.SQLQuery
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

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
        IS_NOT,
        /// <summary>
        /// LIKE
        /// </summary>
        LIKE,
        /// <summary>
        /// IN
        /// </summary>
        IN
    }

    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/ms173290(v=sql.90).aspx
    /// </summary>
    public class WhereClauseComparisonOperator
    {
        #region Constructors

        public WhereClauseComparisonOperator(string comparisonOperator)
        {
            string comparisonOperatorLower = comparisonOperator.Trim().ToLower();
            Dictionary<string, string> comparisonOperators = GetComparisonOperators();
            bool validOperator = false;
            foreach(string s in comparisonOperators.Values)
            {
                if(s == comparisonOperatorLower)
                {
                    validOperator = true;
                    break;
                }
            }
            if(!validOperator)
            {
                throw new ArgumentException(string.Format("Invalid comparison operator {0}.", comparisonOperator));
            }
            _value = comparisonOperatorLower;
        }

        public WhereClauseComparisonOperator(ComparisonOperator comparisonOperator)
        {
            _value = GetComparisonOperators()[comparisonOperator.ToString()];
        }

        #endregion //Constructors

        #region Fields

        private string _value;
        protected Dictionary<string, string> _comparisonOperators;

        #endregion //Fields

        #region Properties

        public string Value
        {
            get { return _value; }
        }

        #endregion //Properties

        #region Methods

        public static Dictionary<string, string> GetComparisonOperators()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            result.Add(ComparisonOperator.EQUALS.ToString(), "=");
            result.Add(ComparisonOperator.GREATER_THAN.ToString(), ">");
            result.Add(ComparisonOperator.LESS_THAN.ToString(), "<");
            result.Add(ComparisonOperator.GREATER_THAN_OR_EQUAL_TO.ToString(), ">=");
            result.Add(ComparisonOperator.LESS_THAN_OR_EQUAL_TO.ToString(), "<=");
            result.Add(ComparisonOperator.NOT_EQUAL_TO_ANGLE_BRACKETS.ToString(), "<>");
            result.Add(ComparisonOperator.NOT_EQUAL_TO.ToString(), "!=");
            result.Add(ComparisonOperator.NOT_LESS_THAN.ToString(), "!<");
            result.Add(ComparisonOperator.NOT_GREATER_THAN.ToString(), "!>");
            result.Add(ComparisonOperator.IS.ToString(), "IS");
            result.Add(ComparisonOperator.IS_NOT.ToString(), "IS_NOT");
            result.Add(ComparisonOperator.LIKE.ToString(), "LIKE");
            result.Add(ComparisonOperator.IN.ToString(), "IN");
            return result;
        }

        public static ComparisonOperator GetComparisonOperatorFromString(string comparisonOperator)
        {
            string comparisonOperatorLower = comparisonOperator.ToLower();
            foreach (KeyValuePair<string, string> keyPair in GetComparisonOperators())
            {
                if (keyPair.Value == comparisonOperatorLower)
                {
                    return (ComparisonOperator)Enum.Parse(typeof(ComparisonOperator), keyPair.Key);
                }
            }
            throw new ArgumentException(string.Format("Could not find a {0} for string {1}.", typeof(ComparisonOperator).Name, comparisonOperator));
        }

        public override string ToString()
        {
            return _value;
        }

        #endregion //Methods
    }
}