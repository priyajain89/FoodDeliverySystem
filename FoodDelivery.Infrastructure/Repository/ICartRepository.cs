using FoodDelivery.Domain.Models;
public interface ICartRepository
{
    Task<User?> GetUserByIdAsync(int userId);
    Task<MenuItem?> GetMenuItemByIdAsync(int itemId);
    Task<Cart?> GetCartByCustomerAndRestaurantAsync(int customerId, int? restaurantId);
    Task<Cart> CreateCartAsync(Cart cart);
    Task<CartItem> AddCartItemAsync(CartItem cartItem);
    Task<Cart?> GetCartWithItemsAsync(int customerId);
    Task<List<Cart>> GetAllCartsWithItemsAsync(int customerId);

    Task<bool> UpdateQuantityAsync(int cartItemId, int quantity);
    Task<bool> RemoveItemAsync(int cartItemId);

}

