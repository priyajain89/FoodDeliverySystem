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

            return Unauthorized("Only customers can add items to cart.");

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

        var savedItem = await _cartRepository.AddCartItemAsync(cartItem);

        return Ok(new

        {

            cart.CartId,

            Message = "Item added to cart.",

            CartItemId = savedItem.CartItemId

        });

    }

    [HttpGet("customer-carts")]

    public async Task<IActionResult> GetCustomerCarts()

    {

        var customerIdClaim = User.FindFirst("id")?.Value;

        if (string.IsNullOrEmpty(customerIdClaim))

            return Unauthorized("CustomerId not found in token.");

        var customerId = int.Parse(customerIdClaim);

        var carts = await _cartRepository.GetAllCartsWithItemsAsync(customerId);

        if (carts == null || !carts.Any())

            return NotFound("No carts found.");


        var result = carts.Select(cart => new CartViewDto

        {

            CartId = cart.CartId,

            CustomerId = cart.UserId ?? 0,

            RestaurantId = cart.RestaurantId ?? 0,

            RestaurantName = cart.Restaurant?.User?.Name ?? "Unknown",

            Items = cart.CartItems.Select(ci => new CartItemDto

            {

                ItemId = ci.ItemId ?? 0,

                Name = ci.Item?.Name ?? string.Empty,

                Description = ci.Item?.Description,

                Price = ci.Item?.Price ?? 0,

                Quantity = ci.Quantity ?? 0,

                Category = ci.Item?.Category,

                FoodImage = ci.Item?.FoodImage

            }).ToList()

        }).ToList();

        return Ok(result);

    }

}

