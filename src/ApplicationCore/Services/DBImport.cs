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
		void ImportKLines(HistoryContext _context, List<Models.KLine> models);
		void SyncKLines(HistoryContext _context, List<Models.KLine> models);
	}

	public class DBImportService : IDBImportService
	{
		public void Test(string type)
		{ 
			
		}

		public void ImportKLines(HistoryContext _context, List<Models.KLine> models)
		{
			var connectionString = _context.Database.GetDbConnection().ConnectionString;

			var newKLines = new List<Models.KLine>();
			foreach (var kLineModel in models)
			{
				//var existingEntity = _context.KLines.Find(KLineModel.Id);
				//if (existingEntity == null) newKLines.Add(KLineModel);
				//else Update(_context, existingEntity, KLineModel);
			}

			_context.SaveChanges();

			using (var context = new HistoryContext(connectionString))
			{
				//context.KLines.AddRange(newKLines);
				var dbSet = context.Set<Models.KLine>();
				dbSet.AddRange(newKLines);
				context.Database.OpenConnection();
				try
				{
					context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT KLines ON");
					context.SaveChanges();
					context.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT KLines OFF");
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
		public void SyncKLines(HistoryContext _context, List<Models.KLine> models)
		{
			var ids = models.Select(x => x.Id).ToList();

			var deletedEntities = _context.KLines.Where(x => !ids.Contains(x.Id)).ToList();

			if (deletedEntities.HasItems()) _context.KLines.RemoveRange(deletedEntities);

			_context.SaveChanges();
		}

	}
}
