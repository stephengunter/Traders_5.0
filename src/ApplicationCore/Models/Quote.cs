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
		public int Date { get; set; }

		public int Time { get; set; }

		public int Price { get; set; }

		public int Open { get; set; }

		public int High { get; set; }   

		public int Low { get; set; }


		public ICollection<Data> DataList { get; set; } = new List<Data>();

	}
}
