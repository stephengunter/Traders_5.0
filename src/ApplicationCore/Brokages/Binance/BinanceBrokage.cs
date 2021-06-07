using ApplicationCore.Managers;
using ApplicationCore.OrderMaker;
using ApplicationCore.OrderMaker.Views;
using ApplicationCore.Receiver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Brokages.Binance
{
    public partial class BinanceBrokage : BaseOrderMaker, IQuoteSource
    {
        public BinanceBrokage(BrokageSettings settings) : base(BrokageName.BINANCE, settings)
        {
            InitReceiver();
        }

        public event EventHandler NotifyTick;

        public override string ClearOrders(string symbol, string account)
        {
            throw new NotImplementedException();
        }

        public override void Connect()
        {
            
        }

        public override void DisConnect()
        {
            StopQuote();
        }

        
    }
}
