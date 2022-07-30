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
	public class TickListFilterSpecification : BaseSpecification<Tick>
	{
		public TickListFilterSpecification(string symbol) : base(item => item.Symbol == symbol)
		{

		}

		public TickListFilterSpecification(string symbol, int begin, int end)
			: base(item => item.Symbol == symbol && item.Time >= begin && item.Time < end)
		{

		}
	}

	public class TickFilterSpecification : BaseSpecification<Tick>
    {
		public TickFilterSpecification(string symbol, int time, int order) 
			: base(item => item.Symbol == symbol && item.Time == time && item.Order == order)
		{
			
		}
	}

	
}
