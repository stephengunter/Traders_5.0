﻿using Infrastructure.Views;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Helpers
{
	public static class DateTimeHelpers
	{
		public static DateTime? ToDatetimeOrNull(this string str)
		{
			DateTime dateValue;
			if (DateTime.TryParse(str, out dateValue)) return dateValue;

			return null;

		}

		public static DateTime ToDatetimeOrDefault(this string str, DateTime defaultValue)
		{
			DateTime dateValue;
			if (DateTime.TryParse(str, out dateValue)) return dateValue;

			return defaultValue;

		}

		public static DateTime ToDatetime(this int val)
		{
			var strVal = val.ToString();

			int year = strVal.Substring(0, 4).ToInt();
			int month = strVal.Substring(4, 2).ToInt();
			int day = strVal.Substring(6, 2).ToInt();

			return new DateTime(year, month, day);

		}

		public static DateTime? ToStartDate(this string input)
		{
			var startDate = input.ToDatetimeOrNull();
			if (startDate.HasValue)
			{
				var dateStart = Convert.ToDateTime(startDate);
				return new DateTime(dateStart.Year, dateStart.Month, dateStart.Day, 0, 0, 0);
			}
			else return null;
		}

		public static DateTime? ToEndDate(this string input)
		{
			var endDate = input.ToDatetimeOrNull();
			if (endDate.HasValue)
			{
				var dateEnd = Convert.ToDateTime(endDate);
				return dateEnd.ToEndDate();
			}
			else return null;
		}

		public static DateTime ToEndDate(this DateTime date)
		{
			return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
		}

		public static DateTime ConvertToTaipeiTime(this DateTime input)
		{
			string taipeiTimeZoneId = "Taipei Standard Time";
			TimeZoneInfo taipeiTimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(taipeiTimeZoneId);
			return TimeZoneInfo.ConvertTimeFromUtc(input.ToUniversalTime(), taipeiTimeZoneInfo);

		}
		public static int ToDateNumber(this DateTime input)
			=> Convert.ToInt32(GetDateString(input.Date));

		public static int ToTimeNumber(this DateTime input)
			=> Convert.ToInt32(GetTimeString(input));
        

		public static string ToTimeString(this int val)
		{
			string str = val.ToString();
			while (str.Length < 6) str = "0" + str;

			string h = str.Substring(0, 2);
			string m = str.Substring(2, 2);
			string s = str.Substring(4, 2);

			return String.Format("{0}:{1}:{2}", h, m, s);

		}

		public static string ToTimeString(this DateTime input)
			=> input.ToString("H:mm:ss");

		public static string ToDateString(this DateTime input)
			=> input.ToString("yyyy-MM-dd");

		public static string ToDateString(this DateTime? input)
			=> input.HasValue ? input.Value.ToDateString() : "";

		public static string ToDateTimeString(this DateTime input)
			=> input.ToString("yyyy-MM-dd H:mm:ss");

		public static string ToDateTimeString(this DateTime? input)
			=> input.HasValue ? input.Value.ToDateTimeString() : "";

		public static string GetDateString(DateTime dateTime)
		{
			string year = dateTime.Year.ToString();
			string month = dateTime.Month.ToString();
			string day = dateTime.Day.ToString();

			if (dateTime.Month < 10) month = "0" + month;
			if (dateTime.Day < 10) day = day = "0" + day;


			return year + month + day;
		}

		public static DateTime? GetDate(this string val)
		{
			if (val.Length != 8) return null;

			int year = val.Substring(0, 4).ToInt();
			int month = val.Substring(4, 2).ToInt();
			int day = val.Substring(6, 2).ToInt();

			if(year == 0 || month == 0 || day == 0) return null;
			if(month > 12 || day > 31) return null;

			return new DateTime(year, month, day);

		}

		public static DateTime? GetDate(this int val) => val.ToString().GetDate();

		public static List<BaseOption<int>> GetYearOptions(this DateTime fromDate, bool chinese = false)
		{
			int currentYear = DateTime.Now.Year;
			var options = new List<BaseOption<int>>();
			for (int i = fromDate.Year; i <= currentYear; i++)
			{
				options.Add(new BaseOption<int>(i, i.ToString()));
			}
			return options;
		}

		static string GetTimeString(DateTime dateTime, bool toMileSecond = false)
		{
			string hour = dateTime.Hour.ToString();
			string minute = dateTime.Minute.ToString();
			string second = dateTime.Second.ToString();
			string mileSecond = dateTime.Millisecond.ToString();

			if (dateTime.Hour < 10) hour = "0" + hour;
			if (dateTime.Minute < 10) minute = "0" + minute;
			if (dateTime.Second < 10) second = "0" + second;

			if (!toMileSecond) return hour + minute + second;


			if (dateTime.Millisecond < 10)
			{
				mileSecond = "00" + mileSecond;
			}
			else if (dateTime.Millisecond < 100)
			{
				mileSecond = "0" + mileSecond;
			}

			return hour + minute + second + mileSecond;

		}



		

	}
}
