using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class CartController : ControllerBase
{
    private readonly ICartRepository _cartRepository;

    public CartController(ICartRepository cartRepository)
    {
        _cartRepository = cartRepository;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
    {
        var customerIdClaim = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(customerIdClaim))
            return Unauthorized("CustomerId not found in token.");

        var customerId = int.Parse(customerIdClaim);

        var user = await _cartRepository.GetUserByIdAsync(customerId);
        if (user == null || user.Role?.ToLower() != "customer")
            return Unauthorized("Only verified customers can add items to cart.");

        var menuItem = await _cartRepository.GetMenuItemByIdAsync(dto.ItemId);
        if (menuItem == null)
            return NotFound("Menu item not found.");

        var restaurantId = menuItem.RestaurantId;

        var cart = await _cartRepository.GetCartByCustomerAndRestaurantAsync(customerId, restaurantId);
        if (cart == null)
        {
            cart = new Cart
            {
                UserId = customerId,
                RestaurantId = restaurantId
            };
            cart = await _cartRepository.CreateCartAsync(cart);
        }

        var cartItem = new CartItem
        {
            CartId = cart.CartId,
            ItemId = dto.ItemId,
            Quantity = dto.Quantity
        };

        await _cartRepository.AddCartItemAsync(cartItem);

        return Ok("Item added to cart.");
    }


    [HttpGet("customer-cart")]
    public async Task<IActionResult> GetCustomerCart()
    {
        var customerIdClaim = User.FindFirst("id")?.Value;
        if (string.IsNullOrEmpty(customerIdClaim))
            return Unauthorized("CustomerId not found in token.");

        var customerId = int.Parse(customerIdClaim);
        var cart = await _cartRepository.GetCartWithItemsAsync(customerId);

        if (cart == null)
            return NotFound("Cart not found.");

        return Ok(cart);
    }
}
