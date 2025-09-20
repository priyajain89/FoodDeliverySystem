using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IBillService
    {
        Task<BillDto> GenerateBillFromOrderAsync(int orderId);
        Task<DeliveryAgent?> AssignNearestAgentAsync(Order order);
    }
}
