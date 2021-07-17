using ApplicationCore.Helpers;
using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface IFuturesService
    {
        string GetYearMonth(DateTime dateTime, Symbol symbol);
    }
    public class FuturesService : IFuturesService
    {
        Dictionary<int, string> _monthDictionary = new Dictionary<int, string>
        {
            { 1, "A" },{ 2, "B" },{ 3, "C" },
            { 4, "D" },{ 5, "E" },{ 6, "F" },
            { 7, "G" },{ 8, "H" },{ 9, "I" },
            { 10, "J" },{ 11, "K" },{ 12, "L" }
        };

        DateTime GetCashDate(DateTime dateTime)
        {
            var cashDate = dateTime.FindThirdWedOfMonth(); //本月結算日
            return new DateTime(cashDate.Year, cashDate.Month, cashDate.Day, 13, 30, 0);
        }

        public string GetYearMonth(DateTime dateTime, Symbol symbol)
        {
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
    }
}
