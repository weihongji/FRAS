using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility
{
	public class DateUtility
	{
		/// <summary>
		/// Get date of the first day in the month of parameter theDate.
		/// </summary>
		/// <param name="theDate"></param>
		/// <returns></returns>
		public static DateTime GetFirstDay(DateTime theDate) {
			return new DateTime(theDate.Year, theDate.Month, 1);
		}

		/// <summary>
		/// Get date of the last day in the month of parameter theDate.
		/// </summary>
		/// <param name="theDate"></param>
		/// <returns></returns>
		public static DateTime GetLastDay(DateTime theDate) {
			DateTime lastDate;
			int days;
			days = DateTime.DaysInMonth(theDate.Year, theDate.Month);
			lastDate = new DateTime(theDate.Year, theDate.Month, days);
			return lastDate;
		}

		public static int DateDiff(DateInterval interval, DateTime start, DateTime end) {
			int diff;
			TimeSpan ts = end - start;
			switch (interval) {
				case DateInterval.Day:
					diff = ts.Days;
					break;
				case DateInterval.Hour:
					diff = ts.Hours;
					break;
				case DateInterval.Minute:
					diff = ts.Minutes;
					break;
				case DateInterval.Second:
					diff = ts.Seconds;
					break;
				default:
					diff = ts.Days;
					break;
			}
			return diff;
		}

		public enum DateInterval
		{
			Day = 1,
			Hour = 2,
			Minute = 3,
			Second = 4
		}

		public static string FormatTime(string time, string format = "HH:mm") {
			DateTime date_time;
			if (time.Length <= 11) { // 11 is the max length of a time, such as 10:00:00 pm
				time = "2000-1-1 " + time;
			}
			bool success = DateTime.TryParse(time, out date_time);
			if (success) {
				return date_time.ToString(format);
			}
			else {
				return time;
			}
		}
	}
}
