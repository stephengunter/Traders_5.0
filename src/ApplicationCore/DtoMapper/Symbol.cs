using ApplicationCore.Models;
using ApplicationCore.Views;
using AutoMapper;
using ApplicationCore.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

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
