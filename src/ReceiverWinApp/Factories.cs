using ApplicationCore.Brokages;
using ApplicationCore.Brokages.Binance;
using ApplicationCore.Managers;
using ApplicationCore.Receiver;
using ApplicationCore.Receiver.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiverWinApp
{
    public class Factories
    {
        public static ISettingsManager CreateSettingsManager() => new SettingsManager();
        public static ITimeManager CreateTimeManager(string begin, string end) => new TimeManager(begin, end);
       
        public static IQuoteSource CreateQuoteSource(string name, BrokageSettings settings)
        {
            //if (name.EqualTo(ProviderName.FAKE.ToString())) return new FakeOrderMaker(settings);
            //else if (name.EqualTo(ProviderName.HUA_NAN.ToString())) return new HuaNanDDSCOrderMaker(settings);
            //else if (name.EqualTo(ProviderName.CONCORD.ToString())) return new ConcordOrderMaker(settings);
            //else return new CapitalOrderMaker(settings);


            //return new CapitalBrokage(settings);
            return new BinanceBrokage(settings);
        }

        public static IFuturesLocalService CreateFuturesLocalService() => new FuturesLocalService();

    }
}
