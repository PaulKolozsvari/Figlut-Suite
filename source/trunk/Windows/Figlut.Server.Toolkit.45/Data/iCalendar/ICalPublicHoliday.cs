namespace Figlut.Server.Toolkit.Data.iCalendar
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class ICalPublicHoliday
    {
        public ICalPublicHoliday()
        {
        }

        public ICalPublicHoliday(
            string eventName,
            int year,
            int month,
            int day)
        {
            this.EventName = eventName;
            this.DateIdentifier = DataShaper.GetDefaultDateString(new DateTime(year, month, day));
            this.Year = year;
            this.Month = month;
            this.Day = day;
        }

        #region Fields

        private string _eventName;
        private string _dateIdentifier;
        private int _year;
        private int _month;
        private int _day;

        #endregion //Fields

        #region Properties

        public string EventName
        {
            get { return _eventName; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(string.Format("{0} may not be null on a {1}.",
                        EntityReader<ICalPublicHoliday>.GetPropertyName(p => p.EventName, false),
                        typeof(ICalPublicHoliday).Name));
                }
                _eventName = value; 
            }
        }

        public string DateIdentifier
        {
            get { return _dateIdentifier; }
            set 
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(string.Format("{0} may not be null on a {1}.",
                        EntityReader<ICalPublicHoliday>.GetPropertyName(p => p.DateIdentifier, false),
                        typeof(ICalPublicHoliday).Name));
                }
                _dateIdentifier = value; 
            }
        }

        public int Year
        {
            get { return _year; }
            set 
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Year may not be less or equal to 0."));
                }
                _year = value; 
            }
        }

        public int Month
        {
            get { return _month; }
            set 
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Month may not be less or equal to 0."));
                }
                if (value > 12)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Month may not be greater than 12."));
                }
                _month = value; 
            }
        }

        public int Day
        {
            get { return _day; }
            set 
            {
                if (value <= 0)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Day may not be less or equal to 0."));
                }
                if (value > 31)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Day may not be greater than 31."));
                }
                _day = value; 
            }
        }

        #endregion //Properties

        #region Methods

        public DateTime GetDate()
        {
            return new DateTime(_year, _month, _day);
        }

        #endregion //Methods
    }
}
