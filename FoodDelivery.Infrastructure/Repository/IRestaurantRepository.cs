using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;

namespace FoodDelivery.Infrastructure.Repository
{
    public interface IRestaurantRepository
    {
        Task<Restaurant> SubmitRestaurantDetailsAsync(Restaurant restaurant);
    }
}
