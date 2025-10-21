using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.Repository
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _context;
        public AdminRepository(AppDbContext context)
        {
            _context = context;
        }
        //public async Task<IEnumerable<User>> GetUnverifiedRestaurantsAsync()
        //{
        //    return await _context.Users
        //        .Where(u => u.Role == "Restaurant" && u.IsVerified == false)
        //        .Where(u => u.Restaurants.Any()) 
        //        .Include(u => u.Restaurants)
        //        .ToListAsync();
        //}
        public async Task<IEnumerable<RestaurantDto>> GetUnverifiedRestaurantsAsync()
        {
            var users = await _context.Users
                .Where(u => u.Role == "Restaurant" && u.IsVerified == false)
                .Where(u => u.Restaurants.Any())
                .Include(u => u.Restaurants)
                .ToListAsync();

            return users.Select(u => new RestaurantDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone,
                Role = u.Role,
                
                SubmittedRestaurants = u.Restaurants.Select(r => new RestaurantGetResponseDto
                {
                    
                    Address = r.Address,
                    FssaiId = r.FssaiId,
                    PinCode = r.PinCode,
                    FssaiImage = r.FssaiImage,
                    TradelicenseImage = r.TradelicenseImage,
                    TradeId = r.TradeId
                }).ToList()
            });
        }

        //public async Task<IEnumerable<User>> GetUnverifiedAgentsAsync()
        //{
        //    return await _context.Users
        //        .Where(u => u.Role == "DeliveryAgent" && u.IsVerified == false)
        //        .Where(u => u.DeliveryAgents.Any()) 
        //        .Include(u => u.DeliveryAgents)
        //        .ToListAsync();
        //}

        public async Task<IEnumerable<DeliveryAgentGetDto>> GetUnverifiedAgentsAsync()
        {
            var users = await _context.Users
               .Where(u => u.Role == "DeliveryAgent" && u.IsVerified == false)
                .Include(u => u.DeliveryAgents)
                .ToListAsync();

            return users.SelectMany(u => u.DeliveryAgents.Select(agent => new DeliveryAgentGetDto
            {
                AgentId = agent.AgentId,
                Address = agent.Address,
                DocumentUrl = agent.DocumentUrl,
                UserId = u.UserId,
                // From parent User entity
                Name = u.Name,
                Email = u.Email,
                Phone = u.Phone
            })).ToList();
        }
        public async Task<bool> VerifyUserAsync(int userId, string role)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null || user.Role?.ToLower() != role.ToLower()) return false;

            user.IsVerified = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
