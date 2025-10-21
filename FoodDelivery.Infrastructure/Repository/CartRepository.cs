using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using Microsoft.EntityFrameworkCore;

public class CartRepository : ICartRepository
{
    private readonly AppDbContext _context;

    public CartRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<MenuItem?> GetMenuItemByIdAsync(int itemId)
    {
        return await _context.MenuItems.FindAsync(itemId);
    }

    public async Task<Cart?> GetCartByCustomerAndRestaurantAsync(int customerId, int? restaurantId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .FirstOrDefaultAsync(c => c.UserId == customerId && c.RestaurantId == restaurantId);
    }

    public async Task<Cart> CreateCartAsync(Cart cart)
    {
        _context.Carts.Add(cart);
        await _context.SaveChangesAsync();
        return cart;
    }

    public async Task<CartItem> AddCartItemAsync(CartItem cartItem)
    {
        _context.CartItems.Add(cartItem);
        await _context.SaveChangesAsync();
        return cartItem;
    }


    public async Task<Cart?> GetCartWithItemsAsync(int customerId)
    {
        return await _context.Carts
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Item)
            .FirstOrDefaultAsync(c => c.UserId == customerId);
    }

    public async Task<List<Cart>> GetAllCartsWithItemsAsync(int customerId)
    {
        return await _context.Carts
            .Where(c => c.UserId == customerId)
            .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Item)
            .Include(c => c.Restaurant)
            .ThenInclude(r => r.User)
            .ToListAsync();
    }

    public async Task<bool> UpdateQuantityAsync(int cartItemId, int quantity)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem == null) return false;

        cartItem.Quantity = quantity;
        await _context.SaveChangesAsync();
        return true;
    }


    public async Task<bool> RemoveItemAsync(int cartItemId)
    {
        var cartItem = await _context.CartItems.FindAsync(cartItemId);
        if (cartItem == null) return false;

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
        return true;
    }



}
