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
        private readonly IMenuItemRepository _menuRepo;

        public MenuItemController(IMenuItemRepository menuRepo)
        {
            _menuRepo = menuRepo;
        }

        [HttpPost]
        [Authorize(Roles = "restaurant")]
        public async Task<IActionResult> Create(MenuItemDto dto)
        {
            var item = await _menuRepo.CreateAsync(dto, User);
            if (item == null)
                return BadRequest("Invalid data or restaurant not verified.");

            return CreatedAtAction(nameof(GetById), new { id = item.ItemId }, ToViewDto(item));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _menuRepo.GetAllAsync();
            return Ok(items.Select(ToViewDto));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _menuRepo.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(ToViewDto(item));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "restaurant")]
        public async Task<IActionResult> Update(int id, MenuItemDto dto)
        {
            var item = await _menuRepo.UpdateAsync(id, dto, User);
            if (item == null)
                return Unauthorized("Invalid or unauthorized update.");

            return Ok(ToViewDto(item));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "restaurant")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _menuRepo.DeleteAsync(id, User);
            if (!success)
                return Unauthorized("Invalid or unauthorized delete.");

            return Ok("Deleted successfully.");
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string pinCode,
            [FromQuery] string? restaurantName,
            [FromQuery] string? itemName,
            [FromQuery] string? category,
            [FromQuery] string? city)
        {
            var items = await _menuRepo.SearchAsync(pinCode, restaurantName, itemName, category, city);
            return Ok(items.Select(ToViewDto));
        }

        private MenuItemViewDto ToViewDto(MenuItem item)
        {
            return new MenuItemViewDto
            {
                ItemId = item.ItemId,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                IsAvailable = item.IsAvailable,
                Category = item.Category,
                RestaurantName = item.Restaurant?.User?.Name,
                FoodImage = item.FoodImage
            };
        }
    }
}
