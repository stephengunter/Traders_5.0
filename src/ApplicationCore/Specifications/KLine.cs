using ApplicationCore.Models;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Helpers;

namespace ApplicationCore.Specifications
{
    public class HistoryKLineFilterSpecification : BaseSpecification<Models.KLine>
    {
		public HistoryKLineFilterSpecification(Symbol symbol) : base(item => item.Symbol == symbol.Code)
		{

		}
		public HistoryKLineFilterSpecification(int date) : base(item => item.Date == date)
		{

		}
		public HistoryKLineFilterSpecification(Symbol symbol, int date) : base(item => item.Symbol == symbol.Code && item.Date == date)
		{

		}
		public HistoryKLineFilterSpecification(int date, Symbol symbol, int time) : base(item => item.Date == date && item.Symbol == symbol.Code && item.Time == time)
		{
			
		}
	}
}
