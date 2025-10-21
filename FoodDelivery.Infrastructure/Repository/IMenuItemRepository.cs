using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IMenuItemRepository
    {
            Task<MenuItemViewDto?> CreateAsync(MenuItemCreateDto dto, int userId);
            Task<IEnumerable<MenuItem>> SearchByPinCodeAsync(string pinCode);
<<<<<<< HEAD
            Task<IEnumerable<MenuItem>> SearchByFiltersAsync(string? restaurantName, string? itemName, string? category, string? city);
            Task<MenuItemViewDto?> GetByIdAsync(int id);
        Task<List<MenuItem>> GetByRestaurantIdAsync(int restaurantId);
        Task<IEnumerable<MenuItemViewDto>> GetAllAsync();
=======
        Task<IEnumerable<MenuItem>> SearchByFiltersAsync(
                string pinCode,
                string? restaurantName,
                string? itemName,
                string? category,
                string? city);

        Task<MenuItemViewDto?> GetByIdAsync(int id);
            Task<IEnumerable<MenuItemViewDto>> GetAllAsync();
>>>>>>> 6a212d3c95d9956cb5ea63677267d50a7d221ef3
            Task<bool> UpdateAsync(int id, MenuItemUpdateDto dto);
            Task<bool> DeleteAsync(int id);
    }
}
