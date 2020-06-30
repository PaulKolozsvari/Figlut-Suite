namespace Figlut.Server.Toolkit.Data.DB.SQLQuery
{
    /// <summary>
    /// Contains and manages sort directions used for running queries against a database.
    /// </summary>
    public class SortDirection
    {
        #region Constants

        /// <summary>
        /// Sort in an ascending order.
        /// </summary>
        public const string ASCENDING = "ASC";
        /// <summary>
        /// Sort in a descending order
        /// </summary>
        public const string DESCENDING = "DESC";

        #endregion //Constants

        #region Methods

        /// <summary>
        /// Gets the SortDirectionType from a string representation of the sort directtion.
        /// </summary>
        /// <param name="sortDirection"></param>
        /// <returns></returns>
        public static SortDirectionType GetSortDirectionType(string sortDirection)
        {
            sortDirection = sortDirection != null ? sortDirection.ToUpper() : string.Empty;
            switch (sortDirection)
            {
                case ASCENDING:
                    return SortDirectionType.Ascending;
                case DESCENDING:
                    return SortDirectionType.Descending;
                default:
                    return SortDirectionType.Ascending;
            }
        }

        /// <summary>
        /// Gets the string representation of th sort direction from the SortDirection type.
        /// </summary>
        /// <param name="sortDirectionType"></param>
        /// <returns></returns>
        public static string GetSortDirection(SortDirectionType sortDirectionType)
        {
            switch (sortDirectionType)
            {
                case SortDirectionType.Ascending:
                    return ASCENDING;
                case SortDirectionType.Descending:
                    return DESCENDING;
                default:
                    return ASCENDING;
            }
        }

        #endregion //Methods
    }
}
