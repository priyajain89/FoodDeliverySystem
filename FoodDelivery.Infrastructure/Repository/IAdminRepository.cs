using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IAdminRepository
    {
        Task<IEnumerable<User>> GetUnverifiedRestaurantsAsync();
        Task<IEnumerable<User>> GetUnverifiedAgentsAsync();
        Task<bool> VerifyUserAsync(int userId, string role);
    }
}
