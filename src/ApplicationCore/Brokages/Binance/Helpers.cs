using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using ApplicationCore.Views;
using Binance.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace ApplicationCore.Brokages.Binance
{
    public static class BinanceHelpers
    {
        public static string ResolveBinanceError(this Error? error, string action)
        {
            if (error == null) return "";
            string code = error.Code.HasValue ? Convert.ToInt32(error.Code).ToString() : "";
            return $"Action: {action}, Code: {code}, Msg: {error.Message}";
        }

        public static KLineViewModel ToKLineView(this IBinanceKline data)
            => new KLineViewModel()
            {
                Date = "",// data.CloseTime.ToDateNumber(),
                Time = "",//data.CloseTime.ToTimeNumber(),
                Open = data.Open,
                High = data.High,
                Low = data.Low,
                Price = data.Close,
                Vol = data.CommonVolume
            };

    }
    
}
