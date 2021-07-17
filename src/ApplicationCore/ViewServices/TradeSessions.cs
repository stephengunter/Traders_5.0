using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Views;
using ApplicationCore.Models;
using ApplicationCore.Paging;
using ApplicationCore.Helpers;
using System.Threading.Tasks;
using System.Linq;
using Infrastructure.Views;
using AutoMapper;

namespace ApplicationCore.ViewServices
{
	public static class TradeSessionsViewService
	{
		public static TradeSessionViewModel MapViewModel(this TradeSession entity, IMapper mapper)
			=> mapper.Map<TradeSessionViewModel>(entity);

		public static TradeSession MapEntity(this TradeSessionViewModel model, IMapper mapper)
			=> mapper.Map<TradeSessionViewModel, TradeSession>(model);

		public static IEnumerable<TradeSession> GetOrdered(this IEnumerable<TradeSession> tradeSessions) => tradeSessions.OrderBy(item => item.Default);

	}
}
