﻿using ApplicationCore.Exceptions;
using ApplicationCore.Helpers;
using ApplicationCore.Managers;
using ApplicationCore.OrderMaker.Views;
using ApplicationCore.Receiver.Views;
using Concord.API.Future.Client.OrderFormat;
using NLog;
using SKCOMLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormLibrary.Brokages.Concord
{
    public partial class ConcordBrokage
    {
        private const string _BHNO = "000";
        FOrderNew InitOrder(string symbol, string account, decimal price, int lots, bool dayTrade)
        {
            //大台 TXF 小台 MXF
            string productId = GetProductId(symbol);
            string rtype = dayTrade ? "2" : "";  // "0":新倉, "1":平倉, "2":期貨當沖, "":自動
                                                 //string fir = price > 0 ? "R" : "I";  //R:ROD, F:FOK, I:IOC
            string fir = "I";  //R:ROD, F:FOK, I:IOC
            string otype = price > 0 ? "L" : "P";  // L:限價, M:市價, P:一定範圍市價

            return new FOrderNew
            {
                bhno = _BHNO,
                cseq = account,
                mtype = "F", //F:期貨, O:選擇權
                sflag = "1", //期貨單式
                commo = productId,
                fir = fir,
                rtype = rtype,
                otype = otype,
                bs = lots > 0 ? "B" : "S",
                price = price > 0 ? price : 0,
                qty = Math.Abs(lots)
            };
        }

        bool GetCertStatus()
        {
            string code = _API.GetCertStatus(SID, out string strSDate, out string strEDate, out string msg);

            OnActionExecuted("GetCertStatus", code, msg);
            return (code == "000");
        }
        public override void MakeOrder(string symbol, string account, decimal price, int lots, bool dayTrade)
        {
            var order = InitOrder(symbol, account, price, lots, dayTrade);

            string strGUID = Guid.NewGuid().ToString();
            string code = _API.FOrderNew(order, out string msg, ref strGUID);

            OnActionExecuted("MakeOrder", code, msg);
        }

        public override void RequestAccountPositions(string account, string symbol = "")
        {
            throw new NotImplementedException();
        }
        public override string ClearOrders(string symbol, string account)
        {
            throw new NotImplementedException();
        }
        public override void FetchDeals(string account)
        {
            throw new NotImplementedException();
        }

        public override List<DealViewModel> GetDeals()
        {
            throw new NotImplementedException();
        }


    }
}
