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
            Task<IEnumerable<MenuItemViewDto>> GetAllAsync();
            Task<bool> UpdateAsync(int id, MenuItemUpdateDto dto);
            Task<bool> DeleteAsync(int id);
            Task<IEnumerable<MenuItem>> SearchAsync(string pinCode, string? restaurantName, string? itemName, string? category, string? city);


    }

}
