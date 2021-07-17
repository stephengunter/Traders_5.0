using Infrastructure.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Views
{
	public class KLineViewModel : BaseEntityViewModel
	{
		public string Symbol { get; set; }
		public string Date { get; set; }
		public string Time { get; set; }
		public decimal Price { get; set; }
		public decimal Open { get; set; }
		public decimal High { get; set; }
		public decimal Low { get; set; }
		public decimal Vol { get; set; }

	}
}
