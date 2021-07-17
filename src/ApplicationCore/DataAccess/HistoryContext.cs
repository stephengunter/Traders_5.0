using ApplicationCore.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace ApplicationCore.DataAccess
{
	public class HistoryContext : DbContext
	{
		public HistoryContext(DbContextOptions<HistoryContext> options) : base(options)
		{
		}

		public HistoryContext(string connectionString) : base(new DbContextOptionsBuilder<HistoryContext>().UseSqlServer(connectionString).Options)
		{
			
		}

		public DbSet<KLine> KLines { get; set; }
		

	}
}
