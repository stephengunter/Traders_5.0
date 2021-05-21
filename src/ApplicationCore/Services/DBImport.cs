using ApplicationCore.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ApplicationCore.Helpers;

namespace ApplicationCore.Services
{
    public interface IDBImportService
    {
		void Test(string type);
		void ImportQuotes(HistoryContext _context, List<Quote> models);
		void SyncQuotes(HistoryContext _context, List<Quote> models);
	}

	public class DBImportService : IDBImportService
	{
		public void Test(string type)
		{ 
			
		}

		public void ImportQuotes(HistoryContext _context, List<Quote> models)
		{
			var connectionString = _context.Database.GetDbConnection().ConnectionString;

			var newQuotes = new List<Quote>();
			foreach (var quoteModel in models)
			{
				//var existingEntity = _context.Quotes.Find(QuoteModel.Id);
				//if (existingEntity == null) newQuotes.Add(QuoteModel);
				//else Update(_context, existingEntity, QuoteModel);
			}

			_context.SaveChanges();

			using (var context = new HistoryContext(connectionString))
			{
				//context.Quotes.AddRange(newQuotes);
				var dbSet = context.Set<Quote>();
				dbSet.AddRange(newQuotes);
				context.Database.OpenConnection();
				try
				{
					context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT Quotes ON");
					context.SaveChanges();
					context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT Quotes OFF");
				}
				finally
				{
					context.Database.CloseConnection();
				}
			}

		}

		
		void Update(HistoryContext _context, BaseEntity existingEntity, BaseEntity model)
		{
			var entry = _context.Entry(existingEntity);
			entry.CurrentValues.SetValues(model);
			entry.State = EntityState.Modified;
		}
		public void SyncQuotes(HistoryContext _context, List<Quote> models)
		{
			var ids = models.Select(x => x.Id).ToList();

			var deletedEntities = _context.Quotes.Where(x => !ids.Contains(x.Id)).ToList();

			if (deletedEntities.HasItems()) _context.Quotes.RemoveRange(deletedEntities);

			_context.SaveChanges();
		}

	}
}
