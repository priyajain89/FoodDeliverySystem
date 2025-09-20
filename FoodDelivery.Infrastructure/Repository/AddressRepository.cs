using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FoodDelivery.Infrastructure.Repository
{
    public class AddressRepository : IAddressRepository
    {
        private readonly AppDbContext _context;
        private readonly IGeocodingService _geocodingService;

        public AddressRepository(AppDbContext context, IGeocodingService geocodingService)
        {
            _context = context;
            _geocodingService = geocodingService;
        }

        public async Task<IEnumerable<AddressViewDto>> GetAllByUserIdAsync( int userId)
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
                Longitude = a.Longitude
            });
        }

        public async Task<AddressViewDto?> GetByUserIdForCustomerAsync(int addressId, int userId)
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
                Longitude = address.Longitude
            };
        }

        public async Task<AddressViewDto?> AddAddressAsync(AddressAddDto dto, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "id");
            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);

            if (userIdClaim == null || roleClaim?.Value?.ToLower() != "customer")
                return null;

            int userId = int.Parse(userIdClaim.Value);


            var fullAddress = string.Join(", ", new[]
            {
    dto.AddressLine1,
    dto.AddressLine2,
    dto.Landmark,
    dto.City,
    dto.State,
    dto.PinCode?.ToString()
}.Where(x => !string.IsNullOrWhiteSpace(x)));


            // Get coordinates from geocoding service
            var geoResult = await _geocodingService.GetCoordinatesAsync(fullAddress);

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
                Latitude = geoResult?.Latitude,
                Longitude = geoResult?.Longitude

            };

            _context.Addresses.Add(address);

            var dbUser = await _context.Users.Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (dbUser != null)
            {
                dbUser.Addresses.Add(address);
            }

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
                Longitude = address.Longitude
            };
        }

        public async Task<bool> UpdateAsync(int addressId, AddressAddDto dto, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == "id");
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                return false;

            var existingAddress = await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId && a.UserId == userId);

            if (existingAddress == null)
                return false;

            existingAddress.AddressLine1 = dto.AddressLine1;
            existingAddress.AddressLine2 = dto.AddressLine2;
            existingAddress.City = dto.City;
            existingAddress.State = dto.State;
            existingAddress.PinCode = dto.PinCode;
            existingAddress.Landmark = dto.Landmark;
            existingAddress.IsDefault = dto.IsDefault;
       

            _context.Entry(existingAddress).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var address = await _context.Addresses.FindAsync(id);
            if (address != null)
            {
                var dbUser = await _context.Users.Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.UserId == address.UserId);

                if (dbUser != null)
                {
                    dbUser.Addresses.Remove(address);
                }

                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
        }
    }
}
