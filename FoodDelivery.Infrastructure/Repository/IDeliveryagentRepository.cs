using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IDeliveryagentRepository
    {
        Task<DeliveryAgent> SubmitAgentDetailsAsync(DeliveryAgent agent);
        Task<bool> UpdateDeliveryAgentAsync(DeliveryAgentResponseDto dto);
    }
}
