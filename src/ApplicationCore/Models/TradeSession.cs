using ApplicationCore.Helpers;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;

namespace ApplicationCore.Models
{
    public class TradeSession : BaseEntity
    {
        public bool Default { get; set; }
        public int SymbolId { get; set; }
        public int Open { get; set; }
        public int Close { get; set; }

        public Symbol Symbol { get; set; }


        public List<int> GetKLineTimes(DateTime date)
        {
            int open = this.Open;
            int close = this.Close;

            var openTimes = open.ToTimes();
            DateTime openTime = new DateTime(date.Year, date.Month, date.Day, openTimes[0], openTimes[1], openTimes[2]);

            var closeTimes = close.ToTimes();
            DateTime closeTime = new DateTime(date.Year, date.Month, date.Day, closeTimes[0], closeTimes[1], closeTimes[2]);

            if (closeTime <= openTime) closeTime = closeTime.AddDays(1);


            var times = new List<int>();
            DateTime time = openTime.AddMinutes(1);
            while (time <= closeTime)
            {
                times.Add(time.ToTimeNumber());
                time = time.AddMinutes(1);
            }
            return times;
        }
    }
}
