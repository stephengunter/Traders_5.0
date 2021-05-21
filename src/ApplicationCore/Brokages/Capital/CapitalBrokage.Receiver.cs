using ApplicationCore.Exceptions;
using ApplicationCore.Helpers;
using ApplicationCore.Managers;
using ApplicationCore.Receiver;
using ApplicationCore.Receiver.Views;
using NLog;
using SKCOMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Brokages.Capital
{
    public partial class CapitalBrokage
    {
        private SKQuoteLib _SKQuoteLib;
        private Dictionary<short, string> _symbolIndexCode = new Dictionary<short, string>();
        private Dictionary<short, double> _symbolIndexPoints = new Dictionary<short, double>();

        void InitReceiver()
        {
            _SKQuoteLib = new SKQuoteLib();
            _SKQuoteLib.OnConnection += SKQuoteLib_OnConnection;
            _SKQuoteLib.OnNotifyHistoryTicks += SKQuoteLib_OnNotifyHistoryTicks;
            _SKQuoteLib.OnNotifyTicks += SKQuoteLib_OnNotifyTicks;
            //_SKQuoteLib.OnNotifyStockList += SKQuoteLib_OnNotifyStockList;
        }

        void ConnectQuoteSource()
        {
            int code = _SKQuoteLib.SKQuoteLib_EnterMonitor();
            OnActionExecuted("SKQuoteLib_EnterMonitor", code);
        }

        IEnumerable<string> _symbolCodes = new List<string>();
        public void RequestQuotes(IEnumerable<string> symbolCodes)
        {
            if(symbolCodes.HasItems()) _symbolCodes = symbolCodes;

            if (_symbolIndexCode.Count < 1) InitSymbolIndexCode();

            RegisterQuote();
        }

        void RegisterQuote()
        {
            short sPage = 1;
            var sCodes = _symbolIndexCode.Values;

            foreach (var sCode in sCodes)
            {
                int code = _SKQuoteLib.SKQuoteLib_RequestTicks(sPage, sCode);
                if (code != 0)
                {
                    OnExceptionHappend("SKQuoteLib_RequestTicks", code);
                }
                sPage++;
            }
        }




        #region Test
        
        //收集商品, 測試用
        void RequestStockList()
        {
            //0:上市   1:上櫃    2:期貨    3:選擇權   4:興櫃
            for (int m = 0; m <= 3; m++)
            {
                short sMarketNo = Convert.ToInt16(m);
                int code = _SKQuoteLib.SKQuoteLib_RequestStockList(sMarketNo);
            }



        }
        Dictionary<short, List<string>> _stockList = new Dictionary<short, List<string>>();
        private void SKQuoteLib_OnNotifyStockList(short sMarketNo, string bstrStockData)
        {

            if (!_stockList.ContainsKey(sMarketNo))
            {
                _stockList.Add(sMarketNo, new List<string>());
            }

            _stockList[sMarketNo].Add(bstrStockData);

        }

        #endregion

        #region Helpers
        int _date = DateTime.Today.ToDateNumber();
        const string TX_SYMBOL_KEY = "TX00";

        void InitSymbolIndexCode()
        {
            _symbolIndexCode = new Dictionary<short, string>();
            _symbolIndexPoints = new Dictionary<short, double>();

            var tx = GetSKSTOCKByCode(TX_SYMBOL_KEY);
            _symbolIndexCode[tx.sStockIdx] = TX_SYMBOL_KEY;

            double txPoints = 1;
            for (int i = 0; i < tx.sDecimal; i++)
            {
                txPoints *= 10;
            }
            _symbolIndexPoints[tx.sStockIdx] = txPoints;

            if (_symbolCodes.IsNullOrEmpty()) return;

            foreach (var code in _symbolCodes)
            {
                var pSKStock = GetSKSTOCKByCode(code);
                _symbolIndexCode[pSKStock.sStockIdx] = code;

                double symbolPoints = 1;
                for (int i = 0; i < pSKStock.sDecimal; i++)
                {
                    symbolPoints *= 10;
                }
                _symbolIndexPoints[pSKStock.sStockIdx] = symbolPoints;
            }
        }
        SKSTOCK GetSKSTOCKByCode(string code)
        {
            SKSTOCK pSKStock = new SKSTOCK();
            int nCode = _SKQuoteLib.SKQuoteLib_GetStockByNo(code, ref pSKStock);
            return pSKStock;
        }
        

        bool IsStock(string code) => code != TX_SYMBOL_KEY;
        #endregion 

        

        public event EventHandler NotifyStockTick;
        public event EventHandler NotifyFuturesTick;
        

        void SKQuoteLib_OnConnection(int nKind, int nCode)
        {
            OnActionExecuted("SKQuoteLib_OnConnection", nCode);

            if (nKind == 3001 && nCode == 0)  //報價連線OK
            {
               
            }
            else if (nKind == 3003 && nCode == 0) //報價商品載入完成
            {
                SetConnectionStatus(ConnectionStatus.CONNECTED);
                //Test
                //RequestStockList();

            }
            else if (nKind == 3002 && nCode == 0) //SK_SUBJECT_CONNECTION_DISCONNECT
            {
                SetConnectionStatus(ConnectionStatus.DISCONNECTED);
            }
        }

        void SKQuoteLib_OnNotifyTicks(short sMarketNo, short sStockIdx, int nPtr, int nDate, int lTimehms, int lTimemillismicros, int nBid, int nAsk, int nClose, int nQty, int nSimulate)
        {
            if (nSimulate > 0) return;

            if (nDate == _date)
            {
                bool realTime = true;
                HandleTickNotify(realTime, sStockIdx, nPtr, lTimehms, nBid, nAsk, nClose, nQty);
            }

        }

        void SKQuoteLib_OnNotifyHistoryTicks(short sMarketNo, short sStockIdx, int nPtr, int nDate, int lTimehms, int lTimemillismicros, int nBid, int nAsk, int nClose, int nQty, int nSimulate)
        {
            if (nSimulate > 0) return;

            if (nDate == _date)
            {
                bool realTime = false;
                HandleTickNotify(realTime, sStockIdx, nPtr, lTimehms, nBid, nAsk, nClose, nQty);
            }

        }

        void HandleTickNotify(bool realTime, short sStockIdx, int nPtr, int lTimehms, int nBid, int nAsk, int nClose, int nQty)
        {
            string code = _symbolIndexCode[sStockIdx];
            double symbolPoints = _symbolIndexPoints[sStockIdx];

            //if (!InTime(code, lTimehms)) return;
            var tick = new TickViewModel
            {
                Order = nPtr,
                Time = lTimehms,
                Bid = nBid / symbolPoints,
                Offer = nAsk / symbolPoints,
                Price = nClose / symbolPoints,
                Qty = nQty
            };

            var e = new TickEventArgs(code, tick, realTime);
            if (IsStock(code)) NotifyStockTick?.Invoke(this, e);
            else NotifyFuturesTick?.Invoke(this, e);
        }

       

    }
}
