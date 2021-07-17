using Infrastructure.Views;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationCore.Views
{
	public class SymbolViewModel : BaseRecordViewModel
	{
		public string Type { get; set; }
		public string Code { get; set; }
		public string Title { get; set; }
		public string TimeZone { get; set; }

		public ICollection<TradeSessionViewModel> TradeSessions { get; set; } = new List<TradeSessionViewModel>();

	}
}
