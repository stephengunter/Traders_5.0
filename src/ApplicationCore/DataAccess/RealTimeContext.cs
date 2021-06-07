using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApplicationCore.DataAccess
{
	public class RealTimeContext : DbContext
	{
		public RealTimeContext(DbContextOptions<RealTimeContext> options) : base(options)
		{
		}

		public RealTimeContext(string connectionString) : base(new DbContextOptionsBuilder<RealTimeContext>().UseSqlServer(connectionString).Options)
		{
			
		}

		public DbSet<Quote> Quotes { get; set; }
		

	}
}
