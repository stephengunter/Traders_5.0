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
			
		}
	}

	public class SymbolCodeSpecification : BaseSpecification<Symbol>
	{
		public SymbolCodeSpecification(string code) : base(x => !x.Removed && x.Code.ToUpper() == code.ToUpper())
		{

		}
	}
}
