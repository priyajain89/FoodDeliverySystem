using FoodDelivery.Domain.Models;

using FoodDelivery.Infrastructure.DTO;

using System.Security.Claims;

namespace FoodDelivery.Infrastructure.Repository

{

    public interface IAddressRepository

    {

        //Task<IEnumerable<AddressViewDto>> GetAllByCustomerIdAsync(int userId);

        Task<IEnumerable<AddressViewDto>> GetAllByUserIdAsync(int userId);

        Task<AddressViewDto?> GetByUserIdForCustomerAsync(int addressId, int userId);

        Task<AddressViewDto?> AddAddressAsync(AddressAddDto dto, ClaimsPrincipal user);

        Task<bool> UpdateAsync(int addressId, AddressAddDto dto, ClaimsPrincipal user);

        Task DeleteAsync(int id);

    }

}

