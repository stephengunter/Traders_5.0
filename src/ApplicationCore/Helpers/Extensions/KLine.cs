using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApplicationCore.Models;

namespace ApplicationCore.Helpers
{
    public static class KLineHelpers
    {
        public static KLine GetKLine(this IEnumerable<Tick> ticks, int time)
        {
            var beginEndTimes = time.ResolveBeginEndTime();
			int begin = beginEndTimes[0];
			int end = beginEndTimes[1];

			var tickList = ticks.Where(x => x.Time >= begin && x.Time < end).OrderBy(t => t.Order);

			if (tickList.IsNullOrEmpty()) return null;

			var firstTick = tickList.First();
			var lastTick = tickList.Last();
			string symbol = firstTick.Symbol;
			int date = firstTick.Date;

			return new KLine
			{
				Symbol = symbol,
				Date = date,
				Time = end,
				High = tickList.Max(t => t.Price),
				Low = tickList.Min(t => t.Price),
				Open = firstTick.Price,
				Price = lastTick.Price,
				Vol = tickList.Sum(x => x.Qty)
			};
		}

		public static int[] ResolveBeginEndTime(this int time)
		{
			var today = DateTime.Today;
			var endTimes = time.ToString().ToTimes();

			var end = new DateTime(today.Year, today.Month, today.Day, endTimes[0], endTimes[1], endTimes[2]);
			var begin = end.AddMinutes(-1);

			return new int[] { begin.ToTimeNumber(), end.ToTimeNumber() };

		}

		//public static bool IsKLineStart(this Tick tick) => (tick.Time % 100) == 0;

		//public static int ToKLineTime(this int time)
		//{
		//	var today = DateTime.Today;
		//	var endTimes = time.ToString().ToTimes();
		//	int second = endTimes[2];
		//	var end = new DateTime(today.Year, today.Month, today.Day, endTimes[0], endTimes[1], second);

		//	if (second == 0) return end.AddMinutes(1).ToTimeNumber();

		//	return end.AddMinutes(1).AddSeconds(0 - second).ToTimeNumber();
		//}
	}
}
