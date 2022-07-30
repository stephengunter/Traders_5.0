using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using ApplicationCore.Specifications;

namespace ApplicationCore.Services
{
    public interface IRealTimeService
    {
        Task AddUpdateAsync(KLine kLine);
    }

    public class RealTimeService : IRealTimeService
    {
        private readonly IRealTimeRepository<KLine> _realTimeRepository;

        public RealTimeService(IRealTimeRepository<KLine> realTimeRepository)
        {
            _realTimeRepository = realTimeRepository;
        }
       
        public async Task AddUpdateAsync(KLine kLine)
        {
            var existingEntity = FindOne(kLine.Date, kLine.Symbol, kLine.Time);
            if (existingEntity == null)
            {
                await _realTimeRepository.AddAsync(kLine);
            }
            else
            {
                kLine.Id = existingEntity.Id;
                await _realTimeRepository.UpdateAsync(existingEntity, kLine);
            }
        }
        KLine FindOne(int date, string symbol, int time)
        {
            var spec = new HistoryKLineFilterSpecification(date, new Symbol { Code = symbol.ToUpper() }, time);
            return _realTimeRepository.GetSingleBySpec(spec);
        }

    }
}
