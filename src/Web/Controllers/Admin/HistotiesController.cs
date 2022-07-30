using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ApplicationCore.Views;
using ApplicationCore.Helpers;
using AutoMapper;
using ApplicationCore.ViewServices;
using Web.Models;
using Web.Helpers;
using Web.Controllers;
using ApplicationCore.Paging;
using System.IO;
using System.Text;

namespace Web.Controllers.Admin
{
	public class HistotiesController : BaseAdminController
	{
		private readonly ISymbolsService _symbolsService;
        private readonly IHistoriesService _historiesService;
        private readonly IMapper _mapper;

		public HistotiesController(ISymbolsService symbolsService, IHistoriesService historyKLinesService,
            IMapper mapper)
		{
			_symbolsService = symbolsService;
            _historiesService = historyKLinesService;
            
            _mapper = mapper;
		}

		[HttpGet("")]
        public async Task<ActionResult> Index(string symbol = "", string date = "", int page = 1, int pageSize = 10)
        {
            if (page < 1) //初次載入頁面
            {
                return await FirstLoad();
            }

            var givenDate = date.ToDatetimeOrNull();
            Symbol givenSymbol = null;
            if(!String.IsNullOrEmpty(symbol)) givenSymbol = _symbolsService.GetByCode(symbol);

            if (!givenDate.HasValue && givenSymbol == null)
            {
                return NotFound();
            }

            IEnumerable<KLineGroupViewModel> groupViewList = new List<KLineGroupViewModel>();

            if (givenDate.HasValue)
            {
                var dateNumber = givenDate.Value.ToDateNumber();

                if (givenSymbol == null)
                {
                    groupViewList = await _historiesService.FetchGroupByDateAsync(dateNumber);
                    return FetchResult(groupViewList, page, pageSize);
                }
                else
                {
                   
                    groupViewList = await _historiesService.FetchGroupAsync(givenSymbol, dateNumber);
                    return FetchResult(groupViewList, page, pageSize);
                }
            }
            else
            {
                if (givenSymbol == null)
                {
                    ModelState.AddModelError("symbol", "symbol不存在");
                    return BadRequest(ModelState);
                }

                groupViewList = await _historiesService.FetchGroupAsync(givenSymbol);
                return FetchResult(groupViewList, page, pageSize);
            }

        }

        #region Fetch
        async Task<ActionResult> FirstLoad()
        {
            var model = new HistoryKLinesAdminModel();
            var symbols = await _symbolsService.FetchAsync();
            model.LoadSymbolsOptions(symbols);

            return Ok(model);
        }

        ActionResult FetchResult(IEnumerable<KLineGroupViewModel> groupViewList, int page, int pageSize)
        {
            var model = new HistoryKLinesAdminModel();
            var pageList = new PagedList<KLineGroupViewModel, KLineGroupViewModel>(groupViewList, page, pageSize);

            pageList.ViewList = groupViewList.ToList();

            model.PagedList =  pageList;
            return Ok(model);
        }

        #endregion

        [HttpGet("details")]
        public async Task<ActionResult> Details(string symbol, int date, int page = 1, int pageSize = 10)
        {
            if (String.IsNullOrEmpty(symbol)) return NotFound();

            Symbol givenSymbol = _symbolsService.GetByCode(symbol);
            if (givenSymbol == null) return NotFound();

            var quotes = await _historiesService.FetchAsync(givenSymbol, date);

            quotes = quotes.GetOrdered();

            return Ok(quotes.GetPagedList(_mapper, page, pageSize));
        }


        

        [HttpPost("import")]
        public async Task<IActionResult> Import([FromForm] HistoryKLinesImportForm model)
        {
            ValidateRequest(model);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var symbol = _symbolsService.GetByCode(model.Symbol);
            if (symbol == null)
            {
                ModelState.AddModelError("symbol", "symbol不存在");
                return BadRequest(ModelState);
            }

            
            var allTicks = new List<Tick>();
            var file = model.File;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                { 
                    string line = await reader.ReadLineAsync();
                    var parts = line.SplitToList();
                    string code = parts[1].Trim();
                    if (code.EqualTo(symbol.Code))
                    {
                        allTicks.Add(new Tick
                        {
                            Symbol = symbol.Code,
                            Date = parts[0].ToInt(),
                            Time = parts[3].ToInt(),
                            Price = Convert.ToDecimal(parts[4]),
                            Order = parts[2].ToInt(), //到期月份
                            Qty = parts[5].ToInt()
                        });
                    }                
                }
            }

            if (allTicks.IsNullOrEmpty())
            {
                ModelState.AddModelError("count", "沒有符合的tick資料.");
                return BadRequest(ModelState);
            }
            
            var dates = allTicks.Select(x => x.Date).Distinct();

            foreach (var tradeSession in symbol.TradeSessions)
            {
                int dateNumber = dates.Max();
                DateTime? date = dateNumber.GetDate();
                if (!date.HasValue)
                {
                    ModelState.AddModelError("date", "無法轉換成有效的日期");
                    return BadRequest(ModelState);
                }

                await ImportKLinesAsync(symbol, date.Value, tradeSession, allTicks);
            }

            return Ok();
        }
       
        async Task ImportKLinesAsync(Symbol symbol, DateTime date, TradeSession tradeSession, List<Tick> allTicks)
        {
            string yearMonth = symbol.GetYearMonth(date);
            var ticks = allTicks.Where(x => x.Order == yearMonth.ToInt()).ToList();
            var kLineTimes = tradeSession.GetKLineTimes(date);
            
            foreach (var time in kLineTimes)
            {
                var kline = ticks.GetKLine(time);
                if (kline != null) await _historiesService.AddUpdateAsync(kline);
            }
        }

        void ValidateRequest(HistoryKLinesImportForm model)
        {
            if (String.IsNullOrEmpty(model.Symbol)) ModelState.AddModelError("symbol", "必須填寫symbol");

            if (model.File == null) ModelState.AddModelError("file", "必須上傳檔案");

            string extension = Path.GetExtension(model.File.FileName).ToLower();
            if (extension != ".csv") ModelState.AddModelError("files", "檔案格式錯誤");
        }

    }
}
