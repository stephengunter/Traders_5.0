using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Receiver.Services;

namespace Web
{
    public class Factories
    {
        public static IFuturesService CreateFuturesService(Symbol symbol, TradeSession tradeSession) 
            => new FuturesService(symbol, tradeSession);
    }
}
