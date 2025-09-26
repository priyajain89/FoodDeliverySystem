using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
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

        public async Task<IEnumerable<User>> GetUnverifiedRestaurantsAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "Restaurant" && u.IsVerified == false)
                .Where(u => u.Restaurants.Any()) 
                .Include(u => u.Restaurants)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUnverifiedAgentsAsync()
        {
            return await _context.Users
                .Where(u => u.Role == "DeliveryAgent" && u.IsVerified == false)
                .Where(u => u.DeliveryAgents.Any()) 
                .Include(u => u.DeliveryAgents)
                .ToListAsync();
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
