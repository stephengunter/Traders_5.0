using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Text;
using ApplicationCore.Models;
using ApplicationCore.Paging;
using ApplicationCore.Views;
using ApplicationCore.ViewServices;
using Infrastructure.Views;

namespace Web.Models
{
    public class HistoryKLinesAdminModel
    {
        public ICollection<BaseOption<string>> SymbolOptions { get; set; } = new List<BaseOption<string>>();

        public PagedList<KLineGroupViewModel, KLineGroupViewModel> PagedList { get; set; }

        public void LoadSymbolsOptions(IEnumerable<Symbol> symbols, string emptyText = "全部")
        {
            var options = symbols.Select(x => x.ToOption()).ToList();

            if (!String.IsNullOrEmpty(emptyText)) options.Insert(0, new BaseOption<string>("", emptyText));

            SymbolOptions = options;
        }

    }
    public class HistoryKLinesImportForm
    {
        public string Symbol { get; set; }
        public string Date { get; set; }        
        public IFormFile File { get; set; }
    }
}
