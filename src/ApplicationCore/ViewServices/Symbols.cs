using ApplicationCore.Models;
using System;
using System.Collections.Generic;
using AutoMapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Views;
using Infrastructure.Views;

namespace ApplicationCore.ViewServices
{
    public static class SymbolsViewService
    {
        public static SymbolViewModel MapViewModel(this Symbol symbol, IMapper mapper)
        { 
            var model = mapper.Map<SymbolViewModel>(symbol);
            model.Type = symbol.Type.ToString();
            model.TimeZone = symbol.TimeZone.ToString();

            return model;
        }

        public static List<SymbolViewModel> MapViewModelList(this IEnumerable<Symbol> symbols, IMapper mapper)
             => symbols.Select(item => MapViewModel(item, mapper)).ToList();
        public static IEnumerable<Symbol> GetOrdered(this IEnumerable<Symbol> symbols) => symbols.OrderBy(item => item.Order);

        public static Symbol MapEntity(this SymbolViewModel model, IMapper mapper, string currentUserId)
        {
            var entity = mapper.Map<SymbolViewModel, Symbol>(model);

            if (model.Id == 0) entity.SetCreated(currentUserId);
            else
            {
                foreach (var sessions in entity.TradeSessions)
                {
                    sessions.SymbolId = entity.Id;
                }
                entity.SetUpdated(currentUserId);
            }

            return entity;
        }

        public static BaseOption<string> ToOption(this Symbol symbol) => new BaseOption<string>(symbol.Code, symbol.Title);
    }
}
