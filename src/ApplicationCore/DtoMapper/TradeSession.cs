using ApplicationCore.Models;
using ApplicationCore.Views;
using AutoMapper;

namespace ApplicationCore.DtoMapper
{
	public class TradeSessionMappingProfile : Profile
	{
		public TradeSessionMappingProfile()
		{
			CreateMap<TradeSession, TradeSessionViewModel>();

			CreateMap<TradeSessionViewModel, TradeSession>();
		}
	}
}
