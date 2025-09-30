using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IDeliveryagentRepository
    {
        Task<DeliveryAgent> SubmitAgentDetailsAsync(DeliveryAgent agent);
        Task<bool> UpdateDeliveryAgentAsync(DeliveryAgentResponseDto dto);

        Task<bool> MarkAgentAvailableAsync(int agentId);
    }
}
