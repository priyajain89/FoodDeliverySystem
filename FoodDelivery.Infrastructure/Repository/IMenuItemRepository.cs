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
        Task<MenuItem?> CreateAsync(MenuItemDto dto, ClaimsPrincipal user);
        Task<IEnumerable<MenuItem>> GetAllAsync();
        Task<MenuItem?> GetByIdAsync(int id);
        Task<MenuItem?> UpdateAsync(int id, MenuItemDto dto, ClaimsPrincipal user);
        Task<bool> DeleteAsync(int id, ClaimsPrincipal user);
        Task<IEnumerable<MenuItem>> SearchAsync(string pinCode, string? restaurantName, string? itemName, string? category, string? city);
    }

}
