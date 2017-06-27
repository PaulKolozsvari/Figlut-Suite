namespace Figlut.Server.Toolkit.Data.iCalendar
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class ICalCalendar
    {
        #region Constructors

        public ICalCalendar()
        {
            PublicHolidays = new List<ICalPublicHoliday>();
        }

        public ICalCalendar(string countryCode, string countryName) : this()
        {
            this.CountryCode = countryCode;
            this.CountryName = countryName;
        }

        #endregion //Constructors

        #region Fields

        private string _countryCode;
        private string _countryName;

        #endregion //Fields

        #region Properties

        public string CountryCode
        {
            get { return _countryCode; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(string.Format(
                        "{0} may not be null on a {1}.",
                        EntityReader<ICalCalendar>.GetPropertyName(p => p.CountryCode, false),
                        typeof(ICalCalendar).Name));
                } 
                _countryCode = value; 
            }
        }

        public string CountryName
        {
            get { return _countryName; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(string.Format(
                        "{0} may not be null on a {1}.",
                        EntityReader<ICalCalendar>.GetPropertyName(p => p.CountryName, false),
                        typeof(ICalCalendar).Name));
                }
                _countryName = value;
            }
        }

        public List<ICalPublicHoliday> PublicHolidays { get; set; }

        #endregion //Properties
    }
}