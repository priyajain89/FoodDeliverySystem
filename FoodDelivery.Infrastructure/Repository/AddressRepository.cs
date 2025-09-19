using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FoodDelivery.Infrastructure.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;

        public AddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AddressViewDto>> GetAllByUserIdAsync(int userId)
        {
            var addresses = await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();

            return addresses.Select(a => new AddressViewDto
            {
                AddressId = a.AddressId,
                UserId = a.UserId,
                AddressLine1 = a.AddressLine1,
                AddressLine2 = a.AddressLine2,
                City = a.City,
                State = a.State,
                PinCode = a.PinCode,
                Landmark = a.Landmark,
                IsDefault = a.IsDefault,
                Latitude = a.Latitude,
                Longitude = a.Longitude,
                MainLabel = a.MainLabel
            });
        }

        public async Task<AddressViewDto?> GetByIdForUserAsync(int addressId, int userId)
        {
            var address = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

            if (address == null) return null;

            return new AddressViewDto
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                PinCode = address.PinCode,
                Landmark = address.Landmark,
                IsDefault = address.IsDefault,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                MainLabel = address.MainLabel
            };
        }

        public async Task<AddressViewDto?> AddAddressAsync(AddressAddDto dto, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "id");
            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (userIdClaim == null || roleClaim?.Value?.ToLower() != "customer")
                return null;

            int userId = int.Parse(userIdClaim.Value);

            var address = new Address
            {
                UserId = userId,
                AddressLine1 = dto.AddressLine1,
                AddressLine2 = dto.AddressLine2,
                City = dto.City,
                State = dto.State,
                PinCode = dto.PinCode,
                Landmark = dto.Landmark,
                IsDefault = dto.IsDefault,
                Latitude = dto.Latitude,
                Longitude = dto.Longitude,
              
            };

            _context.Addresses.Add(address);
            await _context.SaveChangesAsync();

            return new AddressViewDto
            {
                AddressId = address.AddressId,
                UserId = address.UserId,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                State = address.State,
                PinCode = address.PinCode,
                Landmark = address.Landmark,
                IsDefault = address.IsDefault,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                
            };
        }

        public async Task UpdateAsync(Address address)
        {
            _context.Addresses.Update(address);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
    }
}
