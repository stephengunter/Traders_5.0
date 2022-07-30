using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Helpers
{
    public static class TXHelpers
    {
		public static System.DateTime GetCashDate(this DateTime dateTime)
		{
			var cashDate = dateTime.FindThirdWedOfMonth(); //本月結算日
			return new System.DateTime(cashDate.Year, cashDate.Month, cashDate.Day, 13, 30, 0);
		}

		public static string GetYearMonth(this Symbol symbol, DateTime dateTime)
		{
			Dictionary<int, string> _monthDictionary = new Dictionary<int, string>
			{
				{ 1, "A" },{ 2, "B" },{ 3, "C" },
				{ 4, "D" },{ 5, "E" },{ 6, "F" },
				{ 7, "G" },{ 8, "H" },{ 9, "I" },
				{ 10, "J" },{ 11, "K" },{ 12, "L" }
			};

			// TX
			int year = dateTime.Year;
			int month = dateTime.Month;

			var cashDay = GetCashDate(dateTime);
			if (dateTime > cashDay)
			{
				month += 1;
				if (month > 12)
				{
					year += 1;
					month = 1;
				}
			}

			if (month < 10) return $"{year}0{month}";
			return $"{year}{month}";

		}

		public static System.DateTime FindThirdWedOfMonth(this DateTime date)
		{
			System.DateTime thirdWed = new System.DateTime(date.Year, date.Month, 15);

			while (thirdWed.DayOfWeek != DayOfWeek.Wednesday)
			{
				thirdWed = thirdWed.AddDays(1);
			}

			return thirdWed;

		}

		public static int[] ToTimes(this int val) => val.ToString().ToTimes();
		public static int[] ToTimes(this string strVal)
		{
			while (strVal.Length < 6) strVal = "0" + strVal;

			var hour = strVal.Substring(0, 2).ToInt();
			var minute = strVal.Substring(2, 2).ToInt();
			var second = strVal.Substring(4, 2).ToInt();


			return new int[] { hour, minute, second };
		}
	}
}
