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

        /// <summary>
        /// Get all unverified restaurants who submitted details.
        /// </summary>
        [HttpGet("unverified/restaurants")]
        public async Task<IActionResult> GetUnverifiedRestaurants()
        {
            var users = await _adminRepo.GetUnverifiedRestaurantsAsync();
            return Ok(users);
        }




        /// <summary>
        /// Get all unverified delivery agents who submitted details.
        /// </summary>
        [HttpGet("unverified/agents")]
        public async Task<IActionResult> GetUnverifiedAgents()
        {
            var users = await _adminRepo.GetUnverifiedAgentsAsync();
            return Ok(users);
        }

        /// <summary>
        /// Verify a user by ID and role.
        /// </summary>
        [HttpPut("verify")]
        public async Task<IActionResult> VerifyUser([FromBody] VerifyUserDto dto)
        {
            var success = await _adminRepo.VerifyUserAsync(dto.UserId, dto.Role);
            if (!success) return NotFound("User not found or role mismatch.");
            return Ok($"{dto.Role} with ID {dto.UserId} verified successfully.");
        }
    }
}
