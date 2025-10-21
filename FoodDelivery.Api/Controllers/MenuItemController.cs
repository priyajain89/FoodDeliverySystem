using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers

{

    [ApiController]

    [Route("api/[controller]")]

    public class MenuItemController : ControllerBase
    {
        private readonly IMenuItemRepository _repo;
        public MenuItemController(IMenuItemRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("add-item")]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> Create([FromForm] MenuItemCreateDto dto)
        {
            var userIdClaim = User.FindFirst("id")?.Value;
            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))
                return Unauthorized("Invalid token.");
            var result = await _repo.CreateAsync(dto, userId);
            if (result == null)
                return BadRequest("Restaurant not verified or category invalid.");
            return Ok(result);
        }

        [HttpGet("get-by{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _repo.GetByIdAsync(id);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("get-by-restaurant/{restaurantId}")]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> GetByRestaurant(int restaurantId)
        {
            var items = await _repo.GetByRestaurantIdAsync(restaurantId);

            var result = items.Select(item => new MenuItemViewDto
            {
                ItemId = item.ItemId,
                Name = item.Name ?? string.Empty,
                Description = item.Description,
                Price = item.Price ?? 0,
                IsAvailable = item.IsAvailable ?? false,
                Category = item.Category,
                FoodImage = item.FoodImage,
                RestaurantId = item.RestaurantId ?? 0,
                RestaurantName = item.Restaurant?.User?.Name ?? "Unknown"
            });

            return Ok(result);
        }


        [HttpGet("get-all-Item")]

        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpPut("update-ItemBy{id}")]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> Update(int id, [FromBody] MenuItemUpdateDto dto)
        {
            var success = await _repo.UpdateAsync(id, dto);
            if (!success)
                return BadRequest("Update failed.");
            return NoContent();
        }

        [HttpDelete("delete-ById{id}")]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repo.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }



        [HttpGet("search-by-pincode")]
        public async Task<IActionResult> SearchByPinCode([FromQuery] string pinCode)
        {
            var items = await _repo.SearchByPinCodeAsync(pinCode);

            var result = items.Select(item => new MenuItemViewDto
            {
                ItemId = item.ItemId,
                Name = item.Name ?? string.Empty,
                Description = item.Description,
                Price = item.Price ?? 0,
                IsAvailable = item.IsAvailable ?? false,
                Category = item.Category,
                FoodImage = item.FoodImage,
                RestaurantName = item.Restaurant?.User?.Name ?? "Unknown",
            });

            return Ok(result);
        }

        [HttpGet("search-by-filters")]
        public async Task<IActionResult> SearchByFilters(
    [FromQuery] string pinCode,
    [FromQuery] string? restaurantName,
    [FromQuery] string? itemName,
    [FromQuery] string? category,
    [FromQuery] string? city)
        {
            if (string.IsNullOrWhiteSpace(pinCode))
            {
                return BadRequest("Pin code is required.");
            }

            var items = await _repo.SearchByFiltersAsync(pinCode, restaurantName, itemName, category, city);

            var result = items.Select(item => new MenuItemViewDto
            {
                ItemId = item.ItemId,
                Name = item.Name ?? string.Empty,
                Description = item.Description,
                Price = item.Price ?? 0,
                IsAvailable = item.IsAvailable ?? false,
                Category = item.Category,
                FoodImage = item.FoodImage,
                RestaurantName = item.Restaurant?.User?.Name ?? "Unknown",
            });

            return Ok(result);
        }
    }
}
