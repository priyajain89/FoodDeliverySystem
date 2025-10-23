using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FoodDelivery.Infrastructure.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly AppDbContext _context;

        private readonly IFileService _fileService;

        public RestaurantRepository(AppDbContext context, IFileService fileService)

        {
            _context = context;
            _fileService = fileService;
        }

        public async Task<Restaurant> SubmitRestaurantDetailsAsync(Restaurant restaurant, IFormFile? fssaiImage, IFormFile? TradelicenseImage)
        {
            var user = await _context.Users.FindAsync(restaurant.UserId);
            if (user == null || user.Role?.ToLower() != "restaurant")
                return null;


            if (fssaiImage != null)
                restaurant.FssaiImage = await _fileService.SaveFileAsync(fssaiImage, "fssai-docs");

            if (TradelicenseImage != null)
                restaurant.TradelicenseImage = await _fileService.SaveFileAsync(TradelicenseImage, "trade-docs");


            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }

        //public async Task<List<RestaurantIDDto>> GetAllRestaurantsAsync()
        //{
        //    var restaurants = await _context.Restaurants
        //        .Select(r => new RestaurantIDDto
        //        {
        //            RestaurantId = r.RestaurantId,
        //            UserId = r.UserId ?? 0,

        //            Address = r.Address,
        //            FssaiId = r.FssaiId,
        //            PinCode = r.PinCode,
        //            FssaiImage = r.FssaiImage,
        //            TradelicenseImage = r.TradelicenseImage,
        //            TradeId = r.TradeId,
        //        })
        //        .ToListAsync();

        //    return restaurants;
        //}

        //public async Task<List<RestaurantDto>> GetAllRestaurantsAsync()
        //{
        //    var restaurants = await _context.Users
        //        .Where(u => u.Role == "Restaurant") // Optional: filter by role
        //        .Select(u => new RestaurantDto
        //        {
        //            UserId = u.UserId,
        //            Name = u.Name,
        //            Email = u.Email,
        //            Phone = u.Phone,

        //            Role = u.Role,
        //            SubmittedRestaurants = u.Restaurants.Select(r => new RestaurantGetResponseDto
        //            {
        //                Address = r.Address,
        //                FssaiId = r.FssaiId,
        //                PinCode = r.PinCode,

        //                TradeId = r.TradeId
        //            }).ToList()
        //        })
        //        .ToListAsync();

        //    return restaurants;
        //}

        public async Task<List<RestaurantDto>> GetAllRestaurantsAsync()
        {
            var restaurants = await _context.Users
                .Where(u => u.Role == "Restaurant" && u.IsVerified == true)
                .Select(u => new RestaurantDto
                {
                    UserId = u.UserId,
                    Name = u.Name,
                    Email = u.Email,
                    Phone = u.Phone,
                    Role = u.Role,
                    SubmittedRestaurants = u.Restaurants.Select(r => new RestaurantGetResponseDto
                    {
                        Address = r.Address,
                        FssaiId = r.FssaiId,
                        PinCode = r.PinCode,
                        TradeId = r.TradeId,
                        OrderCount = _context.Orders.Count(o => o.RestaurantId == r.RestaurantId)
                    }).ToList()
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
