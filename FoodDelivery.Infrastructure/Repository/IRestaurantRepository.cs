using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> SubmitRestaurantDetailsAsync(Restaurant restaurant, IFormFile? fssaiImage, IFormFile? TradelicenseImage);
        Task<List<RestaurantIDDto>> GetAllRestaurantsAsync();
        Task<bool> UpdateRestaurantAsync(RestaurantIDDto dto);

        Task<RestaurantIDDto?> GetRestaurantByUserIdAsync(int userId);
    }
}
