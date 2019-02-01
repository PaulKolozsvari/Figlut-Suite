namespace Figlut.Server.Toolkit.Web.MVC.Models
{
    #region Using Directives

    using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

    #endregion //Using Directives

    public class FilterModel<T> where T : class
    {
        #region Constructors

        public FilterModel()
        {
            if (DataModel == null)
            {
                DataModel = new List<T>();
            }
        }

        #endregion //Constructors

        #region Properties

        public bool IsAdministrator { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        /// <summary>
        /// Total Records
        /// </summary>
        public long TotalCount
        {
            get { return DataModel.LongCount(); }
        }

        /// <summary>
        /// 
        /// </summary>
        public long TotalTableCount { get; set; }

        public string SearchText { get; set; }

        public string SearchFieldIdentifier { get; set; }

        public string Sort { get; set; }

        public string Sortdir { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> EndDate { get; set; }

        public List<T> DataModel { get; set; }

        public Nullable<Guid> ParentId { get; set; }

        public string ParentCaption { get; set; }

        public Nullable<Guid> SecondParentId { get; set; }

        public string SecondParentCaption { get; set; }

        #endregion //Properties

        #region Methods

        public FilterModel<E> Clone<E>() where E : class
        {
            FilterModel<E> result = new FilterModel<E>();
            result.Page = this.Page;
            result.PageSize = this.PageSize;
            result.SearchText = this.SearchText;
            result.Sort = this.Sort;
            result.Sortdir = this.Sortdir;
            return result;
        }

        #endregion //Methods
    }
}