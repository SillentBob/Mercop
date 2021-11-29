using System;

namespace Mercop.Core.Web.Data
{
    public static class DateFormatter
    {
        private const string DATETIME_FORMAT = "yyyy-MM-dd\\THH:mm:ss\\Z";

        public static string FormatDatetimeToString(DateTime dateTime)
        {
            string datetime = dateTime.ToString(DATETIME_FORMAT);
            return datetime;
        }

        public static DateTime FormatStringToDatetime(string dateTime)
        {
            var dateValue = DateTime.Parse(dateTime, null);
            return dateValue;
        }
    }
}