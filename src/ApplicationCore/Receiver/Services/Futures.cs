using ApplicationCore.Helpers;
using ApplicationCore.Models;
using ApplicationCore.Views;
using ApplicationCore.ViewServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Receiver.Services
{
	public interface IFuturesService
	{
        Dictionary<int, List<Tick>> DictionaryTicks { get; }
    }

	public class FuturesService : IFuturesService
    {
		private readonly Symbol _symbol;
		private readonly TradeSession _tradeSession;

        Dictionary<int, List<Tick>> _dictionaryTicks = new Dictionary<int, List<Tick>>();

        public FuturesService(Symbol symbol, TradeSession tradeSession)
		{
			_symbol = symbol;
			_tradeSession = tradeSession;

            var openTimes = tradeSession.Open.ToTimes();
            var open = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                                openTimes[0], openTimes[1], openTimes[2]
                                );

            var closeTimes = tradeSession.Close.ToTimes();
            var close = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day,
                                closeTimes[0], closeTimes[1], closeTimes[2]
                                );

            var time = new DateTime(open.Year, open.Month, open.Day,
                                    open.Hour, open.Minute, open.Second
                                   );

            time = time.AddMinutes(1);
            while (time <= close)
            {
                _dictionaryTicks.Add(time.ToTimeNumber(), new List<Tick>());
                time = time.AddMinutes(1);
            }
        }


        public Dictionary<int, List<Tick>> DictionaryTicks => _dictionaryTicks;


    }

}
