using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IDeliveryagentRepository
    {
        Task<DeliveryAgent> SubmitAgentDetailsAsync(DeliveryAgent agent, IFormFile? DocumentUrl);
        Task<bool> UpdateDeliveryAgentAsync(DeliveryAgent agent, IFormFile? DocumentUrl);

        Task<bool> MarkAgentAvailableAsync(int agentId);

        Task<int?> GetAgentIdByUserIdAsync(int userId);
    }
}
