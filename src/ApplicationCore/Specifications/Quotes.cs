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
    public class HistoryQuoteFilterSpecification : BaseSpecification<Quote>
    {
		public HistoryQuoteFilterSpecification(string symbol, int date, int time) : base(item => item.Symbol == symbol.ToUpper() && item.Date == date && item.Time == time)
		{
			
		}
	}
}
