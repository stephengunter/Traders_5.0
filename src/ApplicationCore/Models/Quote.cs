using ApplicationCore.Helpers;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ApplicationCore.Models
{
	public class Quote : BaseEntity
	{
		public string Symbol { get; set; }
		public int Date { get; set; }
		public int Time { get; set; }
		public decimal Price { get; set; }
		public decimal Open { get; set; }
		public decimal High { get; set; }   
		public decimal Low { get; set; }

		public decimal Vol { get; set; }

	}
}
