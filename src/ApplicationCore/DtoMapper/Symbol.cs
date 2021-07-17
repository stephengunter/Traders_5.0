using ApplicationCore.Models;
using ApplicationCore.Views;
using AutoMapper;

namespace ApplicationCore.DtoMapper
{
	public class SymbolMappingProfile : Profile
	{
		public SymbolMappingProfile()
		{
			CreateMap<Symbol, SymbolViewModel>();

			CreateMap<SymbolViewModel, Symbol>();
		}
	}
}
