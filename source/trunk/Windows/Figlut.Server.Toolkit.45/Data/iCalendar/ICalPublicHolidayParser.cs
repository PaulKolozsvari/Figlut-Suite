namespace Figlut.Server.Toolkit.Data.iCalendar
{
    #region Using Directives

    using Ical.Net;
    using Ical.Net.DataTypes;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    public class ICalPublicHolidayParser
    {
        #region Constants

        public const string ICALENDAR_FILE_EXTENSION = ".ics";
        public const string PUBLIC_HOLIDAY_EVENT_CLASS_NAME = "public";

        #endregion //Constants

        #region Methods

        public static ICalCalendar ParseICalendarFile(string filePath, string countryCode, string countryName)
        {
            ICalCalendar result = new ICalCalendar(countryCode, countryName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(string.Format("Could not find {0}.", filePath));
            }
            if (Path.GetExtension(filePath).Trim().ToLower() != ICALENDAR_FILE_EXTENSION)
            {
                throw new ArgumentException(string.Format(
                    "Invalid file extension on iCalendar file. Expected file extension of '{0}'.", 
                    ICALENDAR_FILE_EXTENSION));
            }
            CalendarCollection calendarCollection = Calendar.LoadFromFile(filePath) as CalendarCollection;
            if (calendarCollection == null)
            {
                throw new NullReferenceException(string.Format("No iCalendar collection found in {0}.", filePath));
            }
            Calendar firstCalendar = calendarCollection.First() as Calendar;
            if (firstCalendar == null)
            {
                throw new NullReferenceException(string.Format("No iCalendar found in {0}.", filePath));
            }
            foreach (Event e in firstCalendar.Events)
            {
                if (e.Class.ToLower() == PUBLIC_HOLIDAY_EVENT_CLASS_NAME) //This is a public holiday event.
                {
                    string name = e.Name;
                    string eventName = e.Summary;
                    string description = e.Description;
                    CalDateTime startDate = e.Start as CalDateTime;
                    CalDateTime endDate = e.End as CalDateTime;
                    if (!startDate.HasDate || !endDate.HasDate)
                    {
                        continue;
                    }
                    result.PublicHolidays.Add(new ICalPublicHoliday(eventName, startDate.Year, startDate.Month, startDate.Day));
                }
            }
            return result;
        }

        #endregion //Methods
    }
}