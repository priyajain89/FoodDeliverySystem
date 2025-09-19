using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodDelivery.Infrastructure.Repository
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDbContext _context;
        private static readonly List<string> AllowedCategories = new()
        {
            "Beverages", "Main Course", "Desserts", "Snacks", "Starters"
        };

        public MenuItemRepository(AppDbContext context)
        {
            _context = context;
        }

        private async Task<Restaurant?> GetVerifiedRestaurantAsync(ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (userIdClaim == null) return null;

            int userId = int.Parse(userIdClaim);
            var restaurantUser = await _context.Users
                .Include(u => u.Restaurants)
                .FirstOrDefaultAsync(u => u.UserId == userId && u.Role == "restaurant" && u.IsVerified == true);

            return restaurantUser?.Restaurants.FirstOrDefault();
        }

        public async Task<MenuItem?> CreateAsync(MenuItemDto dto, ClaimsPrincipal user)
        {
            var restaurantIdClaim = user.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            if (restaurantIdClaim == null) return null;

            int restaurantId = int.Parse(restaurantIdClaim);
            var restaurant = await _context.Restaurants.FindAsync(restaurantId);
            if (restaurant == null || !restaurant.User.IsVerified.GetValueOrDefault()) return null;


            var item = new MenuItem
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                Category = dto.Category,
                RestaurantId = restaurant.RestaurantId,
                FoodImage = dto.FoodImage
            };

            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();
            item.Restaurant = restaurant;
            return item;
        }

        public async Task<IEnumerable<MenuItem>> GetAllAsync()
        {
            return await _context.MenuItems
                .Include(m => m.Restaurant)
                .ThenInclude(r => r.User)
                .ToListAsync();
        }

        public async Task<MenuItem?> GetByIdAsync(int id)
        {
            return await _context.MenuItems
                .Include(m => m.Restaurant)
                .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(m => m.ItemId == id);
        }

        public async Task<MenuItem?> UpdateAsync(int id, MenuItemDto dto, ClaimsPrincipal user)
        {
            var item = await _context.MenuItems
                .Include(m => m.Restaurant)
                .FirstOrDefaultAsync(m => m.ItemId == id);

            var restaurant = await GetVerifiedRestaurantAsync(user);
            if (item == null || restaurant == null || item.RestaurantId != restaurant.RestaurantId || !AllowedCategories.Contains(dto.Category))
                return null;

            item.Name = dto.Name;
            item.Description = dto.Description;
            item.Price = dto.Price;
            item.IsAvailable = dto.IsAvailable;
            item.Category = dto.Category;
            item.FoodImage = dto.FoodImage;

            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<bool> DeleteAsync(int id, ClaimsPrincipal user)
        {
            var item = await _context.MenuItems.FindAsync(id);
            var restaurant = await GetVerifiedRestaurantAsync(user);
            if (item == null || restaurant == null || item.RestaurantId != restaurant.RestaurantId)
                return false;

            _context.MenuItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<MenuItem>> SearchAsync(string pinCode, string? restaurantName, string? itemName, string? category, string? city)
        {
            var query = _context.MenuItems
                .Include(m => m.Restaurant)
                .ThenInclude(r => r.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(pinCode))
                query = query.Where(m => m.Restaurant.PinCode.ToString() == pinCode);

            if (!string.IsNullOrEmpty(restaurantName))
                query = query.Where(m => m.Restaurant.User.Name.Contains(restaurantName));

            if (!string.IsNullOrEmpty(itemName))
                query = query.Where(m => m.Name.Contains(itemName));

            if (!string.IsNullOrEmpty(category))
                query = query.Where(m => m.Category == category);

            if (!string.IsNullOrEmpty(city))
                query = query.Where(m => m.Restaurant.Address != null && m.Restaurant.Address.Contains(city));

            return await query.ToListAsync();
        }
    }
}
