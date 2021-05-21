using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
	public class Key : BaseEntity
	{
		public string UserId { get; set; }

		public string IP { get; set; }

		public int Date { get; set; }

	}
}
