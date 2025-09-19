//using FoodDelivery.Domain.Models;
//using FoodDelivery.Infrastructure.DTO;
//using FoodDelivery.Infrastructure.Repository;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using System.Data;

//namespace FoodDelivery.Api.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class UserController : ControllerBase
//    {
//        private readonly IUserRepository _userRepository;

//        public UserController(IUserRepository userRepository)
//        {
//            _userRepository = userRepository;
//        }

//        [HttpGet]
//        public async Task<IActionResult> GetAll()
//        {
//            var users = await _userRepository.GetAllUsersAsync();
//            return Ok(users);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetById(int id)
//        {
//            var user = await _userRepository.GetUserByIdAsync(id);
//            if (user == null) return NotFound();
//            return Ok(user);
//        }

//        [HttpPost]
//        public async Task<IActionResult> Create([FromBody] UserDto dto)
//        {
//            if (!Enum.TryParse<Role>(dto.Role, true, out var parsedRole))
//            {
//                return BadRequest("Invalid role specified.");
//            }

//            var user = new User
//            {
//                Name = dto.Name,
//                Email = dto.Email,
//                Phone = dto.Phone,
//                Role = parsedRole,
//                IsVerified = parsedRole == Role.Customer
//            };

//            var created = await _userRepository.CreateUserAsync(user);
//            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> Update(int id, UserDto dto)
//        {
//            var user = await _userRepository.GetUserByIdAsync(id);
//            if (user == null) return NotFound();

//            if (!Enum.TryParse<Role>(dto.Role, true, out var parsedRole))
//            {
//                return BadRequest("Invalid role specified.");
//            }

//            user.Name = dto.Name;
//            user.Email = dto.Email;
//            user.Phone = dto.Phone;
//            user.Role = parsedRole;

//            var updated = await _userRepository.UpdateUserAsync(user);
//            return Ok(updated);
//        }


//        [HttpDelete("{id}")]
//        public async Task<IActionResult> Delete(int id)
//        {
//            var success = await _userRepository.DeleteUserAsync(id);
//            if (!success) return NotFound();
//            return NoContent();
//        }
//    }

//}
