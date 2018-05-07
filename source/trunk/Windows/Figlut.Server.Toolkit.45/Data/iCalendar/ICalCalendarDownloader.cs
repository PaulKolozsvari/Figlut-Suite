namespace Figlut.Server.Toolkit.Data.iCalendar
{
    using Figlut.Server.Toolkit.Data;
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading.Tasks;

    #endregion //Using Directives

    /// <summary>
    /// Utility class that downloads an .ics file from a website and converts to to an ICalCalendar object.
    /// </summary>
    public class ICalCalendarDownloader
    {
        #region Constructors

        /// <summary>
        /// Provide a URL which contains parameter place holders for the country code, start date and end date.
        /// e.g. a URL such as this http://www.kayaposoft.com/enrico/ics/v1.0?country=zaf&fromDate=01-01-2018&toDate=31-12-2018
        /// would be specified in this constructor as http://www.kayaposoft.com/enrico/ics/v1.0?country={0}&fromDate={1}&toDate={2}
        /// in order to allow for the replacement of the country code {0}, start date {1} and end date {2}.
        /// </summary>
        /// <param name="downloadUrl"></param>
        public ICalCalendarDownloader(string downloadUrl)
        {
            DownloadUrl = downloadUrl;
        }

        #endregion //Constructors

        #region Fields

        private string _downloadUrl;

        #endregion //Fields

        #region Properties

        /// <summary>
        ///  /// Provide a URL which contains parameter place holders for the country code, start date and end date.
        /// e.g. a URL such as this http://www.kayaposoft.com/enrico/ics/v1.0?country=zaf&fromDate=01-01-2018&toDate=31-12-2018
        /// would be specified in this constructor as http://www.kayaposoft.com/enrico/ics/v1.0?country={0}&fromDate={1}&toDate={2}
        /// in order to allow for the replacement of the country code {0}, start date {1} and end date {2}.
        /// </summary>
        public string DownloadUrl
        {
            get { return _downloadUrl; }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new NullReferenceException(string.Format("{0} may not be null or empty when constructing a {1}.",
                        EntityReader<ICalCalendarDownloader>.GetPropertyName(p => p.DownloadUrl, false),
                        typeof(ICalCalendarDownloader).Name));
                }
                _downloadUrl = value;
            }
        }

        #endregion //Properties

        #region Methods

        /// <summary>
        /// Generates a date range with the start date from the 1st of January of the given year to the 31st of December of the given year.
        /// Converts the date time to the following date format when downloading th calendar from the URL: day-month-year e.g. 01-12-2018.
        /// </summary>
        public ICalCalendar DownloadICalCalendar(
            string countryCode,
            string countryName,
            int year,
            string outputFilePath,
            bool deleteOutputFileAfterParsing)
        {
            DateTime startDate = new DateTime(year, 01, 01);
            DateTime endDate = new DateTime(year, 12, 31);
            return DownloadICalCalendar(countryCode, countryName, startDate, endDate, outputFilePath, deleteOutputFileAfterParsing);
        }

        /// <summary>
        /// Converts the date time to the following date format when downloading th calendar from the URL: day-month-year e.g. 01-12-2018.
        /// </summary>
        /// <returns></returns>
        public ICalCalendar DownloadICalCalendar(
            string countryCode,
            string countryName,
            DateTime startDate,
            DateTime endDate,
            string outputFilePath,
            bool deleteOutputFileAfterParsing)
        {
            string startDateString = string.Format("{0}-{1}-{2}", startDate.Day.ToString().PadLeft(2, '0'), startDate.Month.ToString().PadLeft(2, '0'), startDate.Year);
            string endDateString = string.Format("{0}-{1}-{2}", endDate.Day.ToString().PadLeft(2, '0'), endDate.Month.ToString().PadLeft(2, '0'), endDate.Year);
            return DownloadICalCalendar(countryCode, countryName, startDateString, endDateString, outputFilePath, deleteOutputFileAfterParsing);
        }

        /// <summary>
        /// The country code, start date and end date will be applied to the download URL of this ICalCalendarDownloader.
        /// The Country Name is only used for display purposes when constructing the resulting the ICalCalendar.
        /// If the output file name is not specified, a temp file path will be generated in the current user's temp folder.
        /// </summary>
        /// <returns></returns>
        public ICalCalendar DownloadICalCalendar(
            string countryCode, 
            string countryName, 
            string startDate, 
            string endDate,
            string outputFilePath,
            bool deleteOutputFileAfterParsing)
        {
            if (string.IsNullOrEmpty(countryCode))
            {
                throw new NullReferenceException(string.Format("Country Code may not be null or empty when downloading an {0}", typeof(ICalCalendar).Name));
            }
            if (string.IsNullOrEmpty(countryName))
            {
                throw new NullReferenceException(string.Format("Country Name may not be null or empty when downloading an {0}", typeof(ICalCalendar).Name));
            }
            if (string.IsNullOrEmpty(startDate))
            {
                throw new NullReferenceException(string.Format("Start Date may not be null or empty when downloading an {0}", typeof(ICalCalendar).Name));
            }
            if (string.IsNullOrEmpty(endDate))
            {
                throw new NullReferenceException(string.Format("End Date may not be null or empty when downloading an {0}", typeof(ICalCalendar).Name));
            }
            if (string.IsNullOrEmpty(outputFilePath))
            {
                string outputFileName = string.Format("{0}-{1}-{2}{3}", countryCode, startDate, endDate, ICalPublicHolidayParser.ICALENDAR_FILE_EXTENSION);
                outputFilePath = Path.Combine(Path.GetTempPath(), outputFileName);
            }
            using (WebClient webClient = new WebClient())
            {
                string formattedDownloadUrl = string.Format(_downloadUrl, countryCode, startDate, endDate);
                if (File.Exists(outputFilePath))
                {
                    File.Delete(outputFilePath);
                }
                webClient.DownloadFile(formattedDownloadUrl, outputFilePath);
            }
            ICalCalendar result = ICalPublicHolidayParser.ParseICalendarFile(outputFilePath, countryCode, countryName);
            if (deleteOutputFileAfterParsing)
            {
                File.Delete(outputFilePath);
            }
            return result;
        }

        #endregion //Methods
    }
}