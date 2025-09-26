using FoodDelivery.Domain.Data;

using FoodDelivery.Domain.Models;

using FoodDelivery.Infrastructure.Common;

using FoodDelivery.Infrastructure.DTO;

using Microsoft.EntityFrameworkCore;

using System.Security.Claims;

namespace FoodDelivery.Infrastructure.Repository

{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDbContext _context;
        public MenuItemRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<MenuItemViewDto?> CreateAsync(MenuItemCreateDto dto, int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null || user.Role != "Restaurant" || user.IsVerified != true)
                return null;
            var restaurant = await _context.Restaurants
                .FirstOrDefaultAsync(r => r.UserId == userId);

            if (restaurant == null)
                return null;

            if (!StaticCategories.Categories.Contains(dto.Category))
                return null;
            var item = new MenuItem
            {
                RestaurantId = restaurant.RestaurantId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                Category = dto.Category,
                FoodImage = dto.FoodImage
            };

            _context.MenuItems.Add(item);
            await _context.SaveChangesAsync();
            return new MenuItemViewDto
            {
                ItemId = item.ItemId,
                Name = item.Name ?? string.Empty,
                Description = item.Description,
                Price = item.Price ?? 0,
                IsAvailable = item.IsAvailable ?? false,
                Category = item.Category,
                FoodImage = item.FoodImage,
                RestaurantId = restaurant.RestaurantId,
                RestaurantName = user.Name ?? "Unknown"
            };

        }
        public async Task<MenuItemViewDto?> GetByIdAsync(int id)

        {
            var item = await _context.MenuItems.Include(m => m.Restaurant).FirstOrDefaultAsync(m => m.ItemId == id);
            return item == null ? null : ToViewDto(item, item.Restaurant?.User.Name ?? "Unknown");
        }

        public async Task<IEnumerable<MenuItemViewDto>> GetAllAsync()
        {
            var items = await _context.MenuItems.Include(m => m.Restaurant).ToListAsync();
            return items.Select(i => ToViewDto(i, i.Restaurant?.User.Name ?? "Unknown")).ToList();
        }
        public async Task<bool> UpdateAsync(int id, MenuItemUpdateDto dto)
        {
            var item = await _context.MenuItems.FindAsync(id);

            if (item == null || !StaticCategories.Categories.Contains(dto.Category))

                return false;

            item.Name = dto.Name;

            item.Description = dto.Description;

            item.Price = dto.Price;

            item.IsAvailable = dto.IsAvailable;

            item.Category = dto.Category;

            item.FoodImage = dto.FoodImage;

            await _context.SaveChangesAsync();

            return true;

        }

        public async Task<bool> DeleteAsync(int id)

        {

            var item = await _context.MenuItems.FindAsync(id);

            if (item == null) return false;

            _context.MenuItems.Remove(item);

            await _context.SaveChangesAsync();

            return true;

        }

        private MenuItemViewDto ToViewDto(MenuItem item, string restaurantName)

        {

            return new MenuItemViewDto

            {

                ItemId = item.ItemId,

                Name = item.Name ?? string.Empty,

                Description = item.Description,

                Price = item.Price ?? 0,

                IsAvailable = item.IsAvailable ?? false,

                Category = item.Category,

                FoodImage = item.FoodImage,

                RestaurantName = restaurantName

            };

        }


        public async Task<IEnumerable<MenuItem>> SearchAsync(string pinCode, string? restaurantName, string? itemName, string? category, string? city)
        {
            var query = _context.MenuItems
                .Include(m => m.Restaurant)
                .ThenInclude(r => r.User)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(pinCode))
                query = query.Where(m => m.Restaurant.PinCode.ToString() == pinCode);

            if (!string.IsNullOrWhiteSpace(restaurantName))
                query = query.Where(m => m.Restaurant.User.Name.Contains(restaurantName));

            if (!string.IsNullOrWhiteSpace(itemName))
                query = query.Where(m => m.Name != null && m.Name.Contains(itemName));

            if (!string.IsNullOrWhiteSpace(category))
                query = query.Where(m => m.Category == category);

            if (!string.IsNullOrWhiteSpace(city))
                query = query.Where(m => m.Restaurant.Address != null && m.Restaurant.Address.Contains(city));

            return await query.ToListAsync();
        }
    }
}