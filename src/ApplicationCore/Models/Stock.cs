using ApplicationCore.Helpers;
using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;


namespace ApplicationCore.Models
{
	public class Stock : BaseEntity
	{
		public string Name { get; set; }

		public string Code { get; set; }

		public double Price { get; set; }

		public double Weight { get; set; }

		public bool Base { get; set; }

		public bool Ignore { get; set; }

		
	}
}
