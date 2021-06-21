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
    public interface ISymbolsService
    {
        Task<IEnumerable<Symbol>> FetchAsync();
        Task<Symbol> GetByIdAsync(int id);
        Symbol GetByCode(string code);
        Task<Symbol> CreateAsync(Symbol symbol);
        Task UpdateAsync(Symbol symbol);
        Task UpdateAsync(Symbol existingEntity, Symbol model);
        Task RemoveAsync(Symbol symbol);
    }
    public class SymbolsService : ISymbolsService
    {
        private readonly IDefaultRepository<Symbol> _symbolsRepository;
        public SymbolsService(IDefaultRepository<Symbol> symbolsRepository)
        {
            _symbolsRepository = symbolsRepository;
        }

        public async Task<IEnumerable<Symbol>> FetchAsync()
            => await _symbolsRepository.ListAsync(new SymbolFilterSpecification());
        public async Task<Symbol> GetByIdAsync(int id) => await _symbolsRepository.GetByIdAsync(id);
        public Symbol GetByCode(string code) => _symbolsRepository.GetSingleBySpec(new SymbolCodeSpecification(code));
        public async Task<Symbol> CreateAsync(Symbol symbol) => await _symbolsRepository.AddAsync(symbol);
        public async Task UpdateAsync(Symbol symbol) => await _symbolsRepository.UpdateAsync(symbol);
        public async Task UpdateAsync(Symbol existingEntity, Symbol model) => await _symbolsRepository.UpdateAsync(existingEntity, model);
        public async Task RemoveAsync(Symbol symbol)
        {
            symbol.Removed = true;
            await _symbolsRepository.UpdateAsync(symbol);
        }
    }
}
