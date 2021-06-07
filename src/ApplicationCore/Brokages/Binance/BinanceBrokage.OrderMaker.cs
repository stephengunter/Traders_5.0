using ApplicationCore.OrderMaker.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Brokages.Binance
{
    public partial class BinanceBrokage
    {
        public override void FetchDeals(string account)
        {
            throw new NotImplementedException();
        }

        public override List<DealViewModel> GetDeals()
        {
            throw new NotImplementedException();
        }

        public override void MakeOrder(string symbol, string account, decimal price, int lots, bool dayTrade)
        {
            throw new NotImplementedException();
        }

        public override void RequestAccountPositions(string account, string symbol = "")
        {
            throw new NotImplementedException();
        }

       
    }
}
