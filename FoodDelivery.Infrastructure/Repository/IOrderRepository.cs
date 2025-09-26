using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IOrderRepository
    {

        Task<int> CreateOrderFromCartAsync(int customerId, CreateOrderFromCartDto dto);
        Task<bool> AssignAddressToOrderAsync(AssignAddressToOrderDto dto);
        Task<Order?> GetOrderByIdAsync(int orderId);
        Task UpdateOrderAsync(Order order);

    }
}
