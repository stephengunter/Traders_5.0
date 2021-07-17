using ApplicationCore.Helpers;
using ApplicationCore.Services;
using ApplicationCore.Settings;
using ApplicationCore.Views;
using ApplicationCore.ViewServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Auth.ApiKey;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore;
using Web.Models;
using System;
using System.Collections.Generic;
using ApplicationCore.Models;

namespace Web.Controllers.Tests
{
    public class ATestsController : BaseTestController
    {
        
        private readonly AppSettings _appSettings;
        private readonly ISymbolsService _symbolsService;
        private readonly IFuturesService _futuresService;
        public ATestsController(IOptions<AppSettings> appSettings, ISymbolsService symbolsService, IFuturesService futuresService)
        {
            _appSettings = appSettings.Value;
            _symbolsService = symbolsService;
            _futuresService = futuresService;
        }


        
        [HttpGet]
        public async Task<ActionResult> Index(int dn)
        {
            var symbol = _symbolsService.GetByCode("TX");
            DateTime? date = dn.GetDate();
            string yearMonth = _futuresService.GetYearMonth(date.Value, symbol);


            var ticks = new List<Tick>();
            foreach (var tradeSession in symbol.TradeSessions)
            {
                if (tradeSession.Default)
                {
                    var kLineTimes = tradeSession.GetKLineTimes(date.Value);
                }
                else
                {
                    var kLineTimes = tradeSession.GetKLineTimes(date.Value);
                    foreach (var time in kLineTimes)
                    {
                        var kline = ticks.GetKLine(time);


                    }
                }
            }

            return Ok(yearMonth);

        }
        [HttpGet("version")]
        public ActionResult Version()
        {
            return Ok(_appSettings.ApiVersion);
        }


        [HttpGet("ex")]
        public ActionResult Ex()
        {
            throw new System.Exception("Test Throw Exception");
        }
    }
}
