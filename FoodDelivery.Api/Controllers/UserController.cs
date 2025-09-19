using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Create([FromBody] UserDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailOrPhoneAsync(dto.Email, dto.Phone);
            if (existingUser != null)
            {
                return Conflict("User with this email or phone already exists.");
            }

            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role,
                IsVerified = dto.Role.ToLower() == "customer"
                //IsVerified = dto.Role.Equals("Customer", StringComparison.OrdinalIgnoreCase)
            };

            var created = await _userRepository.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            user.Name = dto.Name;
            user.Email = dto.Email;
            user.Phone = dto.Phone;
            user.Role = dto.Role;

            var updated = await _userRepository.UpdateUserAsync(user);
            return Ok(updated);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _userRepository.DeleteUserAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
    }
}
