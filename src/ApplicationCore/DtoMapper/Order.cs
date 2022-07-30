using ApplicationCore.Models;
using ApplicationCore.Views;
using AutoMapper;

namespace ApplicationCore.DtoMapper
{
	public class OrderMappingProfile : Profile
	{
		public OrderMappingProfile()
		{
			CreateMap<Order, OrderViewModel>();

			CreateMap<OrderViewModel, Order>();
		}
	}
}
