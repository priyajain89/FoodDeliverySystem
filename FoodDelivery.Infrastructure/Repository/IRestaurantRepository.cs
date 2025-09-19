using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> SubmitRestaurantDetailsAsync(Restaurant restaurant);
        Task<List<RestaurantResponseDto>> GetAllRestaurantsAsync();

        Task<bool> UpdateRestaurantAsync(RestaurantResponseDto dto);
    }
}
