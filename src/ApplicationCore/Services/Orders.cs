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
    public interface IOrdersService
    {
        Task<Order> GetByIdAsync(int id);
        Task UpdateAsync(Order order);
        Task RemoveAsync(Order order);
    }
    public class OrdersService : IOrdersService
    {
        private readonly IDefaultRepository<Order> _ordersRepository;
        public OrdersService(IDefaultRepository<Order> ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }
       
        public async Task<Order> GetByIdAsync(int id) => await _ordersRepository.GetByIdAsync(id);
        public async Task UpdateAsync(Order order) => await _ordersRepository.UpdateAsync(order);
        
        public async Task RemoveAsync(Order order)
        {
            order.Removed = true;
            await _ordersRepository.UpdateAsync(order);
        }
    }
}
