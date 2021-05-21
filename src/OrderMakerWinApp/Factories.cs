using ApplicationCore.Helpers;
using ApplicationCore.Managers;
using ApplicationCore.OrderMaker;
using ApplicationCore.Brokages;
using ApplicationCore.OrderMaker.Managers;
using ApplicationCore.OrderMaker.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Brokages.Capital;

namespace OrderMakerWinApp
{
    public class Factories
    {
        public static ISettingsManager CreateSettingsManager() => new SettingsManager();
        public static ITimeManager CreateTimeManager(string begin, string end) => new TimeManager(begin, end);
        public static IPositionManager CreatePositionManager(IOrderMaker orderMaker, TradeSettings tradeSettings, ILogger logger)
        {
            return new PositionManager(orderMaker, tradeSettings, logger);
        }
        public static IOrderMaker CreateOrderMaker(string name, BrokageSettings settings)
        {
            //if (name.EqualTo(ProviderName.FAKE.ToString())) return new FakeOrderMaker(settings);
            //else if (name.EqualTo(ProviderName.HUA_NAN.ToString())) return new HuaNanDDSCOrderMaker(settings);
            //else if (name.EqualTo(ProviderName.CONCORD.ToString())) return new ConcordOrderMaker(settings);
            //else return new CapitalOrderMaker(settings);


            return new CapitalBrokage(settings);
        }

    }
}
