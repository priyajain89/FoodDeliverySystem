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


        [HttpPost]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> Create([FromBody] MenuItemCreateDto dto)
        {
            var userIdClaim = User.FindFirst("id")?.Value;

            if (userIdClaim == null || !int.TryParse(userIdClaim, out int userId))

                return Unauthorized("Invalid token.");

            var result = await _repo.CreateAsync(dto, userId);

            if (result == null)

                return BadRequest("Restaurant not verified or category invalid.");

            return Ok(result);

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _repo.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(

              [FromQuery] string pinCode,
              [FromQuery] string? restaurantName,
              [FromQuery] string? itemName,             
              [FromQuery] string? category,
              [FromQuery] string? city)

        {
            var items = await _repo.SearchAsync(pinCode, restaurantName, itemName, category, city);
            return Ok(items.Select(item => new MenuItemViewDto
            {
                ItemId = item.ItemId,
                Name = item.Name ?? string.Empty,
                Description = item.Description,
                Price = item.Price ?? 0,
                IsAvailable = item.IsAvailable ?? false,
                Category = item.Category,
                FoodImage = item.FoodImage,
                RestaurantName = item.Restaurant?.User?.Name ?? "Unknown",
            }));

        }


        [HttpPut("{id}")]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> Update(int id, [FromBody] MenuItemUpdateDto dto)
        {
            var success = await _repo.UpdateAsync(id, dto);
            if (!success)
                return BadRequest("Update failed.");
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Restaurant")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _repo.DeleteAsync(id);
            if (!success)
                return NotFound();
            return NoContent();
        }

       
    }


}



