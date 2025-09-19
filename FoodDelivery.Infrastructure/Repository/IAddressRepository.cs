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
    public class IAddressRepository
    {

        Task<IEnumerable<AddressViewDto>> GetAllByUserIdAsync(int userId);
        Task<AddressViewDto?> GetByIdForUserAsync(int addressId, int userId);
        Task<AddressViewDto?> AddAddressAsync(AddressAddDto dto, ClaimsPrincipal user);
        Task UpdateAsync(Address address);
        Task DeleteAsync(int id);

    }
}
