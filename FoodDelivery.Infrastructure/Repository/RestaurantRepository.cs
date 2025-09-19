using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;

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
                throw new InvalidOperationException("Invalid restaurant user.");

            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();
            return restaurant;
        }
    }
}
