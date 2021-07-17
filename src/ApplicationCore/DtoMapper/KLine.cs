using ApplicationCore.Models;
using ApplicationCore.Views;
using AutoMapper;

namespace ApplicationCore.DtoMapper
{
	public class KLineMappingProfile : Profile
	{
		public KLineMappingProfile()
		{
			CreateMap<KLine, KLineViewModel>();

			CreateMap<KLineViewModel, KLine>();
		}
	}
}
