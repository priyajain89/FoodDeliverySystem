using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IAdminRepository
    {
        Task<IEnumerable<RestaurantDto>> GetUnverifiedRestaurantsAsync();
        Task<IEnumerable<DeliveryAgentGetDto>> GetUnverifiedAgentsAsync();
        Task<bool> VerifyUserAsync(int userId, string role);
    }
}
