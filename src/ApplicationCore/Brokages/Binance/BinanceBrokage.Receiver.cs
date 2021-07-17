using ApplicationCore.Exceptions;
using ApplicationCore.OrderMaker.Views;
using Binance.Net;
using Binance.Net.Interfaces;
using Binance.Net.Objects.Spot.MarketStream;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Binance.Net.Enums;
using ApplicationCore.Helpers;

namespace ApplicationCore.Brokages.Binance
{
    public partial class BinanceBrokage
    {
        private IBinanceSocketClient _socketClient;
        private BinanceClient _client;
        void InitReceiver()
        {
            _socketClient = new BinanceSocketClient();
            _client = new BinanceClient();
        }

        public void RequestQuotes(IEnumerable<string> symbolCodes)
        {
            InitBinanceCodeSymbolCode(symbolCodes);

            GetHistoryKlines(BinanceSymbolCodes.FirstOrDefault(), 2021,5, 20);

            //var result = _socketClient.Spot.SubscribeToTradeUpdates(BinanceSymbolCodes, OnTradeData);
            //if (!result.Success)
            //{
            //    throw new KLineSourceException(result.Error.ResolveBinanceError("RequestKLines.SubscribeToTradeUpdates"));
            //}

            //var result = _socketClient.Spot.SubscribeToKlineUpdates(BinanceSymbolCodes, OnTradeData);
            //if (!result.Success)
            //{
            //    throw new KLineSourceException(result.Error.ResolveBinanceError("RequestKLines.SubscribeToTradeUpdates"));
            //}
        }

        void OnTradeData(BinanceStreamTrade data)
        { 
            
        }

        void GetHistoryKlines(string symbol, int year, int month, int day)
        {
            var start = new DateTime(year, month, day);
            for (int i = 0; i < 24; i++)
            {
                var end = start.AddHours(1).AddSeconds(-1);
                var result = _client.Spot.Market.GetKlines(symbol, KlineInterval.OneMinute, start, end);
                if (result.Success)
                {
                    var list = result.Data.Select(x => x.ToKLineView());

                }
                else
                {
                    throw new QuoteSourceException(result.Error.ResolveBinanceError("GetKlines"));
                }


                start = start.AddHours(1);
            }
            
        }

        void OnKLineData()
        { 
            
        }
        void StopQuote() => _socketClient.UnsubscribeAll().Wait();
    }
}
