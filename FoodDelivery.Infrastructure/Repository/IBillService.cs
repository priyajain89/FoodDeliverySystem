using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoodDelivery.Infrastructure.DTO;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IBillService
    {
        Task<BillDto> GenerateBillFromOrderAsync(int orderId);
    }
}
