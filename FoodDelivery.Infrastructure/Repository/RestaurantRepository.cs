using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly AppDbContext _context;

        public RestaurantRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Restaurant> SubmitRestaurantDetailsAsync(Restaurant restaurant)
        {
            var user = await _context.Users.FindAsync(restaurant.UserId);
            if (user == null || user.Role?.ToLower() != "restaurant")
                return null;

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        public async Task<List<RestaurantIDDto>> GetAllRestaurantsAsync()
        {
            var restaurants = await _context.Restaurants
                .Select(r => new RestaurantIDDto
                {
                    RestaurantId = r.RestaurantId,
                    UserId = r.UserId ?? 0,
                    Address = r.Address,
                    FssaiId = r.FssaiId,
                    PinCode = r.PinCode,
                    FssaiImage = r.FssaiImage,
                    TradelicenseImage = r.TradelicenseImage,
                    TradeId = r.TradeId,
                })
                .ToListAsync();

            return restaurants;
        }

        public async Task<bool> UpdateRestaurantAsync(RestaurantIDDto dto)
        {
            var restaurant = await _context.Restaurants.FindAsync(dto.RestaurantId);
            if (restaurant == null) return false;

            restaurant.Address = dto.Address;
            restaurant.FssaiId = dto.FssaiId;
            restaurant.PinCode = dto.PinCode;
            restaurant.FssaiImage = dto.FssaiImage;
            restaurant.TradelicenseImage = dto.TradelicenseImage;
            restaurant.TradeId = dto.TradeId;

            _context.Restaurants.Update(restaurant);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}
