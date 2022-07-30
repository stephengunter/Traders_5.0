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
using Microsoft.AspNetCore.Http;
using ApplicationCore.Settings;
using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using ApplicationCore.DataAccess;
using ApplicationCore;

namespace Web.Controllers.Admin
{
	public class TicksController : BaseAdminController
	{
        private readonly AdminSettings _adminSettings;
        private readonly ISymbolsService _symbolsService;
        private readonly ITicksService _ticksService;

		public TicksController(IOptions<AdminSettings> adminSettings, ISymbolsService symbolsService, ITicksService ticksService)
		{
            _adminSettings = adminSettings.Value;
            _symbolsService = symbolsService;
            _ticksService = ticksService;
		}

        [HttpPost("")]
        public async Task<IActionResult> Store([FromBody] AdminFileRequest model)
        {
            await ValidateAsync(model);
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var txSymbol = _symbolsService.GetByCode(SymbolCodes.TX);
            var tradeSession = txSymbol.TradeSessions.First(x => x.Default);

            var rows = new List<List<string>>();
            var file = model.Files.FirstOrDefault();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    var values = reader.ReadLine().SplitToList();
                    string symbol = values[1].Trim();
                    if (symbol.EqualTo(txSymbol.Code)) rows.Add(values);
                }
            }

            int month = rows.Select(x => x[2].Trim().ToInt()).Distinct()
                                                        .Where(x => x > 0).Min();

            //只要最近月
            rows = rows.Where(x => x[2].Trim().ToInt() == month).ToList();

            int order = 0;
            var ticks = new List<Tick>();
            foreach (var row in rows)
            {
                int time = row[3].Trim().ToInt();
                if (time >= tradeSession.Open && time <= tradeSession.Close)
                {
                    ticks.Add(new Tick
                    {
                        Symbol = txSymbol.Code,
                        Date = row[0].Trim().ToInt(),
                        Time = row[3].Trim().ToInt(),
                        Order = order,
                        Price = row[4].Trim().ToInt(),
                        Qty = row[5].Trim().ToInt()
                    });

                    order += 1;
                }
            }

            var pagedList = new PagedList<Tick>(ticks, 1, 100);
            for (int i = 1; i <= pagedList.TotalPages; i++)
            {
                pagedList.GoToPage(i);
                _ticksService.AddMany(pagedList.List);
            }

            return Ok();
        }

        

        async Task ValidateAsync(AdminFileRequest model)
        {
            ValidateRequest(model, _adminSettings);

            if (model.Files.Count < 1) ModelState.AddModelError("files", "必須上傳檔案");
            else if (model.Files.Count > 1) ModelState.AddModelError("files", "只能上傳一個檔案");

            var file = model.Files.FirstOrDefault();
            string extension = Path.GetExtension(file.FileName).ToLower();
            if (extension != ".csv") ModelState.AddModelError("files", "檔案格式錯誤");

            int count = await _ticksService.CountAsync();
            if(count > 0) ModelState.AddModelError("count", "資料表尚未清空");
        }

    }
}
