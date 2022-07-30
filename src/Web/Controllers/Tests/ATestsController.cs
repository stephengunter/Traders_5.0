using ApplicationCore.Helpers;
using ApplicationCore.Services;
using ApplicationCore.Settings;
using ApplicationCore.Models;
using ApplicationCore.Views;
using ApplicationCore.ViewServices;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using Web.Models;
using System.Globalization;
using ApplicationCore.Paging;
using ApplicationCore;
using ApplicationCore.Receiver.Services;

namespace Web.Controllers.Tests
{
    public class ATestsController : BaseTestController
    {
        private readonly IUsersService _usersService;
        private readonly ISymbolsService _symbolsService;
        private readonly IRealTimeService _realTimeService;
        private readonly ITicksService _ticksService;
        private readonly AdminSettings _adminSettings;
        private readonly IMapper _mapper;

        private IFuturesService _futuresService;

        public ATestsController(IUsersService usersService, ISymbolsService symbolsService, IRealTimeService realTimeService, ITicksService ticksService, IOptions<AdminSettings> adminSettings, IMapper mapper)
        {
            _usersService = usersService;
            _symbolsService = symbolsService;
            _realTimeService = realTimeService;
            _ticksService = ticksService;
            _adminSettings = adminSettings.Value;
            _mapper = mapper;

            
        }
        
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var txSymbol = _symbolsService.GetByCode(SymbolCodes.TX);
            var tradeSession = txSymbol.TradeSessions.FirstOrDefault(x => x.Default);
            _futuresService = Factories.CreateFuturesService(txSymbol, tradeSession);


            var allTicks = await _ticksService.FetchAllAsync(txSymbol.Code);

            var keys = _futuresService.DictionaryTicks.Keys.ToList();
            var kLines = new List<KLine>();
            
            for (int i = 0; i < keys.Count; i++) kLines.Add(allTicks.GetKLine(keys[i]));

            foreach (var kLine in kLines)
            {
                await _realTimeService.AddUpdateAsync(kLine);
            }

            return Ok();
        }

        [HttpGet("version")]
        public ActionResult Version()
        {
           

            return Ok();
        }


        [HttpGet("ex")]
        public ActionResult Ex()
        {
            throw new System.Exception("Test Throw Exception");
        }
    }
}
