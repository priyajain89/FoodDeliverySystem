using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.Common;
using FoodDelivery.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using FoodDelivery.Infrastructure.Services;


namespace FoodDelivery.Infrastructure.Repository
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly AppDbContext _context;
        private readonly IFileService _fileService;
        public MenuItemRepository(AppDbContext context, IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<MenuItemViewDto?> CreateAsync(MenuItemCreateDto dto, int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
            if (user == null || user.Role != "Restaurant" || user.IsVerified != true)
                return null;

            var restaurants = await _context.Restaurants.FirstOrDefaultAsync(r => r.UserId == userId);

            if (restaurants == null)
                return null;

            if (!StaticCategories.Categories.Contains(dto.Category))
                return null;


            string? imageUrl = null;
            if (dto.FoodImage != null)
            {
                imageUrl = await _fileService.SaveFileAsync(dto.FoodImage, "menu-images"); // ? Save image
            }

            var item = new MenuItem
            {
                RestaurantId = restaurants.RestaurantId,
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                IsAvailable = dto.IsAvailable,
                Category = dto.Category,
                FoodImage = imageUrl
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
                RestaurantId = restaurants.RestaurantId,
                RestaurantName = user.Name ?? "Unknown"
            };
        }

        public async Task<MenuItemViewDto?> GetByIdAsync(int id)
        {
            var item = await _context.MenuItems
                                     .Include(m => m.Restaurant)
                                     .ThenInclude(r => r.User)
                                     .FirstOrDefaultAsync(m => m.ItemId == id);

            return item == null ? null : ToViewDto(item, item.Restaurant?.User?.Name ?? "Unknown");
        }
        public async Task<IEnumerable<MenuItemViewDto>> GetAllAsync()
        {
            var items = await _context.MenuItems
                                      .Include(m => m.Restaurant)
                                      .ThenInclude(r => r.User)
                                      .ToListAsync();
<<<<<<< HEAD
=======

>>>>>>> 6a212d3c95d9956cb5ea63677267d50a7d221ef3
            return items.Select(i => new MenuItemViewDto
            {
                ItemId = i.ItemId,
                Name = i.Name,
                Description = i.Description,
                Price = i.Price,
                IsAvailable = i.IsAvailable,
                Category = i.Category,
                FoodImage = i.FoodImage,
                RestaurantId = i.RestaurantId ?? 0,
                RestaurantName = i.Restaurant?.User?.Name ?? "Unknown"
            }).ToList();

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

        public async Task<List<MenuItem>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.MenuItems
                .Where(item => item.RestaurantId == restaurantId)
                .Include(item => item.Restaurant)
                .ThenInclude(r => r.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<MenuItem>> SearchByPinCodeAsync(string pinCode)
        {
            if (string.IsNullOrWhiteSpace(pinCode))
                return new List<MenuItem>();

            var query = _context.MenuItems
                .Include(m => m.Restaurant)
                .ThenInclude(r => r.User)
                .Where(m => m.Restaurant.PinCode.ToString() == pinCode);

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<MenuItem>> SearchByFiltersAsync(
     string pinCode,
     string? restaurantName,
     string? itemName,
     string? category,
     string? city)
        {
            if (!int.TryParse(pinCode, out int parsedPin))
                return Enumerable.Empty<MenuItem>();

            var query = _context.MenuItems
                .Include(m => m.Restaurant)
                    .ThenInclude(r => r.User)
                .Where(m => m.Restaurant.PinCode == parsedPin)
                .AsQueryable();

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