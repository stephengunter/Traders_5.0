using System;
using System.Collections.Generic;
using System.Text;
using Infrastructure.Entities;

namespace ApplicationCore.Models
{
	public enum DayType
	{
		Holiday,
		Extra,
        Empty
	}

	public class Day : BaseEntity
	{
		public int Year { get; set; }

		public int Date { get; set; }

		public DayType Type { get; set; }

		public string Text { get; set; }
	}

}
