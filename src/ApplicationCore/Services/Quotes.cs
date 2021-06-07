using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using ApplicationCore.Specifications;

namespace ApplicationCore.Services
{
    public interface IQuotesService
    {
        Task AddUpdateAsync(Quote quote);
    }

    public class QuotesService : IQuotesService
    {
        private readonly IRealTimeRepository<Quote> _realTimeRepository;
        private readonly IHistoryRepository<Quote> _historyRepository;
        public QuotesService(IRealTimeRepository<Quote> realTimeRepository, IHistoryRepository<Quote> historyRepository)
        {
            _realTimeRepository = realTimeRepository;
            _historyRepository = historyRepository;
        }

        public async Task AddUpdateAsync(Quote quote)
        {
            var existingEntity = FindOne(quote.Symbol, quote.Date, quote.Time);
            if (existingEntity == null)
            {
                await _historyRepository.AddAsync(quote);
            }
            else
            {
                quote.Id = existingEntity.Id;
                await _historyRepository.UpdateAsync(existingEntity, quote);
            }
        }

        Quote FindOne(string symbol, int date, int time)
        {
            var spec = new HistoryQuoteFilterSpecification(symbol, date, time);
            return _historyRepository.GetSingleBySpec(spec);
        }
        
    }
}
