using ApplicationCore.DataAccess;
using ApplicationCore.Models;
using ApplicationCore.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Services
{
    public interface ITicksService
    {
        Task<Tick> CreateAsync(Tick tick);
        void AddMany(IEnumerable<Tick> ticks);
        Task AddUpdateAsync(Tick tick);

        Task<IEnumerable<Tick>> FetchAllAsync(string symbol);
        Task<IEnumerable<Tick>> FetchAsync(string symbol, int begin, int end);
        Task<int> CountAsync();
    }
    public class TicksService : ITicksService
    {
        private readonly IRealTimeRepository<Tick> _ticksRepository;
        public TicksService(IRealTimeRepository<Tick> ticksRepository)
        {
            _ticksRepository = ticksRepository;
        }


        public async Task<Tick> CreateAsync(Tick tick) => await _ticksRepository.AddAsync(tick);

        public async Task AddUpdateAsync(Tick tick)
        {
            var existingEntity = FindOne(tick.Symbol, tick.Time, tick.Order);
            if (existingEntity == null) await _ticksRepository.AddAsync(tick);
            else
            {
                tick.Id = existingEntity.Id;
                await _ticksRepository.UpdateAsync(existingEntity, tick);
            }
        }

        public void AddMany(IEnumerable<Tick> ticks) => _ticksRepository.AddRange(ticks);

        Tick FindOne(string symbol, int time, int order)
        {
            var spec = new TickFilterSpecification(symbol, time, order);
            return _ticksRepository.GetSingleBySpec(spec);
        }

        public async Task<IEnumerable<Tick>> FetchAllAsync(string symbol)
            => await _ticksRepository.ListAsync(new TickListFilterSpecification(symbol));

        public async Task<IEnumerable<Tick>> FetchAsync(string symbol, int begin, int end)
        {
            var spec = new TickListFilterSpecification(symbol, begin, end);
            return await _ticksRepository.ListAsync(spec);
        }

        public async Task<int> CountAsync() => await _ticksRepository.CountAsync();
    }
}
