using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository _adminRepo;

        public AdminController(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

       
        [HttpGet("unverified/restaurants")]
        public async Task<IActionResult> GetUnverifiedRestaurants()
        {
            var users = await _adminRepo.GetUnverifiedRestaurantsAsync();
            return Ok(users);
        }


        [HttpGet("unverified/agents")]
        public async Task<IActionResult> GetUnverifiedAgents()
        {
            var users = await _adminRepo.GetUnverifiedAgentsAsync();
            return Ok(users);
        }

       
        [HttpPut("verify")]
        public async Task<IActionResult> VerifyUser([FromBody] VerifyUserDto dto)
        {
            var success = await _adminRepo.VerifyUserAsync(dto.UserId, dto.Role);
            if (!success) return NotFound("User not found or role mismatch.");
            return Ok(new { success = true, message = $"{dto.Role} with ID {dto.UserId} verified successfully." });
        }
    }
}
