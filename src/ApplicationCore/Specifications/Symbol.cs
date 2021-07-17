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
    public class SymbolFilterSpecification : BaseSpecification<Symbol>
    {
		public SymbolFilterSpecification() : base(x => !x.Removed)
		{
			AddInclude(item => item.TradeSessions);
		}
		public SymbolFilterSpecification(int id) : base(x => !x.Removed && x.Id == id)
		{
			AddInclude(item => item.TradeSessions);
		}
		public SymbolFilterSpecification(string code) : base(x => !x.Removed && x.Code.ToUpper() == code.ToUpper())
		{
			AddInclude(item => item.TradeSessions);
		}

	}
}
