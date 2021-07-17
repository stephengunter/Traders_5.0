using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using ApplicationCore.Specifications;
using ApplicationCore.Views;

namespace ApplicationCore.Services
{
    public interface IHistoriesService
    {
        Task<IEnumerable<KLine>> FetchAsync(Symbol symbol, int date);
        Task AddUpdateAsync(KLine kLine);
        Task<IEnumerable<KLineGroupViewModel>> FetchGroupAsync(Symbol symbol);
        Task<IEnumerable<KLineGroupViewModel>> FetchGroupAsync(Symbol symbol, int date);
        Task<IEnumerable<KLineGroupViewModel>> FetchGroupByDateAsync(int date);
    }

    public class HistoriesService : IHistoriesService
    {
        private readonly IHistoryRepository<KLine> _historyRepository;
        public HistoriesService(IHistoryRepository<KLine> historyRepository)
        {
            _historyRepository = historyRepository;
        }

        public async Task<IEnumerable<KLine>> FetchAsync(Symbol symbol, int date)
        {
            var spec = new HistoryKLineFilterSpecification(symbol, date);
            return await _historyRepository.ListAsync(spec);
        }
        public async Task AddUpdateAsync(KLine kLine)
        {
            var existingEntity = FindOne(kLine.Symbol, kLine.Date, kLine.Time);
            if (existingEntity == null)
            {
                await _historyRepository.AddAsync(kLine);
            }
            else
            {
                kLine.Id = existingEntity.Id;
                await _historyRepository.UpdateAsync(existingEntity, kLine);
            }
        }

        public async Task<IEnumerable<KLineGroupViewModel>> FetchGroupAsync(Symbol symbol)
        {
            var spec = new HistoryKLineFilterSpecification(symbol);
            var kLines = await _historyRepository.ListAsync(spec);

            return GetGroupModelList(kLines);
        }

        public async Task<IEnumerable<KLineGroupViewModel>> FetchGroupAsync(Symbol symbol, int date)
        {
            var kLines = await FetchAsync(symbol, date);

            return GetGroupModelList(kLines);
        }

        public async Task<IEnumerable<KLineGroupViewModel>> FetchGroupByDateAsync(int date)
        {
            var spec = new HistoryKLineFilterSpecification(date);
            var kLines = await _historyRepository.ListAsync(spec);

            return GetGroupModelList(kLines);
        }

        IEnumerable<KLineGroupViewModel> GetGroupModelList(IEnumerable<KLine> kLines)
        {
            return kLines.GroupBy(q => new { q.Date, q.Symbol })
                            .Select(g => new KLineGroupViewModel
                            {
                                Symbol = g.Key.Symbol,
                                Date = g.Key.Date,
                                Count = g.Count()
                            });
        }

        KLine FindOne(string symbol, int date, int time)
        {
            var spec = new HistoryKLineFilterSpecification(date, new Symbol { Code = symbol .ToUpper() }, time);
            return _historyRepository.GetSingleBySpec(spec);
        }
        
    }
}
