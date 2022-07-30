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
    public static class OrdersViewService
    {
        public static OrderViewModel MapViewModel(this Order order, IMapper mapper)
        { 
            var model = mapper.Map<OrderViewModel>(order);
            model.Type = order.Type.ToString();
            model.BS = order.BS.ToString();

            return model;
        }

        public static List<OrderViewModel> MapViewModelList(this IEnumerable<Order> orders, IMapper mapper)
             => orders.Select(item => MapViewModel(item, mapper)).ToList();
        public static IEnumerable<Order> GetOrdered(this IEnumerable<Order> orders) => orders.OrderByDescending(item => item.CreatedAt);
       
    }
}
