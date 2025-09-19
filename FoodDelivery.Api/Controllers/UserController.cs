using FoodDelivery.Domain.Data;
using FoodDelivery.Domain.Models;
using FoodDelivery.Infrastructure.DTO;
using FoodDelivery.Infrastructure.Services;
using FoodDelivery.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace FoodDelivery.Api.Controllers{

    [ApiController]

    [Route("api/[controller]")]

    public class UserController : ControllerBase

    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly OtpService _otpService;

        public UserController(IUserRepository userRepository, OtpService otpService, IConfiguration configuration)

        {
            _configuration = configuration;
            _userRepository = userRepository;
            _otpService = otpService;

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

            if (dto.Role.ToLower() == "admin")
            {
                var existingAdmin = await _userRepository.GetUsersByRoleAsync("Admin");
                if (existingAdmin.Any())
                {
                    return Conflict("An admin already exists in the system.");
                }
            }


            var user = new User
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                Role = dto.Role,

                IsVerified = dto.Role.ToLower() == "customer"

            };

            var created = await _userRepository.CreateUserAsync(user);

            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);

        }

        [HttpGet("role/{role}")]
        public async Task<IActionResult> GetByRole(string role)
        {
            var users = await _userRepository.GetUsersByRoleAsync(role);
            return Ok(users);
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


        //Login & OPT
        [HttpPost("login-request")]
        public async Task<IActionResult> LoginRequest([FromBody] RequestOtpDTO dto)
        {
            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized("You are not a registered user.");
            }

            await _otpService.GenerateOtpAsync(dto.Email);
            return Ok("OTP sent successfully to your email.");
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDTO dto)
        {
            var isValid = _otpService.VerifyOtp(dto.Email, dto.Otp);
            if (!isValid)
            {
                return Unauthorized("Invalid OTP or email.");
            }

            var user = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized("User not found.");
            }
            if ((user.Role.ToLower() == "admin" || user.Role.ToLower() == "customer") && user.IsVerified == true)
            {
                user.IsVerified = true;
                await _userRepository.UpdateUserAsync(user);
            }

            TokenGeneration jwtTokenString = new TokenGeneration(_configuration);
            string tokenString = jwtTokenString.GenerateJWT(user.UserId.ToString(), user.Name, user.Email, user.Role);

            return Ok(new
            {
                message = "OTP verified successfully. User is now verified.",
                token = tokenString
            });
        }

    }

}

